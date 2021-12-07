using System;
using System.IO;
using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class TripleSummer
    {
        private List<int> mem;
        private int tripleSum;
        private int prevSum;

        private int cur;
        private int prev;

        public TripleSummer()
        {
            this.mem = new List<int>();
            this.tripleSum = this.prevSum = Int16.MaxValue;
            this.prev = this.cur = Int16.MaxValue;
        }
        public void feed(int next)
        {
            if (this.mem.Count == 3)
            {
                this.mem.RemoveAt(0);
                this.prevSum = this.tripleSum;
            }

            this.mem.Add(next);

            this.tripleSum = 0;

            this.mem.ForEach(delegate (int x)
            {
                this.tripleSum += x;
            });

            this.prev = cur;
            this.cur = next;
        }

        public bool Increased
        {
            get
            {
                return this.cur > this.prev;
            }
        }

        public bool TripleIncreased
        {
            get
            {
                if (this.mem.Count < 3)
                {
                    return false;
                }

                return this.tripleSum > this.prevSum;
            }
        }
    }

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

            int increased = 0;
            int tripleIncreased = 0;

            TripleSummer ts = new TripleSummer();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                int cur = int.Parse(line.Trim("\n ".ToCharArray()));
                ts.feed(cur);

                if (ts.Increased)
                {
                    increased++;
                }

                if (ts.TripleIncreased)
                {
                    tripleIncreased++;
                }
            }

            Console.WriteLine("Increased: " + increased);
            Console.WriteLine("Tripleincreased: " + tripleIncreased);
        }
    }
}

