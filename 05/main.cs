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

            Dictionary<string, int> map = new Dictionary<string, int>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                line = line.Replace(" ", "");
                string[] ep = line.Split("->");

                string[] leftPoint = ep[0].Split(",");
                string[] rightPoint = ep[1].Split(",");

                int x1 = int.Parse(leftPoint[0]), x2 = int.Parse(rightPoint[0]), y1 = int.Parse(leftPoint[1]), y2 = int.Parse(rightPoint[1]);


                if (x1 != x2 && y1 != y2)
                {
                    int xi = ((x1 < x2) ? 1 : -1);
                    int yi = ((y1 < y2) ? 1 : -1);

                    for (int x = x1, y = y1; x != (x2 + xi) && y != (y2 + yi); x += xi, y += yi)
                    {
                        string id = x + "," + y;
                        map.TryGetValue(id, out var currentCount);
                        map[id] = currentCount + 1;

                    }
                }
                else
                {
                    int xs = ((x1 < x2) ? x1 : x2);
                    int xe = ((x1 < x2) ? x2 : x1);

                    int ys = ((y1 < y2) ? y1 : y2);
                    int ye = ((y1 < y2) ? y2 : y1);
                    for (int x = xs; x <= xe; x++)
                    {
                        for (int y = ys; y <= ye; y++)
                        {
                            string id = x + "," + y;
                            map.TryGetValue(id, out var currentCount);
                            map[id] = currentCount + 1;
                        }
                    }
                }


            }

            int crossings = 0;
            foreach (var item in map)
            {
                if (item.Value > 1)
                {
                    crossings++;
                }
            }

            Console.WriteLine("Overlaps: " + crossings);
        }
    }
}

