using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public class Position
        {
            public char val;
            public bool marked;

            public Position(char val)
            {
                this.val = val;
                this.marked = false;
            }

            public static bool operator >(Position a, Position b)
                => a.val > b.val;
            public static bool operator >(Position a, char b)
                => a.val > b;
            public static bool operator >(char a, Position b)
                => a > b.val;
            public static bool operator <(Position a, Position b)
                => a.val < b.val;
            public static bool operator <(char a, Position b)
                => a < b.val;
            public static bool operator <(Position a, char b)
                => a.val < b;

            public static bool operator ==(Position a, Position b)
                => a.val == b.val;
            public static bool operator !=(Position a, Position b)
                => a.val != b.val;
            public static bool operator ==(Position a, char b)
                => a.val == b;
            public static bool operator !=(Position a, char b)
                => a.val != b;

            public static Position operator +(Position a, char b)
            {
                var temp = new Position((char)(a.val + b));
                temp.marked = a.marked;
                return temp;
            }

            public static Position operator +(Position a, int b)
            {
                var temp = new Position((char)(a.val + b));
                temp.marked = a.marked;
                return temp;
            }

            public static char operator +(int a, Position b)
                    => (char)(a + b.val);
        }
        public static int flood(List<List<Position>> field, int x, int y)
        {
            int size = 1;
            var cur = field[y][x];
            if (x > 0 && !field[y][x - 1].marked && field[y][x - 1] != '9' && field[y][x - 1] > cur)
            {
                field[y][x - 1].marked = true;
                size += flood(field, x - 1, y);
            }
            if (y > 0 && !field[y - 1][x].marked && field[y - 1][x] != '9' && field[y - 1][x] > cur)
            {
                field[y - 1][x].marked = true;
                size += flood(field, x, y - 1);
            }
            if (x < field[0].Count - 1 && !field[y][x + 1].marked && field[y][x + 1] != '9' && field[y][x + 1] > cur)
            {
                field[y][x + 1].marked = true;
                size += flood(field, x + 1, y);
            }
            if (y < field.Count - 1 && !field[y + 1][x].marked && field[y + 1][x] != '9' && field[y + 1][x] > cur)
            {
                field[y + 1][x].marked = true;
                size += flood(field, x, y + 1);
            }

            return size;
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

            List<List<Position>> field = new List<List<Position>>();

            int risk = 0;
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                field.Add(line.ToList().Select((x) => new Position(x)).ToList());
            }

            List<int> basins = new List<int>();

            for (int y = 0; y < field.Count; y++)
            {
                for (int x = 0; x < field[0].Count; x++)
                {
                    var cur = field[y][x];
                    if ((y == 0 || field[y - 1][x] > cur)
                    && (y == field.Count - 1 || field[y + 1][x] > cur)
                    && (x == 0 || field[y][x - 1] > cur)
                    && (x == field[0].Count - 1 || field[y][x + 1] > cur))
                    {
                        risk += (1 + cur - '0');
                        basins.Add(flood(field, x, y));
                    }
                }
            }

            Console.WriteLine($"Task1: Risk is {risk}");
            Console.WriteLine($"Task2: Sum of three largest basisns {(from x in basins orderby x descending select x).ToList().Take(3).ToList().Aggregate((a, x) => a * x)}");
        }
    }
}

