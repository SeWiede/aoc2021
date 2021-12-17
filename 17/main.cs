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

            int x1 = 0;
            int x2 = 0;

            int y1 = 0;
            int y2 = 0;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }

                line = line.Split("target area: x=")[1];

                var xy = line.Split(", y=");

                var xb = xy[0].Split("..");
                var yb = xy[1].Split("..");

                x1 = Convert.ToInt32(xb[0]);
                x2 = Convert.ToInt32(xb[1]);

                y1 = Convert.ToInt32(yb[0]);
                y2 = Convert.ToInt32(yb[1]);
            }

            // x =  x > 0 ? x--: 0;
            // y--;

            // get all y that can hit target in y dir
            var validY = new List<int>();

            for (int y = y1; y < 1000; y++)
            {
                var ytarget = 0;
                for (int i = y; ytarget > y1; i--)
                {
                    ytarget += i;

                    if (ytarget <= y2 && ytarget >= y1)
                    {
                        validY.Add(y);
                        break;
                    }
                }
            }

            var distinctPairs = new HashSet<(int, int)>();

            for (int x = 0; x <= x2; x++)
            {
                var xtarget = 0;
                var allY = new List<(int, int, int)>();
                validY.ForEach(y => allY.Add((0, y, y)));

                for (int i = x; i > 0; i--)
                {
                    xtarget += i;

                    for (int n = 0; n < allY.Count; n++)
                    {
                        allY[n] = (allY[n].Item1 + allY[n].Item2, allY[n].Item2 - 1, allY[n].Item3);
                    }

                    if (xtarget >= x1 && xtarget <= x2)
                    {
                        var temp = allY.Where((y) => y.Item1 <= y2 && y.Item1 >= y1).ToList();

                        foreach (var t in temp)
                        {
                            distinctPairs.Add((x, t.Item3));
                        }
                    }
                }

                if (xtarget < x1 || xtarget > x2) continue;

                while (allY.Where(y => y.Item1 > y1).ToList().Count > 0)
                {
                    for (int n = 0; n < allY.Count; n++)
                    {
                        allY[n] = (allY[n].Item1 + allY[n].Item2, allY[n].Item2 - 1, allY[n].Item3);
                    }

                    var temp = allY.Where((y) => y.Item1 <= y2 && y.Item1 >= y1).ToList();

                    foreach (var t in temp)
                    {
                        distinctPairs.Add((x, t.Item3));
                    }
                }


            }

            var result = 0;
            for (int i = validY.Max(); i > 0; i--)
            {
                result += i;
            }

            Console.WriteLine($"Task1: {result}");
            Console.WriteLine($"Task1: {distinctPairs.Count}");
        }
    }
}

