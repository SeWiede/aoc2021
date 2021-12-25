using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

            var lines = new List<char[]>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                lines.Add(line.ToCharArray());
            }

            var moving = true;
            int steps = 0;

            while (moving)
            {
                moving = false;

                var oldfield = new List<List<char>>();
                lines.ForEach(l => oldfield.Add(new List<char>(l.ToList())));

                //move east
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[0].Count(); x++)
                    {
                        if (oldfield[y][x] == '>')
                        {
                            if (oldfield[y][(x + 1) % (lines[0].Count())] == '.')
                            {
                                lines[y][(x + 1) % (lines[0].Count())] = '>';
                                lines[y][x] = '.';
                                moving = true;
                            }
                        }
                    }
                }

                oldfield.Clear();
                lines.ForEach(l => oldfield.Add(new List<char>(l.ToList())));

                // move south
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[0].Count(); x++)
                    {
                        if (oldfield[y][x] == 'v')
                        {
                            if (oldfield[(y + 1) % (lines.Count)][x] == '.')
                            {
                                lines[(y + 1) % (lines.Count)][x] = 'v';
                                lines[y][x] = '.';
                                moving = true;
                            }
                        }
                    }
                }

                steps++;
            }

            Console.WriteLine($"Task: {steps}");
        }
    }
}

