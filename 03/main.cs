using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        private static int getRating(List<string> lines, int pos, int bits, bool getOxygenRating)
        {
            int[] zerosCounter = getMostCommonBitPerPos(lines, bits);
            int mostCommonBit = (zerosCounter[pos] <= (lines.Count >> 1)) ? 1 : 0;

            if (zerosCounter[pos] == (lines.Count >> 1))
            {
                mostCommonBit = 1;
            }

            if (!getOxygenRating)
            {
                mostCommonBit ^= 1;
            }

            List<string> newLines = new List<string>();
            foreach (string line in lines)
            {
                char[] charsLine = line.ToCharArray();

                if ((charsLine[pos] - '0') == mostCommonBit)
                {
                    newLines.Add(line);
                }
            }

            if (newLines.Count == 1)
            {
                return Convert.ToInt32(newLines[0], 2);
            }

            return getRating(newLines, ++pos, bits, getOxygenRating);
        }

        private static int[] getMostCommonBitPerPos(List<string> lines, int bits)
        {

            int[] zerosCounter = Enumerable.Repeat(0, bits).ToArray();

            foreach (string line in lines)
            {
                char[] charsLine = line.ToCharArray();
                for (int i = 0; i < zerosCounter.Length; i++)
                {
                    if (charsLine[i] == '0')
                    {
                        zerosCounter[i]++;
                    }
                }

            }

            return zerosCounter;
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

            List<string> lines = new List<string>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                lines.Add(line);
            }

            int bits = lines[0].Length;
            int[] zerosCounter = getMostCommonBitPerPos(lines, bits);
            int mostCommon = 0;

            for (int i = 0; i < zerosCounter.Length; i++)
            {
                if (zerosCounter[i] <= (lines.Count >> 1))
                {
                    mostCommon |= (1 << ((bits - 1) - i));
                }
            }

            int invertMask = 0;
            for (int i = 0; i < bits; i++)
            {
                invertMask |= 1 << i;
            }

            int oxygenRating = getRating(lines, 0, bits, true);
            int co2ScrubberRating = getRating(lines, 0, bits, false);

            Console.WriteLine("Power consumption = " + mostCommon * ((~mostCommon) & invertMask));
            Console.WriteLine("Life support rating = " + oxygenRating * co2ScrubberRating);
        }
    }
}

