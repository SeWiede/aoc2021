using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public static List<string> extendField(List<string> field, char def)
        {
            var newLine = String.Join("", field[0].ToList().Select(c => def));

            field = field.Prepend(newLine).ToList();

            field.Add(newLine);

            for (int i = 0; i < field.Count; i++)
            {
                field[i] = def + field[i] + def;
            }

            return field;
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


            var lines = new List<string>();

            string enhalg = inputStream.ReadLine();
            inputStream.ReadLine();

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

            // extend

            int extend = 250; // fml
            for (int i = 0; i < extend; i++)
            {
                lines = extendField(lines, '.');
            }

            int steps = 50;


            char def = enhalg[0];

            for (int s = 0; s < steps; s++)
            {
                if (s % 2 == 1 || enhalg[0] == '.')
                {
                    def = enhalg[0];
                }
                else
                {
                    def = enhalg[enhalg.Length - 1];
                }

                lines = extendField(lines, def);
                //copy
                var cField = new List<string>();
                for (int y = 0; y < lines.Count; y++)
                {
                    var line = lines[y];

                    cField.Add(new String(line));
                }

                for (int y = 1; y < cField.Count - 1; y++)
                {
                    for (int x = 1; x < cField[y].Length - 1; x++)
                    {
                        //Console.WriteLine($"checking ({x},{y})");

                        var algStr = String.Join("", String.Join("", new char[]{
                                cField[y - 1][x - 1] , cField[y - 1][x] , cField[y - 1][x + 1],
                                cField[y][x - 1] , cField[y][x] , cField[y][x + 1],
                                cField[y + 1][x - 1] , cField[y + 1][x] , cField[y + 1][x + 1]
                            }).Select((c) => { if (c == '#') return "1"; else return "0"; }).ToList());

                        //Console.WriteLine($"algStr is {algStr}");

                        var algNum = Convert.ToInt32(algStr, 2);

                        //Console.WriteLine($"algNum is {algNum}");

                        var algChar = enhalg[algNum];

                        //Console.WriteLine($"algChar is {algChar}");

                        var lineCha = lines[y].ToCharArray();

                        lineCha[x] = algChar;
                        lines[y] = new String(lineCha);
                    }
                }
                /* 
                                foreach (var line in lines)
                                {
                                    Console.WriteLine(line);
                                }

                                Console.ReadLine(); */
            }

            int lightcount = 0;
            for (int y = extend / 2; y < lines.Count - extend / 2; y++)
            {
                for (int x = extend / 2; x < lines[y].Length - extend / 2; x++)
                {
                    if (lines[y][x] == '#') lightcount++;
                }
            }

            Console.WriteLine($"Task1: {lightcount}");
        }
    }
}

