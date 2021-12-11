using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public class Octopus
        {
            public int val;
            int age;
            public List<Octopus> neighbors;

            public Octopus(char c)
            {
                this.val = c - '0';
                this.age = 0;

                this.neighbors = new List<Octopus>();
            }

            public static implicit operator Octopus(char c)
            {
                return new Octopus(c);
            }

            public int advance(int step, ref int flashesInStep)
            {
                if (this.val == 0 && step == this.age)
                    return 0;

                if (step > this.age) this.age++;

                this.val++;

                if (this.val > 9)
                {
                    this.val = 0;

                    int flashes = 1;

                    foreach (var n in this.neighbors)
                    {
                        flashes += n.advance(step, ref flashesInStep);
                    }

                    flashesInStep++;

                    return flashes;
                }

                return 0;
            }

        }

        public static void AddNeighbor(List<List<Octopus>> f, Octopus o, int x, int y)
        {
            if (x < 0 || y < 0 || x >= f[0].Count || y >= f.Count) return;
            o.neighbors.Add(f[y][x]);
        }
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

            var field = new List<List<Octopus>>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }


                field.Add(line.Select((x) => new Octopus(x)).ToList());
            }

            for (int y = 0; y < field.Count; y++)
            {
                for (int x = 0; x < field[0].Count; x++)
                {
                    AddNeighbor(field, field[y][x], x - 1, y);
                    AddNeighbor(field, field[y][x], x + 1, y);
                    AddNeighbor(field, field[y][x], x, y + 1);
                    AddNeighbor(field, field[y][x], x, y - 1);
                    AddNeighbor(field, field[y][x], x + 1, y + 1);
                    AddNeighbor(field, field[y][x], x + 1, y - 1);
                    AddNeighbor(field, field[y][x], x - 1, y + 1);
                    AddNeighbor(field, field[y][x], x - 1, y - 1);
                }
            }

            int numSteps = 100;
            int flashes = 0;

            int stepAllFlash = -1;

            for (int s = 0; ; s++)
            {
                int flashesInStep = 0;

                foreach (var l in field)
                {
                    foreach (var o in l)
                    {
                        int fs = o.advance(s, ref flashesInStep);
                        if (s < numSteps)
                            flashes += fs;
                    }
                }

                if (flashesInStep == field.Count * field[0].Count)
                {
                    stepAllFlash = s + 1;
                    break;
                }
            }

            Console.WriteLine($"Task1: {flashes}");
            Console.WriteLine($"Task2: {stepAllFlash}");
        }
    }
}

