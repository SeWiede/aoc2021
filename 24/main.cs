using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public static void printInstr(List<string[]> instructions, Stack<int> input)
        {
            foreach (var op in instructions)
            {
                string p = "";
                switch (op[0])
                {
                    case "inp":
                        p += $"\n{op[1]} = {input.Pop()}";
                        break;
                    case "add":
                        p += $"{op[1]} = {op[1]} + {op[2]}";
                        break;
                    case "mul":
                        p += $"{op[1]} = {op[1]} * {op[2]}";
                        break;
                    case "div":
                        p += $"{op[1]} = {op[1]} / {op[2]}";
                        break;
                    case "mod":
                        p += $"{op[1]} = {op[1]} % {op[2]}";
                        break;
                    case "eql":
                        p += $"{op[1]} = {op[1]} == {op[2]}";
                        break;
                }
                Console.WriteLine(p);
            }
        }

        public static void execute(Dictionary<string, long> vars, List<string[]> instructions, int d)
        {
            foreach (var op in instructions)
            {
                long o;

                switch (op[0])
                {
                    case "inp":
                        vars[op[1]] = d;
                        break;
                    case "add":
                        if (!long.TryParse(op[2], out o))
                        {
                            o = vars[op[2]];
                        }

                        vars[op[1]] += o;
                        break;
                    case "mul":
                        if (!long.TryParse(op[2], out o))
                        {
                            o = vars[op[2]];
                        }

                        vars[op[1]] *= o;
                        break;
                    case "div":
                        if (!long.TryParse(op[2], out o))
                        {
                            o = vars[op[2]];
                        }

                        vars[op[1]] /= o;
                        break;
                    case "mod":
                        if (!long.TryParse(op[2], out o))
                        {
                            o = vars[op[2]];
                        }

                        vars[op[1]] %= o;
                        break;
                    case "eql":
                        if (!long.TryParse(op[2], out o))
                        {
                            o = vars[op[2]];
                        }

                        vars[op[1]] = ((vars[op[1]] == o) ? 1 : 0);
                        break;
                }
            }
        }


        public static long iterate(Dictionary<long, long> bestZ, List<List<string[]>> instructionSets, int i, bool max)
        {
            if (i > 13)
            {
                return bestZ.ElementAt(bestZ.Count - 1).Key;
            };

            var newBestZ = new Dictionary<long, long>();

            foreach (var kv in bestZ)
            {
                for (int d = 1; d < 10; d++)
                {

                    var instructions = instructionSets[i];

                    var z = kv.Value;

                    var vars = new Dictionary<string, long>();

                    vars.Add("w", 0);
                    vars.Add("x", 0);
                    vars.Add("y", 0);

                    vars.Add("z", z);

                    execute(vars, instructions, d);

                    if (vars["z"] >= Math.Pow(26, 5))
                    {
                        continue;
                    }
                    if (i == 13)
                    {
                        if (vars["z"] != 0)
                        {
                            continue;
                        }
                    }
                    if (i == 12)
                    {
                        if (vars["z"] > 26 * 26) continue;
                    }
                    if (i == 11)
                    {
                        if (vars["z"] > 26 * 26 * 26) continue;
                    }

                    newBestZ.Add(kv.Key * 10 + d, vars["z"]);

                }
            }

            var test = newBestZ
                .GroupBy(kv => kv.Value)
                .Where(z => z.Count() > 1);


            foreach (var t in test)
            {
                var maxPair = t.ElementAt(0);

                foreach (var t2 in t)
                {
                    if (max)
                    {

                        if (t2.Key > maxPair.Key)
                        {
                            newBestZ.Remove(maxPair.Key);
                            maxPair = t2;
                        }
                        else if (t2.Key < maxPair.Key)
                        {
                            newBestZ.Remove(t2.Key);
                        }
                    }
                    else
                    {

                        if (t2.Key < maxPair.Key)
                        {
                            newBestZ.Remove(maxPair.Key);
                            maxPair = t2;
                        }
                        else if (t2.Key > maxPair.Key)
                        {
                            newBestZ.Remove(t2.Key);
                        }
                    }
                }
            }

            return iterate(newBestZ, instructionSets, i + 1, max);
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

            var instructionSets = new List<List<string[]>>();

            var intrs = new List<string[]>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                var op = line.Split(' ');

                if (op[0] == "inp")
                {
                    instructionSets.Add(intrs);
                    intrs = new List<string[]>();
                }

                intrs.Add(op);
            }
            instructionSets.RemoveAt(0);
            instructionSets.Add(intrs);


            var bestZ = new Dictionary<long, long>();
            bestZ.Add(0, 0);


            Console.WriteLine($"Task1: {iterate(bestZ, instructionSets, 0, true)}");
            Console.WriteLine($"Task2: {iterate(bestZ, instructionSets, 0, false)}");
        }
    }
}

