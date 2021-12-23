using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public static long bestSol = long.MaxValue;

        public class Pos
        {
            public int x, y;

            public char c;

            public Pos(int x, int y, char c)
            {
                this.x = x;
                this.y = y;

                this.c = c;
            }

            public override bool Equals(object obj)
            {
                var p = (Pos)obj;
                return p.x == this.x && p.y == this.y && p.c == this.c;
            }

            public override string ToString()
            {
                return $"({this.x},{this.y}; {this.c})";
            }

            public override int GetHashCode()
            {
                return getCost(this.c) + this.x * 10 + this.y;
            }
        }

        public class State : IComparable<State>
        {
            public List<Pos> poses; //moveable poses
            public int weight;

            public State previous;

            public State(List<Pos> poses, int w)
            {
                this.poses = new List<Pos>(poses);
                this.weight = w;
                this.previous = null;

                if (this.poses.Count != 8) throw new ArgumentException("invalid posess");
            }

            public List<State> getNeighbors()
            {

                var reaches = new List<State>();

                for (int p = 0; p < poses.Count; p++)
                {
                    var moveP = this.poses[p];
                    //Console.WriteLine($"poses: {poses.Count}; idx: {p} pos of {moveP.c} is ({moveP.x},{moveP.y}) form: {((moveP.c - 'A') * 2 + 3)}");

                    var remainingPos = new List<Pos>(this.poses);
                    remainingPos.Remove(moveP);

                    if (moveP.y >= 2)
                    {
                        // skip if in goal
                        if (moveP.x == ((moveP.c - 'A') * 2 + 3) && moveP.y == 3)
                        {
                            continue; // alrdy in goal
                        }

                        if (moveP.x == ((moveP.c - 'A') * 2 + 3) && moveP.y == 2)
                        {
                            bool ingoal = false;
                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.c == moveP.c && cPos.x == ((cPos.c - 'A')) * 2 + 3 && cPos.y == 3)
                                {
                                    ingoal = true;
                                    break;
                                }
                            }

                            if (ingoal)
                            {
                                //Console.WriteLine($"{moveP.c} in goal in state ");
                                continue;
                            }
                        }


                        // move to y =1 where possible

                        for (int x = 1; x < 12; x++)
                        {
                            if (x >= 3 && x <= 9 && x % 2 == 1) continue; // no hallway


                            // check if smth is in the way
                            var blocked = false;

                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.y != 1) continue;

                                if ((x <= cPos.x && moveP.x >= cPos.x)
                                    || (x >= cPos.x && moveP.x <= cPos.x))
                                {
                                    blocked = true;
                                }
                            }

                            if (moveP.y == 3)
                            {
                                // check if y == 2 is empty
                                foreach (var cPos in remainingPos)
                                {
                                    if (cPos.y == 2 && cPos.x == moveP.x) blocked = true;
                                }

                            }

                            if (!blocked)
                            {
                                var aNextPos = new List<Pos>(remainingPos);
                                aNextPos.Add(new Pos(x, 1, moveP.c));

                                var newState = new State(aNextPos, this.weight + (Math.Abs(moveP.x - x) + (moveP.y - 1)) * getCost(moveP.c));

                                reaches.Add(newState);
                            }
                        }
                    }
                    else if (moveP.y == 1)
                    {
                        // move to corresponding hallway
                        int x = ((moveP.c - 'A') * 2 + 3);

                        bool blocked = false;

                        // check if horizantal move is even possible
                        foreach (var cPos in remainingPos)
                        {
                            if (cPos.y != 1) continue;

                            if ((x <= cPos.x && moveP.x >= cPos.x)
                                || (x >= cPos.x && moveP.x <= cPos.x))
                            {
                                blocked = true;
                            }
                        }

                        if (blocked) continue;

                        // check y == 3;
                        foreach (var cPos in remainingPos)
                        {
                            if ((cPos.y >= 2) && cPos.x == x) blocked = true;
                        }

                        if (!blocked)
                        {
                            var aNextPos = new List<Pos>(remainingPos);
                            aNextPos.Add(new Pos(x, 3, moveP.c));

                            var newState = new State(aNextPos, this.weight + (Math.Abs(moveP.x - x) + (2)) * getCost(moveP.c));

                            reaches.Add(newState);
                        }


                        blocked = false;

                        bool y3HasElem = false;
                        // check y == 2
                        foreach (var cPos in remainingPos)
                        {
                            if (cPos.y == 3 && cPos.x == x && cPos.c != moveP.c) blocked = true;
                            if (cPos.y == 2 && cPos.x == x) blocked = true;
                            if (cPos.y == 3 && cPos.x == x && cPos.c == moveP.c) y3HasElem = true;
                        }

                        if (!blocked && y3HasElem)
                        {
                            var aNextPos = new List<Pos>(remainingPos);
                            aNextPos.Add(new Pos(x, 2, moveP.c));

                            var newState = new State(aNextPos, this.weight + (Math.Abs(moveP.x - x) + (1)) * getCost(moveP.c));

                            reaches.Add(newState);
                        }
                    }
                }

                return reaches;
            }

            public bool isEndState()
            {
                foreach (var p in this.poses)
                {
                    if ((p.y == 1) || (p.x != (((p.c - 'A')) * 2 + 3))) return false;
                }

                return true;
            }

            public int CompareTo(State other) { return this.weight - other.weight; }

            public override string ToString()
            {
                string ret = "";
                foreach (var p in this.poses)
                {
                    ret += $"({p.x},{p.y}; {p.c})\n";
                }
                ret += "\n";
                ret += $"w: {this.weight}\n";

                return ret;
            }

            public override bool Equals(object obj)
            {
                State s = (State)obj;

                foreach (var p in this.poses)
                {
                    bool found = false;

                    foreach (var ps in s.poses)
                    {
                        if (p.Equals(ps))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                int hash = 0;
                foreach (var p in this.poses)
                {
                    hash += p.GetHashCode();
                }

                return hash; ;
            }
        }

        public class State2 : IComparable<State2>
        {
            public List<Pos> poses; //moveable poses
            public int weight;

            public State2 previous;

            public State2(List<Pos> poses, int w)
            {
                this.poses = new List<Pos>(poses);
                this.weight = w;
                this.previous = null;

                if (this.poses.Count != 16) throw new ArgumentException("invalid posess");
            }

            public List<State2> getNeighbors()
            {

                var reaches = new List<State2>();

                for (int p = 0; p < poses.Count; p++)
                {
                    var moveP = this.poses[p];
                    //Console.WriteLine($"poses: {poses.Count}; idx: {p} pos of {moveP.c} is ({moveP.x},{moveP.y}) form: {((moveP.c - 'A') * 2 + 3)}");

                    var remainingPos = new List<Pos>(this.poses);
                    remainingPos.Remove(moveP);

                    if (moveP.y >= 2)
                    {
                        if (moveP.x == ((moveP.c - 'A') * 2 + 3))
                        {
                            int inGoal = 0;
                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.c == moveP.c && cPos.x == ((cPos.c - 'A')) * 2 + 3 && cPos.y > moveP.y)
                                {
                                    inGoal++;
                                    break;
                                }
                            }

                            if (inGoal >= 5 - moveP.y)
                            {
                                //Console.WriteLine($"{moveP.c} in goal in state ");
                                continue;
                            }
                        }


                        // move to y =1 where possible

                        for (int x = 1; x < 12; x++)
                        {
                            if (x >= 3 && x <= 9 && x % 2 == 1) continue; // no hallway


                            // check if smth is in the way
                            var blocked = false;

                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.y != 1) continue;

                                if ((x <= cPos.x && moveP.x >= cPos.x)
                                    || (x >= cPos.x && moveP.x <= cPos.x))
                                {
                                    blocked = true;
                                }
                            }


                            // check if < y is empty
                            blocked = false;

                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.y < moveP.y && cPos.x == moveP.x)
                                {
                                    blocked = true;
                                    break;
                                }
                            }


                            if (blocked)
                            {
                                var aNextPos = new List<Pos>(remainingPos);
                                aNextPos.Add(new Pos(x, 1, moveP.c));

                                var newState = new State2(aNextPos, this.weight + (Math.Abs(moveP.x - x) + (moveP.y - 1)) * getCost(moveP.c));

                                reaches.Add(newState);
                            }
                        }
                    }
                    else if (moveP.y == 1)
                    {
                        // move to corresponding hallway
                        int x = ((moveP.c - 'A') * 2 + 3);

                        bool blocked = false;

                        // check if horizantal move is even possible
                        foreach (var cPos in remainingPos)
                        {
                            if (cPos.y != 1) continue;

                            if ((x <= cPos.x && moveP.x >= cPos.x)
                                || (x >= cPos.x && moveP.x <= cPos.x))
                            {
                                blocked = true;
                            }
                        }

                        if (blocked) continue;

                        // check general hallway
                        blocked = false;

                        foreach (var cPos in remainingPos)
                        {
                            if (cPos.x == x && cPos.c != moveP.c) blocked = true;
                        }

                        if (blocked) continue;

                        // get available y

                        int freey = 5;

                        for (int f = freey; f >= 1; f--)
                        {
                            bool hasElemAtF = false;
                            foreach (var cPos in remainingPos)
                            {
                                if (cPos.x == x && cPos.y == f)
                                {
                                    hasElemAtF = true;
                                };
                            }

                            if (!hasElemAtF)
                            {
                                freey = f;
                                break;
                            }
                        }



                        if (freey < 1)
                        {
                            var aNextPos = new List<Pos>(remainingPos);
                            aNextPos.Add(new Pos(x, 2, moveP.c));

                            var newState = new State2(aNextPos, this.weight + (Math.Abs(moveP.x - x) + (freey)) * getCost(moveP.c));

                            reaches.Add(newState);
                        }
                    }
                }

                return reaches;
            }

            public bool isEndState()
            {
                foreach (var p in this.poses)
                {
                    if ((p.y == 1) || (p.x != (((p.c - 'A')) * 2 + 3))) return false;
                }

                return true;
            }

            public int CompareTo(State2 other) { return this.weight - other.weight; }

            public override string ToString()
            {
                string ret = "";
                foreach (var p in this.poses)
                {
                    ret += $"({p.x},{p.y}; {p.c})\n";
                }
                ret += "\n";
                ret += $"w: {this.weight}\n";

                return ret;
            }

            public override bool Equals(object obj)
            {
                State2 s = (State2)obj;

                foreach (var p in this.poses)
                {
                    bool found = false;

                    foreach (var ps in s.poses)
                    {
                        if (p.Equals(ps))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                int hash = 0;
                foreach (var p in this.poses)
                {
                    hash += p.GetHashCode();
                }

                return hash; ;
            }
        }

        public static int getCost(char c)
        {
            if (c == 'A')
            {
                return 1;
            }
            else if (c == 'B')
            {
                return 10;
            }
            else if (c == 'C')
            {
                return 100;
            }
            else if (c == 'D')
            {
                return 1000;
            }

            throw new ArgumentException("invalid char");
        }


        public static State dijkstra(List<Pos> startState)
        {
            var Q = new List<State>();
            Q.Add(new State(startState, 0));

            var visited = new HashSet<State>();

            while (Q.Count > 0)
            {
                // get min element
                var minElem = Q.Min();
                Q.Remove(minElem);

                if (minElem.isEndState())
                {
                    return minElem;
                }

                //Console.WriteLine($"minElem weight: {minElem.weight}");

                var neighs = minElem.getNeighbors();

                foreach (var n in neighs)
                {
                    if (visited.Add(n))
                    {
                        n.previous = minElem;
                        Q.Add(n);
                    }
                    else
                    {
                        State nn;
                        visited.TryGetValue(n, out nn);

                        if (nn.weight > n.weight)
                        {
                            nn.weight = n.weight;
                            nn.previous = minElem;
                        }
                    }
                }
            }

            return null;
        }
        public static State2 dijkstra2(List<Pos> startState)
        {
            var Q = new List<State2>();
            Q.Add(new State2(startState, 0));

            var visited = new HashSet<State2>();

            while (Q.Count > 0)
            {
                // get min element
                var minElem = Q.Min();
                Q.Remove(minElem);

                if (minElem.isEndState())
                {
                    return minElem;
                }

                //Console.WriteLine($"minElem weight: {minElem.weight}");

                var neighs = minElem.getNeighbors();

                foreach (var n in neighs)
                {
                    if (visited.Add(n))
                    {
                        n.previous = minElem;
                        Q.Add(n);
                    }
                    else
                    {
                        State2 nn;
                        visited.TryGetValue(n, out nn);

                        if (nn.weight > n.weight)
                        {
                            nn.weight = n.weight;
                            nn.previous = minElem;
                        }
                    }
                }
            }

            return null;
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

            var field = new List<List<char>>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }


                field.Add(line.ToList());
            }

            var poses = new List<Pos>();
            poses.Add(new Pos(3, 2, field[2][3]));
            poses.Add(new Pos(3, 3, field[3][3]));
            poses.Add(new Pos(5, 2, field[2][5]));
            poses.Add(new Pos(5, 3, field[3][5]));
            poses.Add(new Pos(7, 2, field[2][7]));
            poses.Add(new Pos(7, 3, field[3][7]));
            poses.Add(new Pos(9, 2, field[2][9]));
            poses.Add(new Pos(9, 3, field[3][9]));

            var poses2 = new List<Pos>();
            poses2.Add(new Pos(3, 2, field[2][3]));
            poses2.Add(new Pos(3, 5, field[3][3]));
            poses2.Add(new Pos(5, 2, field[2][5]));
            poses2.Add(new Pos(5, 5, field[3][5]));
            poses2.Add(new Pos(7, 2, field[2][7]));
            poses2.Add(new Pos(7, 5, field[3][7]));
            poses2.Add(new Pos(9, 2, field[2][9]));
            poses2.Add(new Pos(9, 5, field[3][9]));

            /* add
            #D#C#B#A#
            #D#B#A#C#*/
            poses2.Add(new Pos(3, 3, 'D'));
            poses2.Add(new Pos(3, 4, 'D'));

            poses2.Add(new Pos(5, 3, 'C'));
            poses2.Add(new Pos(5, 4, 'B'));

            poses2.Add(new Pos(7, 3, 'B'));
            poses2.Add(new Pos(7, 4, 'A'));

            poses2.Add(new Pos(8, 3, 'A'));
            poses2.Add(new Pos(8, 4, 'C'));

            field[2][3] = '.';
            field[3][3] = '.';
            field[2][5] = '.';
            field[3][5] = '.';
            field[2][7] = '.';
            field[3][7] = '.';
            field[2][9] = '.';
            field[3][9] = '.';

            var endNode = dijkstra(poses);

            /* var cur = endNode;
            while (cur != null)
            {
                foreach (var p in cur.poses)
                {
                    field[p.y][p.x] = p.c;
                }

                foreach (var l in field)
                {
                    l.ForEach(c => Console.Write(c));
                    Console.WriteLine();
                }

                foreach (var p in cur.poses)
                {
                    field[p.y][p.x] = '.';
                }

                Console.WriteLine();

                cur = cur.previous;
            } */

            Console.WriteLine($"Task1: {endNode.weight}");


            var endNode2 = dijkstra2(poses2);

            Console.WriteLine($"Task2: {endNode2.weight}");
        }
    }
}

