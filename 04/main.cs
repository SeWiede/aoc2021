using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class BingoBoard
    {
        struct BingoBoardVal
        {
            public int value;
            public bool marked;
        }

        private BingoBoardVal[,] fields;

        int sumUnmarked = 0;
        int lastMarkedVal = 0;

        public bool won = false;

        public int Score
        {
            get
            {
                return sumUnmarked * lastMarkedVal;
            }
        }

        public BingoBoard(int[,] boardVals)
        {
            this.fields = new BingoBoardVal[5, 5];
            this.sumUnmarked = 0;

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    this.fields[x, y].value = boardVals[x, y];
                    this.fields[x, y].marked = false;

                    this.sumUnmarked += boardVals[x, y];
                }
            }

            this.won = false;
        }

        public void Mark(int val)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (this.fields[x, y].value == val)
                    {
                        this.fields[x, y].marked = true;
                        this.sumUnmarked -= this.fields[x, y].value;
                        this.lastMarkedVal = this.fields[x, y].value;
                        return;
                    }
                }
            }
        }

        public bool checkBingo()
        {
            int[] rowMarks = { 0, 0, 0, 0, 0 };

            for (int x = 0; x < 5; x++)
            {
                int marks = 0;
                for (int y = 0; y < 5; y++)
                {
                    if (this.fields[x, y].marked)
                    {
                        marks++;
                        rowMarks[y]++;
                    }
                }
                if (marks == 5)
                {
                    this.won = true;
                    return true;
                }
            }

            foreach (int m in rowMarks)
            {
                if (m == 5)
                {
                    this.won = true;
                    return true;
                }
            }

            return false;
        }
    }

    public class Program
    {

        public static BingoBoard makeBoard(StreamReader inputStream)
        {
            int[,] values = new int[5, 5];
            // read 5 times - 5 rows
            for (int i = 0; i < 5; i++)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());

                string[] valsLine = line.Split(' ');
                for (int x = 0, j = 0; j < valsLine.Length; j++)
                {
                    if (valsLine[j] == "")
                    {
                        continue;
                    }
                    values[i, x] = Convert.ToInt16(valsLine[j]);

                    x++;
                }
            }

            return new BingoBoard(values);
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

            string inputValsLine = inputStream.ReadLine();

            List<int> inputValues = inputValsLine.Split(',').ToList().Select(x => { return Convert.ToInt32(x); }).ToList();
            List<BingoBoard> bingoBoards = new List<BingoBoard>();

            while (inputStream.Peek() >= 0)
            {
                inputStream.ReadLine();
                bingoBoards.Add(makeBoard(inputStream));
            }

            List<int> winScores = new List<int>();

            foreach (int val in inputValues)
            {
                int i = 0;
                foreach (BingoBoard b in bingoBoards)
                {
                    i++;
                    if (b.won) continue;
                    b.Mark(val);
                    if (b.checkBingo())
                    {
                        winScores.Add(b.Score);
                    }
                }
                if (winScores.Count == bingoBoards.Count) break;
            }

            Console.WriteLine("Winner score = " + winScores[0]);
            Console.WriteLine("Last score = " + winScores[winScores.Count - 1]);
        }
    }
}

