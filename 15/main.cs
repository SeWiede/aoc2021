using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public class Node : IComparable<Node>
        {
            public int x;
            public int y;
            public int weight;
            public bool inPath;
            public bool visited;
            public int value;

            public Node predecessor;

            public Node(int value, int x, int y)
            {
                this.value = value;
                this.weight = int.MaxValue;
                this.x = x;
                this.y = y;
                this.predecessor = null;
            }

            public int CompareTo(Node other) { return weight - other.weight; }
        }

        // dijkstra
        public static void getPath(List<List<Node>> field)
        {

        }

        public static void setNeighborWeight(List<List<Node>> field, int x, int y, Node predecessor, List<Node> Q)
        {
            if (x < 0 || y < 0 || x >= field[0].Count || y >= field.Count || field[y][x].weight < predecessor.weight + field[y][x].value) return;

            field[y][x].weight = predecessor.weight + field[y][x].value;
            field[y][x].predecessor = predecessor;

            if (!field[y][x].visited)
            {
                field[y][x].visited = true;
                Q.Add(field[y][x]);
            }
        }

        public static void dijkstra(List<List<Node>> field)
        {
            var Q = new List<Node>();
            Q.Add(field[0][0]);

            Q[0].weight = 0;

            while (Q.Count > 0)
            {
                // get min element
                Node minElem = Q.Min();

                //Console.WriteLine($"current ({minElem.x},{minElem.y}) has weight {minElem.weight}");

                Q.Remove(minElem);

                if (minElem.x == field[0].Count - 1 && minElem.y == field.Count - 1) break;

                // put neighbors in Q
                setNeighborWeight(field, minElem.x - 1, minElem.y, minElem, Q);
                setNeighborWeight(field, minElem.x + 1, minElem.y, minElem, Q);
                setNeighborWeight(field, minElem.x, minElem.y - 1, minElem, Q);
                setNeighborWeight(field, minElem.x, minElem.y + 1, minElem, Q);
            }
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

            var field = new List<List<Node>>();

            int origLength = 0;
            int lineNum = 0;
            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }

                origLength = line.Length; // undefined behaviour if this changes

                var nLine = line.Select((c, x) => new Node((int)c - '0', x, lineNum)).ToList();
                field.Add(nLine);

                lineNum++;
            }

            dijkstra(field);

            Node endNode1 = field[field.Count - 1][field[0].Count - 1];

            Console.WriteLine($"Task1: {endNode1.weight}");

            // extend field in x
            foreach (var l in field)
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int x = 0; x < origLength; x++)
                    {
                        l.Add(new Node(0, l[x].x + i * origLength, l[0].y));
                    }
                }
            }
            for (int i = origLength; i < origLength * 5; i++)
            {
                field.Add(field[field.Count - 1].Select((n) => new Node(0, n.x, field.Count)).ToList());
            }

            // fill extended field
            for (int y = 0; y < field.Count; y++)
            {
                for (int x = 0; x < field[0].Count; x++)
                {
                    var n = field[y][x];
                    n.weight = int.MaxValue;
                    n.visited = false;

                    var newVal = field[y % origLength][x % origLength].value;
                    newVal += x / origLength + y / origLength;

                    if (newVal > 9)
                    {
                        newVal = newVal % 9;
                    }

                    n.value = newVal;
                }
            }

            dijkstra(field);

            endNode1 = field[field.Count - 1][field[0].Count - 1];

            Console.WriteLine($"Task2: {endNode1.weight}");
        }
    }
}

