using System;
using AntColony;
using Graph;

class Program
{
    static void Main(string[] args)
    {
 
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: program.exe <input_file.txt>");
            return;
        }

        string inputFilePath = args[0];

        if (!System.IO.File.Exists(inputFilePath))
        {
            Console.WriteLine($"File not found: {inputFilePath}");
            return;
        }

        double[,] graph = Graph.Graph.Copy(Graph.Graph.ReadGraph(inputFilePath));
 
        Graph.Graph.Print(graph);
        AntAlgorithm antAlgorithm = new AntAlgorithm(graph);

        int[] bestPath = antAlgorithm.Run();
        antAlgorithm.SavePathsToFile("path.txt");
        
        Console.WriteLine("Final Best Path: " + string.Join(" -> ", bestPath) + $" Length: {antAlgorithm.PathLenght(bestPath)}");
          
    }
}