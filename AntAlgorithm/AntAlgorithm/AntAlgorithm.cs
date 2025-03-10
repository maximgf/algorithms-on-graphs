using System;
using System.IO;

namespace AntColony
{
    public class AntAlgorithm
    {
        // Константы для формул 
        private double alpha;
        private double beta;
        private double q;
        private double evaporationRate;
        
        // Инструменты для работы алгоритма
        private double[,] graph;
        private int nodeCount;
        private double[,] pheromones;
        private Random random;
        
        // Для логгирования
        private double[] bestPathLengths;
        private double[] currentPathLengths;
        private double[] bestPathProbabilities;

        public AntAlgorithm(double[,] graph, double alpha = 1, double beta = 1, double q = 5, double evaporationRate = 0.2)
        {
            this.graph = graph;
            this.alpha = alpha;
            this.beta = beta;
            this.q = q;
            this.evaporationRate = evaporationRate;
            nodeCount = graph.GetLength(0);
            pheromones = new double[nodeCount, nodeCount];
            random = new Random();
    
            InitializePheromones();
        }

        //Для корректной работы формулы инициализруется небольшое значения для каждого пути в графе
        private void InitializePheromones()
        {
            for (int row = 0; row < nodeCount; row++)
            {
                for (int col = 0; col < nodeCount; col++)
                {
                    pheromones[row, col] = 0.1;
                }
            }
        }

        // iterations - количество муравьев, которых поочередно запускают
        public int[] Run(int iterations = 10000)
        {
            int[] bestPath = new int[nodeCount];
            double bestPathLength = double.MaxValue;
            bestPathLengths = new double[iterations];
            currentPathLengths = new double[iterations];
            bestPathProbabilities = new double[iterations];

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                int[] path = GeneratePath();
                double pathLength = CalculatePathLength(path);

                UpdateBestPath(path, pathLength, ref bestPath, ref bestPathLength);

                bestPathLengths[iteration] = bestPathLength;
                currentPathLengths[iteration] = pathLength;
                bestPathProbabilities[iteration] = CalculatePathProbability(bestPath);

                UpdatePheromones(path, pathLength);
                EvaporatePheromones();
            }

            return bestPath;
        }

        // Иммитация прохода муравья
        private int[] GeneratePath()
        {
            int[] path = new int[nodeCount];
            bool[] visited = new bool[nodeCount];
            int startNode = random.Next(nodeCount);
            int currentNode = startNode;

            for (int node = 0; node < nodeCount; node++)
            {
                path[node] = currentNode;
                currentNode = ChooseNextNode(currentNode, visited, startNode);
                visited[currentNode] = true;
            }

            return path;
        }

        // На вероятностной основе выбирается следующий узел
        private int ChooseNextNode(int currentNode, bool[] visited, int startNode)
        {
            double[] probabilities = CalculateProbabilities(currentNode, visited, startNode);
            return SelectNextNode(probabilities);
        }

        //
        private double[] CalculateProbabilities(int currentNode, bool[] visited, int startNode)
        {
            double[] probabilities = new double[nodeCount];
            double normalizer = 0.0;

            for (int nextNode = 0; nextNode < nodeCount; nextNode++)
            {
                if (visited[nextNode] || graph[currentNode, nextNode] == double.PositiveInfinity || nextNode == startNode)
                {
                    probabilities[nextNode] = 0.0;
                    continue;
                }
                // Формула коэффициента выбора узла
                probabilities[nextNode] = Math.Pow(pheromones[currentNode, nextNode], alpha) * Math.Pow(1 / graph[currentNode, nextNode], beta);
                // Сумма всех коэффициентов
                normalizer += probabilities[nextNode];
            }

            // Сценарий тупика
            if (normalizer == 0)
            {
                return probabilities;
            }

            // Расчет вероятности для каждого узла
            for (int i = 0; i < nodeCount; i++)
            {
                probabilities[i] /= normalizer;
            }

            return probabilities;
        }
        
        // Генерируем рандомное число, суммируем все вероятности узлов, пока не станем больше rand
        private int SelectNextNode(double[] probabilities)
        {
            double rand = random.NextDouble();
            double cumulativeProbability = 0.0;

            for (int nextNode = 0; nextNode < nodeCount; nextNode++)
            {
                cumulativeProbability += probabilities[nextNode];
                if (rand < cumulativeProbability)
                {
                    return nextNode;
                }
            }

            return 0;
        }

        private void UpdateBestPath(int[] path, double pathLength, ref int[] bestPath, ref double bestPathLength)
        {
            if (pathLength < bestPathLength)
            {
                Array.Copy(path, bestPath, nodeCount);
                bestPathLength = pathLength;
            }
        }

        public double CalculatePathLength(int[] path)
        {
            double sum = 0;
            for (int node = 0; node < nodeCount - 1; node++)
            {
                sum += graph[path[node], path[node + 1]];
            }
            sum += graph[path[nodeCount - 1], path[0]];
            return sum;
        }

        private void UpdatePheromones(int[] path, double pathLength)
        {
            for (int node = 0; node < nodeCount - 1; node++)
            {
                // Формула вичисления значения феромона
                pheromones[path[node], path[node + 1]] += q / pathLength;
            }
            // Возвращение в начало
            pheromones[path[nodeCount - 1], path[0]] += q / pathLength;
        }

        private void EvaporatePheromones()
        {
            for (int row = 0; row < nodeCount; row++)
            {
                for (int col = 0; col < nodeCount; col++)
                {
                    // Учет испарения феромонов
                    pheromones[row, col] *= (1 - evaporationRate);
                }
            }
        }


        // Для логгирования
        private double CalculatePathProbability(int[] path)
        {
            double probability = 1.0;
            for (int node = 0; node < nodeCount - 1; node++)
            {
                int from = path[node];
                int to = path[node + 1];
                double pheromone = pheromones[from, to];
                double distance = graph[from, to];
                probability *= Math.Pow(pheromone, alpha) * Math.Pow(1 / distance, beta);
            }
            return probability;
        }

        // Для логгирования
        public void SavePathsToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Iteration\tBest Path Length\tCurrent Path Length\tProbability");
                for (int i = 0; i < bestPathLengths.Length; i++)
                {
                    writer.WriteLine($"{i}\t{bestPathLengths[i]}\t{currentPathLengths[i]}\t{bestPathProbabilities[i]}");
                }
            }
        }
    }
}