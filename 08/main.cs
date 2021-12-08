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

            int nums = 0;

            List<char[]>[] buckets = new List<char[]>[6];
            const int bucketOffset = 2;

            // 1 - 2
            const int DIGITS_ONE = 2 - bucketOffset;
            // 7 - 3
            const int DIGITS_SEVEN = 3 - bucketOffset;
            // 4 - 4
            const int DIGITS_FOUR = 4 - bucketOffset;
            // 2, 3, 5 - 5
            const int DIGITS_235 = 5 - bucketOffset;
            // 0, 6, 9 - 6 
            const int DIGITS_069 = 6 - bucketOffset;
            // 8 - 7
            const int DIGITS_EIGHT = 7 - bucketOffset;


            int outputSum = 0;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                string[] io = line.Split("|");
                string[] inputs = io[0].Trim(' ').Split(' ');
                string[] outputs = io[1].Trim(' ').Split(' ');

                foreach (string outs in outputs)
                {
                    if (outs.Length == 2 || outs.Length == 3 || outs.Length == 4 || outs.Length == 7)
                    {
                        nums++;
                    }
                }


                for (int i = 0; i < buckets.Length; i++)
                {
                    buckets[i] = new List<char[]>();
                }

                foreach (string inp in inputs)
                {
                    buckets[inp.Length - bucketOffset].Add(inp.ToCharArray());
                }

                // 00000
                //1     3
                //1     3
                // 22222
                //4     6
                //4     6
                // 55555

                char[] wireMapper = Enumerable.Repeat('0', 7).ToArray();

                // get a 1 and 7 to get mapping of 0-wire
                wireMapper[0] = buckets[DIGITS_SEVEN][0].Except(buckets[DIGITS_ONE][0]).ElementAt(0);

                List<char> zeroSeg = new List<char>();
                List<char> nineSeg = new List<char>();

                // find 6-wire by finding a 6 (only elem in of 6-bucket without 1 elems)
                // nd thus fix 3-wire


                foreach (var x in buckets[DIGITS_069])
                {
                    var exc = buckets[DIGITS_ONE][0].Except(x);
                    if (exc.Count() != 0)
                    {
                        // x has to be a 6 exc has to be of length 1 actually and has to be 3 wire
                        wireMapper[3] = exc.ElementAt(0);
                        // thus the other one in 1 has to be the 6-wire
                        wireMapper[6] = buckets[DIGITS_ONE][0].Except(exc).ElementAt(0);
                        break;
                    }
                }

                // with known stuff check 4 and get 1,2 check 6-wires and get the 0 to get 2-map
                foreach (var x in buckets[DIGITS_069])
                {
                    var exc = buckets[DIGITS_FOUR][0].Except(x);
                    // empty => 9
                    if (exc.Count() == 0)
                    {
                        nineSeg = x.ToList();
                    }
                    else if (exc.ElementAt(0) != wireMapper[3])
                    {
                        // ? => 0
                        zeroSeg = x.ToList();
                        // also: ? = 2-wire
                        wireMapper[2] = exc.ElementAt(0);
                    }

                }

                // get 4-wire
                wireMapper[4] = zeroSeg.Except(nineSeg).ElementAt(0);

                // get 5-wire by getting 3 somehow
                foreach (var x in buckets[DIGITS_235])
                {
                    var exc = x.Except(new char[] { wireMapper[0], wireMapper[2], wireMapper[3], wireMapper[6] }.ToList());
                    if (exc.Count() == 1)
                    {
                        wireMapper[5] = exc.ElementAt(0);

                        wireMapper[1] = zeroSeg.Except(wireMapper.ToList()).ElementAt(0);
                    }
                }

                // decode output
                string outputDigits = "0";

                for (int i = 0; i < outputs.Length; i++)
                {
                    var curDigit = outputs[i];

                    switch (curDigit.Length)
                    {
                        case DIGITS_ONE + bucketOffset:
                            outputDigits += 1;
                            break;
                        case DIGITS_FOUR + bucketOffset:
                            outputDigits += 4;
                            break;
                        case DIGITS_EIGHT + bucketOffset:
                            outputDigits += 8;
                            break;
                        case DIGITS_SEVEN + bucketOffset:
                            outputDigits += 7;
                            break;
                        case DIGITS_069 + bucketOffset:
                            if (curDigit.Contains(wireMapper[2]))
                            {
                                if (curDigit.Contains(wireMapper[3]))
                                {
                                    outputDigits += 9;
                                }
                                else
                                {
                                    outputDigits += 6;
                                }
                            }
                            else
                            {
                                outputDigits += 0;
                            }
                            break;
                        case DIGITS_235 + bucketOffset:
                            if (curDigit.Contains(wireMapper[6]))
                            {
                                if (curDigit.Contains(wireMapper[3]))
                                {
                                    outputDigits += 3;
                                }
                                else
                                {
                                    outputDigits += 5;
                                }
                            }
                            else
                            {
                                outputDigits += 2;
                            }
                            break;
                        default:
                            throw new Exception("invalid digit length " + curDigit.Length + " digit: " + curDigit);
                    }
                }

                outputSum += int.Parse(outputDigits);
            }

            Console.WriteLine($"The output sum is {outputSum}");
        }
    }
}

