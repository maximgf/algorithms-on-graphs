using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Graph
{
    static public class Graph<T> where T : INumber<T>
    {

        public static T[,] Copy(T[,] graf)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            T[,] grafCopy = new T[rows, colomns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colomns; j++)
                {
                    grafCopy[i, j] = graf[i, j];
                }
            }
            return grafCopy;
        }

        public static void Print(T[,] graf)
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

        public static T[,] DeleteColomn(T[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;

            T[,] newgraf = new T[rows, colomns - 1];
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

        public static T[,] DeleteRow(T[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            T[,] newgraf = new T[rows - 1, colomns];
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

        public static T[,] ReadGraph(string pathFile)
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
            T[,] adjacencyMatrix = new T[maxIndex, maxIndex];

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
                T weight = (T)Convert.ChangeType(parts[2], typeof(T));

                adjacencyMatrix[from, to] = weight;
            }
            return adjacencyMatrix;
        }


    }
}
