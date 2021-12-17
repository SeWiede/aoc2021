using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public static string getBinStr(string cmd, int at, int length)
        {
            string relevant = cmd.Substring(at >> 2, (int)Math.Ceiling((at % 4 + length) / 4.0));

            var binStr = String.Join(String.Empty, relevant.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

            return binStr.Substring(at % 4, length);
        }

        public static int getVal(string cmd, int at, int length)
        {
            return Convert.ToInt32(getBinStr(cmd, at, length), 2);
        }

        public static int parse(string cmd, int at, int max, ref int version, out long value)
        {
            value = 0;

            // get version
            version += getVal(cmd, at, 3);
            at += 3;

            // get ID
            var ID = getVal(cmd, at, 3);
            at += 3;

            if (ID == 4)
            {
                // literal
                var lit = "";

                var sublit = "";
                do
                {
                    sublit = getBinStr(cmd, at, 5);
                    lit += sublit.Substring(1);
                    at += 5;
                } while (sublit[0] == '1');

                value = Convert.ToInt64(lit, 2);
            }
            else
            {
                //operator

                var values = new List<long>();

                // get I
                var I = getVal(cmd, at, 1);
                at++;

                // get sub package values
                if (I == 0)
                {
                    // get number of bits (15 bits) in subpackage
                    var numBits = getVal(cmd, at, 15);
                    at += 15;

                    var newAt = at;
                    long val;
                    do
                    {
                        newAt = parse(cmd, newAt, at + numBits, ref version, out val);
                        values.Add(val);
                    } while (newAt < at + numBits);

                    at = at + numBits; // or newAt?
                }
                else
                {
                    // get number (11 bits) of subpackages 
                    var numSubPkgs = getVal(cmd, at, 11);
                    at += 11;

                    long val;
                    for (int i = 0; i < numSubPkgs; i++)
                    {
                        at = parse(cmd, at, max, ref version, out val);
                        values.Add(val);
                    }
                }

                switch (ID)
                {
                    case 0:
                        // sum
                        value = values.Aggregate((s, c) => s + c);
                        break;
                    case 1:
                        // product
                        value = values.Aggregate((s, c) => s * c);
                        break;
                    case 2:
                        // min
                        value = values.Min();
                        break;
                    case 3:
                        // max
                        value = values.Max();
                        break;
                    case 5:
                        // gt
                        value = values[0] > values[1] ? 1 : 0;
                        break;
                    case 6:
                        // lt
                        value = values[0] < values[1] ? 1 : 0;
                        break;
                    case 7:
                        // eq
                        value = values[0] == values[1] ? 1 : 0;
                        break;
                }
            }

            return at;
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


            int versionSum = 0;
            long val;

            string line = inputStream.ReadLine();

            parse(line, 0, line.Length, ref versionSum, out val);

            Console.WriteLine($"Task1: {versionSum}");
            Console.WriteLine($"Task2: {val}");
        }
    }
}

