using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public static long[] playerWins = new long[] { 0, 0 };
        public static List<(int, int)> possibleOutcomeNums;

        public static void quantumRoll(int[] points, int[] pos, int p, long winPotentials)
        {
            if (points[(p + 1) % 2] >= 21)
            {
                playerWins[(p + 1) % 2] += winPotentials;
                return;
            }

            for (int i = 0; i < possibleOutcomeNums.Count; i++)
            {
                var newPoints = new int[2];
                var newPos = new int[2];
                long newWinPotentials = 0;

                var test = possibleOutcomeNums[i];

                newPos[p] = pos[p] + test.Item1;
                if (newPos[p] > 10) newPos[p] -= 10;
                newPos[(p + 1) % 2] = pos[(p + 1) % 2];

                newPoints[p] = points[p] + newPos[p];
                newPoints[(p + 1) % 2] = points[(p + 1) % 2];

                newWinPotentials = winPotentials * (long)test.Item2;

                quantumRoll(newPoints, newPos, (p + 1) % 2, newWinPotentials);
            }
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


            var points = new int[2] { 0, 0 };
            var initPos = new int[2] { 0, 0 };

            var p = 0;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                var vs = line.Split(" starting position: ");
                initPos[p] = Convert.ToInt32(vs[1]);
                p++;
            }
            p = 0;

            bool done = false;
            int dieVal = 1;

            int winVal = 1000;

            var rolls = 0;

            var pos = new int[2] { initPos[0], initPos[1] };

            while (!done)
            {
                pos[p] += dieVal++ + dieVal++ + dieVal++;



                if (dieVal > 100)
                {
                    dieVal -= 100;
                    pos[p] -= (dieVal) * 100;
                }

                rolls++;


                while (pos[p] > 10) pos[p] -= 10;


                points[p] += pos[p];

                /* Console.WriteLine($"player {p + 1} pos {pos[p]} points {points[p]}");
                Console.ReadLine(); */

                done = points[p] >= winVal;

                p = (++p) % 2;
            }

            possibleOutcomeNums = new List<(int, int)>();
            possibleOutcomeNums.Add((3, 1));
            possibleOutcomeNums.Add((4, 3));
            possibleOutcomeNums.Add((5, 6));
            possibleOutcomeNums.Add((6, 7));
            possibleOutcomeNums.Add((7, 6));
            possibleOutcomeNums.Add((8, 3));
            possibleOutcomeNums.Add((9, 1));

            quantumRoll(new int[] { 0, 0 }, new int[] { initPos[0], initPos[1] }, 0, 1);

            Console.WriteLine($"Task1: {rolls * 3 * points[p]}");
            Console.WriteLine($"Task2: {playerWins[0] > playerWins[1] ? playerWins[0]: playerWins[1]}");
        }
    }
}

