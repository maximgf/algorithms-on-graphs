using System;
using AntColony;
public class Program
{
 
    public static void Main()
    {
        double[,] distances = 
        {
            { 0, 10, 15, 20 },
            { 12, 0, 35, 25 },
            { 18, 30, 0, 30 },
            { 22, 28, 32, 0 }
        };

        var aco = new AntAlgorithm(distances);

        var bestPath = aco.Run();
        for (int i = 0; i < bestPath.Length; i++)
        {
            Console.WriteLine(bestPath[i]);
        }

        double bestPathLength = aco.PathLenght(bestPath);
        Console.WriteLine("Best path length: " + bestPathLength);
    }
}

 