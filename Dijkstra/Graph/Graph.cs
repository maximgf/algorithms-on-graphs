namespace Graf

{
    static public class Graf
    {
        public static int SimpleShortest(int[,] graf)
        {

            int[,] grafcopy = Copy(graf);
            int rows = grafcopy.GetUpperBound(0) + 1;
            int cols = grafcopy.GetUpperBound(1) + 1;
            int path = 0;

            for (int i = 0; i < rows; i++)
            {
                int temp = 1000000000;
                int node = 0;
                for (int j = 0; j < cols; j++)
                {

                    if (graf[i, j] != 0 && temp > graf[i, j])
                    {
                        temp = graf[i, j];
                        node = i;
                    }
                }
                DeleteColomn(grafcopy, node);
                if (temp == 1000000000)
                {
                    temp = 0;
                }
                path += temp;


            }
            return path;
        }

        public static int[,] Copy(int[,] graf)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            int[,] grafCopy = new int[rows, colomns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colomns; j++)
                {
                    grafCopy[i, j] = graf[i, j];
                }
            }
            return grafCopy;
        }
        public static void Print(int[,] graf)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colomns; j++)
                {
                    Console.Write($"{graf[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public static int[,] DeleteColomn(int[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;

            int[,] newgraf = new int[rows, colomns - 1];
            for (int i = 0; i < rows; i++)
            {
                int jO = -1;
                for (int j = 0; j < colomns - 1; j++)
                {
                    jO++;
                    if (j == index)
                    {
                        jO++;
                    }

                    newgraf[i, j] = graf[i, jO];

                }

            }
            return newgraf;
        }

        public static int[,] DeleteRow(int[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            int[,] newgraf = new int[rows - 1, colomns];
            int iO = -1;
            for (int i = 0; i < rows - 1; i++)
            {
                iO++;
                if (i == index)
                {
                    iO++;
                }
                for (int j = 0; j < colomns; j++)
                {

                    newgraf[i, j] = graf[iO, j];

                }

            }
            return newgraf;
        }

        public static double[,] ReadGraph(string pathFile)
        {
            string[] lines = File.ReadAllLines(pathFile);
 
            Dictionary<int, int> maxIndices = new Dictionary<int, int>();
 
            foreach (var line in lines)
            {
                string[] parts = line.Split(' ');
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);

                if (!maxIndices.ContainsKey(from) || maxIndices[from] < to)
                {
                    maxIndices[from] = to;
                }

                if (!maxIndices.ContainsKey(to) || maxIndices[to] < from)
                {
                    maxIndices[to] = from;
                }
            }
 
            int maxIndex = maxIndices.Keys.Max() + 1;
            double[,] adjacencyMatrix = new double[maxIndex, maxIndex];
 
            foreach (var line in lines)
            {
                string[] parts = line.Split(' ');
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                double weight = double.Parse(parts[2]);

                adjacencyMatrix[from, to] = weight;
            }
            return adjacencyMatrix;
        }

        public static int Dijkstra(int[,] graf, int entry, int end)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int cols = graf.GetUpperBound(1) + 1;


            int[] distances = new int[rows];
            for (int i = 0; i < rows; i++)
            {
                distances[i] = int.MaxValue;
            }
            distances[entry] = 0;

            bool[] visited = new bool[rows];

            for (int count = 0; count < rows - 1; count++)
            {

                int u = MinDistance(distances, visited);

                visited[u] = true;

                for (int v = 0; v < cols; v++)
                {
                    if (!visited[v] && graf[u, v] != 0 && distances[u] != int.MaxValue &&
                        distances[u] + graf[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + graf[u, v];
                    }
                }
            }

            return distances[end];
        }

        private static int MinDistance(int[] distances, bool[] visited)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < distances.Length; v++)
            {
                if (!visited[v] && distances[v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }
    }
}