using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    static public class Graph
    {
        public static double[,] Copy(double[,] graf)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            double[,] grafCopy = new double[rows, colomns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colomns; j++)
                {
                    grafCopy[i, j] = graf[i, j];
                }
            }
            return grafCopy;
        }

        public static void Print(double[,] graf)
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

        public static double[,] DeleteColomn(double[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;

            double[,] newgraf = new double[rows, colomns - 1];
            for (int i = 0; i < rows; i++)
            {
                int jO = 0;
                for (int j = 0; j < colomns; j++)
                {
                    if (j != index)
                    {
                        newgraf[i, jO] = graf[i, j];
                        jO++;
                    }
                }
            }
            return newgraf;
        }

        public static double[,] DeleteRow(double[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            double[,] newgraf = new double[rows - 1, colomns];
            int iO = 0;
            for (int i = 0; i < rows; i++)
            {
                if (i != index)
                {
                    for (int j = 0; j < colomns; j++)
                    {
                        newgraf[iO, j] = graf[i, j];
                    }
                    iO++;
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
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue; // Skip empty lines
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

                // Initialize the matrix with double.PositiveInfinity to indicate no path
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
                        continue; // Skip empty lines
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