using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public class RotCoord : IEquatable<RotCoord>
        {
            public int x;
            public int y;
            public int z;

            public RotCoord(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public static implicit operator RotCoord((int, int, int) coords)
            {
                return new RotCoord(coords.Item1, coords.Item2, coords.Item3);
            }

            public override string ToString()
            {
                return "[" + this.x + "," + this.y + "," + this.z + "]";
            }

            public static RotCoord operator +(RotCoord l, RotCoord r)
            {
                return new RotCoord(l.x + r.x, l.y + r.y, l.z + r.z);
            }
            public static RotCoord operator -(RotCoord l, RotCoord r)
            {
                return new RotCoord(l.x - r.x, l.y - r.y, l.z - r.z);
            }

            public static bool operator ==(RotCoord l, RotCoord r)
            {
                return (l.x == r.x) && (l.y == r.y) && (l.z == r.z);
            }

            public static bool operator !=(RotCoord l, RotCoord r)
            {
                return !((l.x == r.x) && (l.y == r.y) && (l.z == r.z));
            }

            public override int GetHashCode()
            {
                return 0;
            }
            public bool Equals(RotCoord other)
            {
                //Console.WriteLine($"equalcheck: {this} ?== {other}");
                return (this.x == other.x) && (this.y == other.y) && (this.z == other.z);
            }
        }

        public static List<RotCoord> rotate90(List<RotCoord> r, int kind)
        {
            var ret = new List<RotCoord>();

            switch (kind)
            {
                /*
                x,y,z
                x,-y,-z
                x,y, -z
                x,-y,z
                
                -x,y,z
                -x,-y,z,
                -x,y,-z
                -x,-y,-z
                
                */
                case 0:
                    r.ForEach((e) => ret.Add((e.x, e.y, e.z)));
                    break;
                case 1:
                    r.ForEach((e) => ret.Add((-e.y, e.x, e.z)));
                    break;
                case 2:
                    r.ForEach((e) => ret.Add((-e.x, -e.y, e.z)));
                    break;
                case 3:
                    r.ForEach((e) => ret.Add((e.y, -e.x, e.z)));
                    break;
                case 4:
                    r.ForEach((e) => ret.Add((e.z, e.y, -e.x)));
                    break;
                case 5:
                    r.ForEach((e) => ret.Add((e.z, e.x, e.y)));
                    break;
                case 6:
                    r.ForEach((e) => ret.Add((e.z, -e.y, e.x)));
                    break;
                case 7:
                    r.ForEach((e) => ret.Add((e.z, -e.x, -e.y)));
                    break;
                case 8:
                    r.ForEach((e) => ret.Add((-e.x, e.y, -e.z)));
                    break;
                case 9:
                    r.ForEach((e) => ret.Add((e.y, e.x, -e.z)));
                    break;
                case 10:
                    r.ForEach((e) => ret.Add((e.x, -e.y, -e.z)));
                    break;
                case 11:
                    r.ForEach((e) => ret.Add((-e.y, -e.x, -e.z)));
                    break;
                case 12:
                    r.ForEach((e) => ret.Add((-e.z, e.y, e.x)));

                    break;
                case 13:
                    r.ForEach((e) => ret.Add((-e.z, e.x, -e.y)));
                    break;
                case 14:
                    r.ForEach((e) => ret.Add((-e.z, -e.y, -e.x)));
                    break;
                case 15:
                    r.ForEach((e) => ret.Add((-e.z, -e.x, e.y)));
                    break;
                case 16:
                    r.ForEach((e) => ret.Add((e.x, -e.z, e.y)));
                    break;
                case 17:
                    r.ForEach((e) => ret.Add((-e.y, -e.z, e.x)));
                    break;
                case 18:
                    r.ForEach((e) => ret.Add((-e.x, -e.z, -e.y)));
                    break;
                case 19:
                    r.ForEach((e) => ret.Add((e.y, -e.z, -e.x)));
                    break;
                case 20:
                    r.ForEach((e) => ret.Add((-e.x, e.z, e.y)));
                    break;
                case 21:
                    r.ForEach((e) => ret.Add((e.y, e.z, e.x)));
                    break;
                case 22:
                    r.ForEach((e) => ret.Add((e.x, e.z, -e.y)));
                    break;
                case 23:
                    r.ForEach((e) => ret.Add((-e.y, e.z, -e.x)));
                    break;
            }
            return ret;
        }

        public static int checkOverlap(List<RotCoord> f, List<RotCoord> s)
        {
            int ret = 0;

            //f.ForEach(c => Console.WriteLine($"{c}"));
            //Console.WriteLine("-----------------");
            //s.ForEach(c => Console.WriteLine($"{c}"));

            foreach (var c in s)
            {
                if (f.Contains(c)) ret++;
            }

            return ret;
        }

        public static (bool, RotCoord) getOverlap(List<RotCoord> f, List<RotCoord> s)
        {
            for (int r = 0; r < 24; r++)
            {
                var rs = rotate90(s, r);

                for (int i = 0; i < f.Count - 11; i++) // xd
                {
                    for (int j = 0; j < rs.Count - 11; j++)
                    {
                        var cdiff = f[i] - rs[j];

                        int o = checkOverlap(f, rs.Select((c) => c + cdiff).ToList());

                        if (o >= 12)
                        {
                            s.Clear();
                            for (int t = 0; t < rs.Count; t++)
                            {
                                s.Add(rs[t] + cdiff);
                            }

                            return (true, cdiff);
                        }
                    }
                }
            }

            return (false, null);
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

            var scanners = new List<List<RotCoord>>();
            List<RotCoord> currentBeacons = null;

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    currentBeacons.OrderBy(x => x.x).ThenBy(y => y.y).ThenBy(z => z);
                    scanners.Add(currentBeacons);
                    continue;
                }

                if (line.Substring(0, 3) == "---")
                {
                    currentBeacons = new List<RotCoord>();
                    continue;
                }

                var cs = line.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                currentBeacons.Add((cs[0], cs[1], cs[2]));

            }
            currentBeacons.OrderBy(x => x.x).ThenBy(y => y.y).ThenBy(z => z);
            scanners.Add(currentBeacons);

            var beacons = new HashSet<RotCoord>();

            var visited = new List<bool>();
            scanners.ForEach(x => visited.Add(false));

            var next = new List<int>();


            next.Add(0);

            var poses = new List<RotCoord>();
            scanners.ForEach(x => poses.Add((0, 0, 0)));

            while (true)
            {
                var nextnext = new List<int>();

                foreach (var i in next)
                {
                    visited[i] = true;

                    for (int j = 0; j < scanners.Count; j++)
                    {
                        if (i == j || visited[j]) continue;//skip self

                        // Console.WriteLine($"Checking {i} with {j}");

                        var o = getOverlap(scanners[i], scanners[j]);

                        if (o.Item1)
                        {
                            //Console.WriteLine($"Scanner {i} and {j} overlap");

                            if (!visited[j])
                            {
                                nextnext.Add(j);
                            }
                            poses[j] += o.Item2;

                            visited[j] = true;
                        }
                    }
                }

                next = nextnext;
                if (nextnext.Count == 0)
                {
                    break;
                }
            }

            foreach (var x in scanners)
            {
                foreach (var c in x)
                {
                    if (!beacons.Add(c))
                    {
                        // Console.WriteLine($"{c} alreday in set");
                    }
                }
            }

            int maxdist = 0;
            for (int i = 0; i < poses.Count; i++)
            {
                for (int j = i + 1; j < poses.Count; j++)
                {
                    var c1 = poses[i];
                    var c2 = poses[j];

                    var dist = Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y) + Math.Abs(c1.z - c2.z);
                    if (dist > maxdist) maxdist = dist;
                }
            }

            /* for (int i = 0; i < scanners.Count; i++)
            {
                for (int j = i; j < scanners.Count; j++)
                {
                    if (i == j) continue;

                    // Console.WriteLine($"Checking {i} with {j}");

                    var o = getOverlap(scanners[j], scanners[i]);

                    if (o)
                    {
                        Console.WriteLine($"Scanner {i} and {j} overlap");

                        matches[i].Add(j);
                        skip[j] = true;
                    }
                }
            } */

            Console.WriteLine($"Task1: {beacons.Count}");
            Console.WriteLine($"Task2: {maxdist}");
        }
    }
}

