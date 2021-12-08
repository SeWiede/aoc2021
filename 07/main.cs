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


            List<int> xposs = new List<int>();

            int possum = 0;
            int xCount = 0;
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                string[] xpossa = line.Split(",");
                foreach (string x in xpossa)
                {
                    int xi = int.Parse(x);
                    if (xposs.Count <= xi)
                    {
                        List<int> temp = Enumerable.Repeat(0, xi - xposs.Count + 1).ToList();
                        xposs.AddRange(temp);
                    }
                    xposs[xi]++;
                    possum += xi;
                    xCount++;
                }
            }

            int avg = possum / xCount;

            int[] fuelUsages = Enumerable.Repeat(0, xposs.Count).ToArray();
            int minFuelUsage = int.MaxValue;
            int prevUsage = int.MaxValue;
            int ii = 1;
            bool done = false;
            int changedDir = 0;
            for (int i = avg; i < xposs.Count && !done; i += ii)
            {
                fuelUsages[i] = 0;
                for (int j = 0; j < xposs.Count; j++)
                {
                    int steps = Math.Abs(j - i);

                    int cost = 0;

                    for (int c = 1; c <= steps; c++)
                    {
                        cost += c;
                    }

                    fuelUsages[i] += cost * xposs[j];
                }

                if (minFuelUsage > fuelUsages[i])
                {
                    minFuelUsage = fuelUsages[i];
                }

                if (fuelUsages[i] > prevUsage)
                {
                    ii = -ii;
                    changedDir++;
                }

                if (changedDir == 2) done = true;

                prevUsage = fuelUsages[i];
            }

            Console.WriteLine("Min fuel usage: {0} ", minFuelUsage);
        }
    }
}

