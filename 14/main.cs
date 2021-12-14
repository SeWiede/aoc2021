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

            string polyTemplate = inputStream.ReadLine();

            //skip line
            inputStream.ReadLine();

            var mapper = new Dictionary<string, string>();
            var pairCount = new Dictionary<string, long>();
            var elementCount = new Dictionary<char, long>();

            // count initial elemtns and get initial pairs
            for (int x = 0; x < polyTemplate.Length; x++)
            {
                long elemCount;
                elementCount.TryGetValue(polyTemplate[x], out elemCount);
                elementCount[polyTemplate[x]] = elemCount + 1;

                if (x == polyTemplate.Length - 1) continue;

                string pair = polyTemplate.Substring(x, 2);

                long val;
                pairCount.TryGetValue(pair, out val);
                pairCount[pair] = val + 1;
            }


            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }
                line = line.Replace(" ", "");
                var map = line.Split("->");

                mapper.Add(map[0], map[1]);

                long val;
                if (!pairCount.TryGetValue(map[0], out val))
                {
                    pairCount.Add(map[0], 0);
                }
            }

            int steps = 40;

            for (int i = 1; i < steps + 1; i++)
            {
                // save old values
                var prevCountings = pairCount.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

                foreach (var keyval in prevCountings)
                {
                    if (keyval.Value == 0) continue;

                    string map;
                    if (mapper.TryGetValue(keyval.Key, out map))
                    {
                        string newPair1 = keyval.Key[0] + map;
                        string newPair2 = map + keyval.Key[1];

                        long oldPairCnt = keyval.Value;
                        pairCount[keyval.Key] -= keyval.Value;

                        long cntPair1;
                        pairCount.TryGetValue(newPair1, out cntPair1);
                        pairCount[newPair1] = cntPair1 + oldPairCnt;

                        long cntPair2;
                        pairCount.TryGetValue(newPair2, out cntPair2);
                        pairCount[newPair2] = cntPair2 + oldPairCnt;

                        long cnt;
                        elementCount.TryGetValue(map[0], out cnt);
                        elementCount[map[0]] = cnt + oldPairCnt;

                    }
                }

                //Console.ReadLine();
            }


            long least = long.MaxValue;
            long most = 0;

            Console.WriteLine($"After {steps} steps: ");

            foreach (var keyval in elementCount)
            {
                var val = keyval.Value;

                Console.WriteLine($"Key {keyval.Key} cnt: {val}");

                if (val > most)
                {
                    most = val;
                }

                if (val < least)
                {
                    least = val;
                }
            }


            Console.WriteLine($"Task value: {most - least}");
        }
    }
}

