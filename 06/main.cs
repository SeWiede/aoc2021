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

            long[] anglersPop = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                string[] anglersStr = line.Split(",");

                foreach (string angler in anglersStr)
                {
                    int aInt = int.Parse(angler);
                    if (aInt < 0 || aInt > 8)
                    {
                        Console.WriteLine("error in input: invalid age");
                    }

                    anglersPop[aInt]++;
                }
                break;
            }

            int days = 256;

            for (int i = 0; i < days; i++)
            {
                long helper = anglersPop[0];
                for (int j = 0; j < 8; j++)
                {
                    anglersPop[j] = anglersPop[j + 1];
                }
                anglersPop[8] = helper;
                anglersPop[6] += helper;
            }

            long sumAnglers = 0;
            foreach (long a in anglersPop)
            {
                sumAnglers += a;
            }

            Console.WriteLine("Anglers after {0} days: {1}", days, sumAnglers);
        }
    }
}

