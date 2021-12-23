using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {

        public class Instruction
        {
            public bool on;
            public int x1, x2;
            public int y1, y2;
            public int z1, z2;

            public override string ToString()
            {
                return (on ? "on" : "off") + " " + "x=" + x1 + ".." + x2 + ",y=" + y1 + ".." + y2 + ",z=" + z1 + ".." + z2;
            }
        }

        public class Block
        {
            public long x1, x2;
            public long y1, y2;
            public long z1, z2;

            public Block(int x1, int x2, int y1, int y2, int z1, int z2)
            {
                this.x1 = (long)x1;
                this.x2 = (long)x2;
                this.y1 = (long)y1;
                this.y2 = (long)y2;
                this.z1 = (long)z1;
                this.z2 = (long)z2;
            }
            public Block(long x1, long x2, long y1, long y2, long z1, long z2)
            {
                this.x1 = (long)x1;
                this.x2 = (long)x2;
                this.y1 = (long)y1;
                this.y2 = (long)y2;
                this.z1 = (long)z1;
                this.z2 = (long)z2;
            }

            public override string ToString()
            {
                return "x=" + x1 + ".." + x2 + ",y=" + y1 + ".." + y2 + ",z=" + z1 + ".." + z2;
            }

            public long volume
            {
                get
                {
                    return Math.Abs(this.x2 - this.x1 + 1)
                            * Math.Abs(this.y2 - this.y1 + 1)
                            * Math.Abs(this.z2 - this.z1 + 1);
                }
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

            var intstructions = new List<Instruction>();

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    continue;
                }

                var intstr = new Instruction();
                intstr.on = true;
                if (line.StartsWith("off")) intstr.on = false;

                var cs = line.Split(",");

                var xs = cs[0].Split("=")[1].Split("..");
                intstr.x1 = Convert.ToInt32(xs[0]);
                intstr.x2 = Convert.ToInt32(xs[1]);

                var ys = cs[1].Split("=")[1].Split("..");
                intstr.y1 = Convert.ToInt32(ys[0]);
                intstr.y2 = Convert.ToInt32(ys[1]);

                var zs = cs[2].Split("=")[1].Split("..");
                intstr.z1 = Convert.ToInt32(zs[0]);
                intstr.z2 = Convert.ToInt32(zs[1]);

                intstructions.Add(intstr);
            }

            var cuboids = new HashSet<(int, int, int)>();

            foreach (var instr in intstructions)
            {
                if (instr.x1 < -50 || instr.x2 > 50
                    || instr.y1 < -50 || instr.y2 > 50
                    || instr.z1 < -50 || instr.z2 > 50)
                {
                    continue; // skip part1
                }

                //Console.WriteLine($"Executing {instr}");

                for (int x = instr.x1; x <= instr.x2; x++)
                {
                    for (int y = instr.y1; y <= instr.y2; y++)
                    {
                        for (int z = instr.z1; z <= instr.z2; z++)
                        {
                            if (instr.on)
                            {
                                if (cuboids.Add((x, y, z)))
                                {
                                    //Console.WriteLine($"added {(x, y, z)}");
                                }
                                else
                                {
                                    //Console.WriteLine($"skip add {(x, y, z)}");
                                }
                            }
                            else
                            {
                                if (cuboids.Remove((x, y, z)))
                                {
                                    //Console.WriteLine($"removed {(x, y, z)}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Task1: {cuboids.Count}");

            var addBlocks = new List<Block>();
            var subBlocks = new List<Block>();


            foreach (var instr in intstructions)
            {
                int addl = addBlocks.Count;
                int subl = subBlocks.Count;

                for (int i = 0; i < addl; i++)
                {
                    var blk = addBlocks[i];

                    if ((instr.x1 <= blk.x2 && instr.x2 >= blk.x1)
                        && (instr.y1 <= blk.y2 && instr.y2 >= blk.y1)
                        && (instr.z1 <= blk.z2 && instr.z2 >= blk.z1))
                    {
                        // on block is inside an already added block

                        long nx1 = (long)instr.x1 > blk.x1 ? (long)instr.x1 : blk.x1;
                        long nx2 = (long)instr.x2 < blk.x2 ? (long)instr.x2 : blk.x2;

                        long ny1 = (long)instr.y1 > blk.y1 ? (long)instr.y1 : blk.y1;
                        long ny2 = (long)instr.y2 < blk.y2 ? (long)instr.y2 : blk.y2;

                        long nz1 = (long)instr.z1 > blk.z1 ? (long)instr.z1 : blk.z1;
                        long nz2 = (long)instr.z2 < blk.z2 ? (long)instr.z2 : blk.z2;

                        var nBlk = new Block(nx1, nx2, ny1, ny2, nz1, nz2);

                        // has to be substracted
                        subBlocks.Add(nBlk);
                    }
                }

                for (int i = 0; i < subl; i++)
                {
                    var blk = subBlocks[i];
                    if ((instr.x1 <= blk.x2 && instr.x2 >= blk.x1)
                        && (instr.y1 <= blk.y2 && instr.y2 >= blk.y1)
                        && (instr.z1 <= blk.z2 && instr.z2 >= blk.z1))
                    {
                        // on block is inside an already substracted block

                        long nx1 = (long)instr.x1 > blk.x1 ? (long)instr.x1 : blk.x1;
                        long nx2 = (long)instr.x2 < blk.x2 ? (long)instr.x2 : blk.x2;

                        long ny1 = (long)instr.y1 > blk.y1 ? (long)instr.y1 : blk.y1;
                        long ny2 = (long)instr.y2 < blk.y2 ? (long)instr.y2 : blk.y2;

                        long nz1 = (long)instr.z1 > blk.z1 ? (long)instr.z1 : blk.z1;
                        long nz2 = (long)instr.z2 < blk.z2 ? (long)instr.z2 : blk.z2;

                        var nBlk = new Block(nx1, nx2, ny1, ny2, nz1, nz2);

                        // has to be substracted
                        addBlocks.Add(nBlk);
                    }
                }

                var neBlk = new Block(instr.x1, instr.x2, instr.y1, instr.y2, instr.z1, instr.z2);

                if (instr.on)
                    addBlocks.Add(neBlk);
            }

            long vol = 0;

            foreach (var blk in addBlocks)
            {
                vol += blk.volume;
            }
            foreach (var blk in subBlocks)
            {
                vol -= blk.volume;
            }

            Console.WriteLine($"Task2: {vol}");
        }
    }
}

