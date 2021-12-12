using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public class Cave
        {
            public string rep;

            public int visited;

            public HashSet<Cave> neighbors;
            public Cave(string rep)
            {
                this.rep = rep;
                this.visited = 0;
                neighbors = new HashSet<Cave>();
            }

            public static implicit operator Cave(string rep)
            {
                return new Cave(rep);
            }

            public static bool operator >=(Cave x, char c)
            {
                return x[0] >= c;
            }
            public static bool operator <=(Cave x, char c)
            {
                return x[0] <= c;
            }
            public static bool operator ==(Cave x, string rep)
            {
                return x.rep == rep;
            }
            public static bool operator !=(Cave x, string rep)
            {
                return x.rep != rep;
            }
            public char this[int x]
            {
                get => this.rep[x];
            }

            public override bool Equals(object obj)
            {
                if (obj is string)
                {
                    return this.rep == (string)obj;
                }

                return base.Equals(obj);
            }
        }


        public static int getCaveNumInPath(List<string> oida, string c)
        {
            int count = 0;

            foreach (var o in oida)
            {
                if (o == c) count++;
            }

            return count;
        }

        public static bool hasSmallTwice = false;
        public static int findPaths(Dictionary<string, Cave> caves, string currentCave, List<string> currentPath, int i)
        {
            if (currentPath.Contains(currentCave) && currentCave[0] >= 'a')
            {
                if (hasSmallTwice)
                    return 0;
            };

            if (currentCave == "end")
            {
                return 1;
            }

            var acv = caves[currentCave];
            acv.visited++;

            currentPath.Add(currentCave);

            if (currentCave[0] >= 'a' && getCaveNumInPath(currentPath, currentCave) == 2) hasSmallTwice = true;

            int paths = 0;


            foreach (var cav in acv.neighbors)
            {
                paths += findPaths(caves, cav.rep, currentPath, i);
            }

            if (currentCave[0] >= 'a' && getCaveNumInPath(currentPath, currentCave) == 2) hasSmallTwice = false;

            currentPath.Reverse();
            currentPath.Remove(currentCave);
            currentPath.Reverse();

            return paths;
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

            var caves = new Dictionary<string, Cave>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                var pathNodes = line.Split('-');

                caves.TryAdd(pathNodes[0], pathNodes[0]);
                caves.TryAdd(pathNodes[1], pathNodes[1]);

                if (pathNodes[1] != "start")
                    caves[pathNodes[0]].neighbors.Add(caves[pathNodes[1]]);
                if (pathNodes[0] != "start")
                    caves[pathNodes[1]].neighbors.Add(caves[pathNodes[0]]);
            }

            //Console.WriteLine($"oida pls: {caves["start"].rep}");

            int paths = findPaths(caves, "start", new List<string>(), 0);

            Console.WriteLine($"Task2: {paths}");
        }
    }
}

