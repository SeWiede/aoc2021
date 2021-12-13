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

            var field = new List<List<char>>();

            int maxx = 0;
            int maxy = 0;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }

                var c = line.Split(',');
                int xc = int.Parse(c[0]);
                int yc = int.Parse(c[1]);

                if (maxx <= xc)
                {
                    maxx = xc;
                }
                if (maxy <= yc)
                {
                    maxy = yc;
                }

                while (field.Count < maxy + 1)
                {
                    field.Add(new List<char>());
                }

                for (int y = 0; y < field.Count; y++)
                {
                    while (field[y].Count < maxx + 1)
                    {
                        field[y].Add('.');
                    }
                }

                field[yc][xc] = '#';
            }

            int count = 0;

            // next lines are folds
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }

                var fold = line.Remove(0, "fold along ".Length).Split("=");


                var folding = int.Parse(fold[1]);

                for (int y = 0; y < maxy + 1; y++)
                {
                    for (int x = 0; x < maxx + 1; x++)
                    {
                        if (field[y][x] == '.')
                            continue;
                        count++;

                        if (fold[0] == "x")
                        {
                            if (x >= folding)
                            {
                                field[y][x] = '.';

                                int newx = folding - (x - folding);

                                //Console.WriteLine($"fold {folding} x: accessing ({newx},{y})");

                                if (field[y][newx] == '#') count--;

                                field[y][newx] = '#';
                            }
                        }
                        else
                        {
                            if (y >= folding)
                            {
                                field[y][x] = '.';

                                int newy = folding - (y - folding);

                                if (field[newy][x] == '#') count--;

                                field[newy][x] = '#';
                            }
                        }
                    }
                }

                if (fold[0] == "x")
                {
                    maxx -= folding;
                }
                else
                {
                    maxy -= folding;
                }
            }

            for (int y = 0; y < maxy; y++)
            {
                for (int x = 0; x < maxx; x++)
                {
                    Console.Write(field[y][x]);
                }
                Console.WriteLine();
            }

        }
    }
}

