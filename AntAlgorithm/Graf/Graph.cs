using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    static public class Graph<T> where T : IComparable<T>
    {
        public static T SimpleShortest(T[,] graf)
        {
            T[,] grafcopy = Copy(graf);
            int rows = grafcopy.GetUpperBound(0) + 1;
            int cols = grafcopy.GetUpperBound(1) + 1;
            T path = default(T);
            T maxValue = GetMaxValue();

            for (int i = 0; i < rows; i++)
            {
                T temp = maxValue;
                int node = 0;
                for (int j = 0; j < cols; j++)
                {
                    if (!graf[i, j].Equals(default(T)) && temp.CompareTo(graf[i, j]) > 0)
                    {
                        temp = graf[i, j];
                        node = i;
                    }
                }
                DeleteColomn(grafcopy, node);
                if (temp.Equals(maxValue))
                {
                    temp = default(T);
                }
                path = Add(path, temp);
            }
            return path;
        }

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

        public static T[,] DeleteRow(T[,] graf, int index)
        {
            int rows = graf.GetUpperBound(0) + 1;
            int colomns = graf.GetUpperBound(1) + 1;
            T[,] newgraf = new T[rows - 1, colomns];
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

        public static T[,] ReadGraph(string pathFile)
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
            T[,] adjacencyMatrix = new T[maxIndex, maxIndex];

            foreach (var line in lines)
            {
                string[] parts = line.Split(' ');
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                T weight = (T)Convert.ChangeType(parts[2], typeof(T));

                adjacencyMatrix[from, to] = weight;
            }
            return adjacencyMatrix;
        }

        private static T GetMaxValue()
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)int.MaxValue;
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(object)double.MaxValue;
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)float.MaxValue;
            }
            else
            {
                throw new NotSupportedException("Тип данных не поддерживается");
            }
        }

        private static T Add(T a, T b)
        {
            dynamic da = a;
            dynamic db = b;
            return da + db;
        }
    }
}