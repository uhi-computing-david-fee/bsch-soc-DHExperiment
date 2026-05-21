using System;
using System.Collections.Generic;
using System.IO;

namespace bsch_soc_DHExperiment
{
    /// <summary>
    /// Entry point for the Diffie-Hellman experiment application.
    /// Menu-driven interface; follow the prompts to run experiments.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nDiffie-Hellman Experiment");
            Console.WriteLine("=========================");

            bool running = true;
            while (running)
            {
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("  1. Run experiments");
                Console.WriteLine("  2. Exit");
                Console.Write("\nEnter choice: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleRun();
                        break;
                    case "2":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                        break;
                }
            }
        }

        static void HandleRun()
        {
            Console.WriteLine("\n-- Run Experiments --");
            Console.WriteLine("Suggested bit lengths:");
            Console.WriteLine("  Small (attack feasible):    8, 12, 16, 20, 24");
            Console.WriteLine("  Medium (transition zone):   28, 32, 40");
            Console.WriteLine("  Large (attack infeasible):  48, 64, 128");
            Console.WriteLine("  Note: do not exceed 128 bits. Prime generation above");
            Console.WriteLine("  this size becomes impractically slow.");
            Console.WriteLine("\nEnter bit lengths as comma-separated values (e.g. 8,12,16,24,32):");

            var bitLengths = PromptBitLengths();
            int runs = PromptInt("Number of runs per bit length: ", min: 1);
            string resultsFile = PromptPath("Results file path (e.g. results/results.csv): ");

            Directory.CreateDirectory(Path.GetDirectoryName(resultsFile) ?? ".");

            Console.WriteLine($"\nRunning experiments...");
            Console.WriteLine($"Bit lengths: {string.Join(", ", bitLengths)}");
            Console.WriteLine($"Runs per bit length: {runs}");

            var runner = new ExperimentRunner(runs);
            var reporter = new ResultReporter(resultsFile);

            foreach (int bitLength in bitLengths)
            {
                var results = runner.RunExperiment(bitLength);
                reporter.Write(results);
            }

            Console.WriteLine($"\nResults written to: {resultsFile}");
        }

        static List<int> PromptBitLengths()
        {
            while (true)
            {
                Console.Write("Bit lengths: ");
                string input = Console.ReadLine()?.Trim();
                var bitLengths = new List<int>();
                bool valid = true;

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter at least one bit length.");
                    continue;
                }

                foreach (string s in input.Split(','))
                {
                    if (int.TryParse(s.Trim(), out int bl) && bl >= 8)
                        bitLengths.Add(bl);
                    else
                    {
                        Console.WriteLine($"Invalid bit length: {s.Trim()}. All values must be integers of at least 8");
                        valid = false;
                        break;
                    }
                }

                if (valid && bitLengths.Count > 0)
                    return bitLengths;
            }
        }

        static int PromptInt(string message, int min = int.MinValue)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int value) && value >= min)
                    return value;
                Console.WriteLine($"Please enter a valid integer{(min != int.MinValue ? $" (minimum {min})" : "")}.");
            }
        }

        static string PromptPath(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine("Please enter a valid file path.");
            }
        }
    }
}