using System;
using System.IO;

namespace AntColony
{
    public class AntAlgorithm
    {
        private double alpha;
        private double beta;
        private double Q;
        private double Kevaporation;
        private double[,] graf;
        private int nodeCounts;
        private double[,] Pheromones;
        private Random random;
        private double[] BesthPathVector;
        private double[] CurrentPathVector;
        private double[] BesthPathProbalityVector;

        public AntAlgorithm(double[,] graf, double alpha = 1, double beta = 1, double Q = 5, double Kevaporation = 0.2)
        {
            this.graf = graf;
            this.alpha = alpha;
            this.beta = beta;
            this.Q = Q;
            this.Kevaporation = Kevaporation;
            nodeCounts = graf.GetLength(0);
            Pheromones = new double[nodeCounts, nodeCounts];
            random = new Random();

            for (int row = 0; row < nodeCounts; row++)
            {
                for (int col = 0; col < nodeCounts; col++)
                {
                    Pheromones[row, col] = 0.1;
                }
            }


        }

        public int[] Run(int countIterations = 10000)
        {
            int[] bestPath = new int[nodeCounts];
            double bestPathLength = double.MaxValue;
            BesthPathVector = new double[countIterations];
            CurrentPathVector = new double[countIterations];
            BesthPathProbalityVector = new double[countIterations];


            for (int ant = 0; ant < countIterations; ant++)
            {
                int[] path = new int[nodeCounts];
                bool[] visited = new bool[nodeCounts];
                int startedNode = random.Next(nodeCounts);
                int currentNode = startedNode;
                for (int i = 0; i < nodeCounts; i++)
                {
                    visited[i] = false;
                }
 
                for (int node = 0; node < nodeCounts; node++)
                {
                    
                    path[node] = currentNode;
              
                    currentNode = Variable(currentNode, visited,startedNode);
 
                    visited[currentNode] = true;
                    if (currentNode == path[node])
                    {
                        break;
                    }

                }
                /*Console.Write($"{ant}: ");
                for (int node = 0;node < nodeCounts; node++)
                {
                    Console.Write($"{path[node]} ");
                }
                */
                double pathLength = PathLenght(path);
                //Console.WriteLine(pathLength);
                if (pathLength == double.PositiveInfinity)
                {

                    BesthPathVector[ant] = bestPathLength;
                    CurrentPathVector[ant] = 0;
                    continue;
                }
                if (pathLength < bestPathLength)
                {
                    bestPath = path;
                    bestPathLength = pathLength;
                }

                BesthPathVector[ant] = bestPathLength;
                CurrentPathVector[ant] = pathLength;


                BesthPathProbalityVector[ant] = CalculatePathProbability(bestPath);
                AddPheromons(path, pathLength);
                PheromonsEvaporation();
                
            }



            return bestPath;
        }

        private void AddPheromons(int[] path, double pathLength)
        {
            for (int node = 0; node < nodeCounts - 1; node++)
            {
                Pheromones[path[node], path[node + 1]] += Q / pathLength;
            }
            Pheromones[path[nodeCounts - 1], path[0]] += Q / pathLength;
        }

        public double PathLenght(int[] path)
        {
            double sum = 0;
            for (int node = 0; node < nodeCounts - 1; node++)
            {
                sum += graf[path[node], path[node + 1]];
            }
            sum += graf[path[nodeCounts - 1], path[0]];
            return sum;
        }

        private void PheromonsEvaporation()
        {
            for (int row = 0; row < nodeCounts; row++)
            {
                for (int col = 0; col < nodeCounts; col++)
                {
                    Pheromones[row, col] *= (1 - Kevaporation);
                }
            }
        }

        private int Variable(int currentNode, bool[] visited, int startedNode)
        {

            double[] probabilities = new double[nodeCounts];
            double normal = 0;

            for (int into = 0; into < nodeCounts; into++)
            {
                if (visited[into] || graf[currentNode,into] == double.PositiveInfinity || (into == startedNode))
                {
                    probabilities[into] = 0.0;
 
                    continue;
                }
                probabilities[into] = Math.Pow(Pheromones[currentNode, into], alpha) * Math.Pow(1 / graf[currentNode, into], beta);
 
                normal += probabilities[into];
            }
 
            if(normal == 0)
            {
                return currentNode;
            }
            for (int i = 0; i < nodeCounts; i++)
            {
                probabilities[i] /= normal;
            }

            double rand = random.NextDouble();
            double cumulativeProbability = 0.0;
            for (int nextNode = 0; nextNode < nodeCounts; nextNode++)
            {
                cumulativeProbability += probabilities[nextNode];
 
                if (rand < cumulativeProbability)
                {
 
                    return nextNode;
                }
            }
           
            return currentNode;
        }

        public void SavePathsToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Iteration\tBest Path Length\tCurrent Path Length\tProbability");
                for (int i = 0; i < 10000; i++)
                {
                    writer.WriteLine($"{i}\t{BesthPathVector[i]}\t{CurrentPathVector[i]}\t{BesthPathProbalityVector[i]}");
                }
            }
        }

        private double CalculatePathProbability(int[] path)
        {
            double probability = 1.0;
            for (int node = 0; node < nodeCounts - 1; node++)
            {
                int from = path[node];
                int to = path[node + 1];
                double pheromone = Pheromones[from, to];
                double distance = graf[from, to];
                probability *= Math.Pow(pheromone, alpha) * Math.Pow(1 / distance, beta);
            }
            return probability;
        }

    }
}