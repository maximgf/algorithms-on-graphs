using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Path = System.IO.Path;

namespace Graph
{
    public static class Graph
    {
        public static double[,] Copy(double[,] graph)
        {
            int rows = graph.GetUpperBound(0) + 1;
            int columns = graph.GetUpperBound(1) + 1;
            double[,] graphCopy = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    graphCopy[i, j] = graph[i, j];
                }
            }
            return graphCopy;
        }

        public static void Print(double[,] graph)
        {
            int rows = graph.GetUpperBound(0) + 1;
            int columns = graph.GetUpperBound(1) + 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write($"{graph[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public static double[,] DeleteColumn(double[,] graph, int index)
        {
            int rows = graph.GetUpperBound(0) + 1;
            int columns = graph.GetUpperBound(1) + 1;

            double[,] newGraph = new double[rows, columns - 1];
            for (int i = 0; i < rows; i++)
            {
                int jO = 0;
                for (int j = 0; j < columns; j++)
                {
                    if (j != index)
                    {
                        newGraph[i, jO] = graph[i, j];
                        jO++;
                    }
                }
            }
            return newGraph;
        }

        public static double[,] DeleteRow(double[,] graph, int index)
        {
            int rows = graph.GetUpperBound(0) + 1;
            int columns = graph.GetUpperBound(1) + 1;
            double[,] newGraph = new double[rows - 1, columns];
            int iO = 0;
            for (int i = 0; i < rows; i++)
            {
                if (i != index)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        newGraph[iO, j] = graph[i, j];
                    }
                    iO++;
                }
            }
            return newGraph;
        }

        public static double[,] ReadGraph(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            string[] lines = File.ReadAllLines(pathFile);
            Dictionary<int, int> maxIndices = new Dictionary<int, int>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                if (parts.Length != 3)
                {
                    throw new FormatException($"Invalid input format in line: '{line}'. Expected format: 'from to weight'.");
                }

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

            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < maxIndex; j++)
                {
                    adjacencyMatrix[i, j] = double.PositiveInfinity;
                }
            }

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(' ');
                if (parts.Length != 3)
                {
                    throw new FormatException($"Invalid input format in line: '{line}'. Expected format: 'from to weight'.");
                }

                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                double weight = double.Parse(parts[2]);

                adjacencyMatrix[from, to] = weight;
            }

            return adjacencyMatrix;
        }
    }
}