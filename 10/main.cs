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

            Stack<char> bracketStack = new Stack<char>();

            Dictionary<char, int> syntaxErrorScore = new Dictionary<char, int>();
            syntaxErrorScore.Add(')', 3);
            syntaxErrorScore.Add(']', 57);
            syntaxErrorScore.Add('}', 1197);
            syntaxErrorScore.Add('>', 25137);

            Dictionary<char, int> closingScoreBracket = new Dictionary<char, int>();
            closingScoreBracket.Add('(', 1);
            closingScoreBracket.Add('[', 2);
            closingScoreBracket.Add('{', 3);
            closingScoreBracket.Add('<', 4);

            List<char> openBrackets = new char[] { '(', '[', '{', '<' }.ToList();

            long errorScore = 0;

            var closingScores = new List<long>();
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                bracketStack.Clear();
                bool error = false;

                foreach (var c in line.ToCharArray())
                {
                    if (openBrackets.Contains(c))
                    {
                        bracketStack.Push(c);
                    }
                    else if (bracketStack.Peek() == c - 2 || bracketStack.Peek() == c - 1)
                    {
                        bracketStack.Pop();
                    }
                    else
                    {
                        errorScore += syntaxErrorScore[c];
                        error = true;
                        break;
                    }
                }

                if (error) continue;

                long closingScore = 0;
                while (bracketStack.Count > 0)
                {
                    var c = bracketStack.Pop();
                    closingScore = 5 * closingScore + closingScoreBracket[c];
                }

                closingScores.Add(closingScore);
            }


            Console.WriteLine($"Task1: Score is {errorScore}");
            Console.WriteLine($"Task2: Score is {(from x in closingScores orderby x select x).ToList()[closingScores.Count / 2]}");
        }
    }
}

