using System;
using System.IO;
using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StreamReader inputStream = new StreamReader(Console.OpenStandardInput());


            if (args.Length > 0)
            {

                string inputFileName = args[0];

                if (!File.Exists(inputFileName))
                {
                    Console.WriteLine("Input file " + inputFileName + " does not exist!");
                    return;
                }

                inputStream = new StreamReader(inputFileName);
            }

            int horizontalPos = 0;
            int depth = 0;

            int aim = 0;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                string[] splits = line.Split(' ');

                string command = splits[0];
                int steps = Int16.Parse(splits[1]);

                switch (command)
                {
                    case "forward":
                        horizontalPos += steps;
                        depth += (aim * steps);
                        break;
                    case "down":
                        aim += steps;
                        break;
                    case "up":
                        aim -= steps;
                        break;
                    default:
                        Console.WriteLine("Invalid command " + command);
                        continue;
                }
            }

            Console.WriteLine("Depth * horizontalPos: " + depth * horizontalPos);
        }
    }
}

