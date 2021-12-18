using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// See https://aka.ms/new-console-template for more information

namespace Aoc2021
{
    public class Program
    {
        public static long magnitude(string inp, ref int at)
        {
            //3*l + 2*r;
            long left = 0;
            long right = 0;
            at++;

            if (inp[at] == '[')
            {
                left = magnitude(inp, ref at);
            }
            else
            {
                //Console.WriteLine($"Getting left value from at {at}");
                left = Convert.ToInt32(inp.Substring(at, 1));
            }

            // skip '<number>,'
            at += 2;

            if (inp[at] == '[')
            {
                right = magnitude(inp, ref at);
            }
            else
            {
                //Console.WriteLine($"Getting right value from at {at}");
                right = Convert.ToInt32(inp.Substring(at, 1));
            }

            at++;

            return 3 * left + 2 * right;
        }

        public static bool explode(string inp, out string outp)
        {
            int depth = 0;
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i] == ']') depth--;
                if (inp[i] == '[') depth++;

                if (depth == 5)
                {
                    outp = "";
                    var lNumLen = 0;
                    int iNumLen = 0;

                    for (int t = 1; Char.IsNumber(inp[i + t]); iNumLen++, t++) ;
                    lNumLen = iNumLen;

                    // get first number on the left
                    int j = i;
                    while (j > 0 && !Char.IsNumber(inp[j])) j--;
                    if (j > 0)
                    {
                        // get num lengths
                        int jNumLen = 0;
                        for (int t = j; Char.IsNumber(inp[t]); jNumLen++, t--) ;

                        outp = inp.Substring(0, j - jNumLen + 1);

                        var newN = inp.Substring(i + 1, iNumLen);
                        var rightN = inp.Substring(j - jNumLen + 1, jNumLen);


                        outp += (Convert.ToInt32(newN) + Convert.ToInt32(rightN));

                        outp += inp.Substring(j + 1, i - j - 1);
                    }
                    else
                    {
                        // nothing left
                        outp = inp.Substring(0, i);
                    }

                    outp += '0';

                    iNumLen = 0;
                    for (int t = lNumLen + 2; Char.IsNumber(inp[i + t]); iNumLen++, t++) ;

                    // get first number on the right
                    j = i + 2 + iNumLen + lNumLen;

                    while (j < inp.Length && !Char.IsNumber(inp[j])) j++;
                    if (j < inp.Length)
                    {
                        // get num length
                        int jNumLen = 0;
                        for (int t = j; Char.IsNumber(inp[t]); jNumLen++, t++) ;

                        /* Console.WriteLine($"iNum from {i + 3} is {inp.Substring(i + 3)}");
                        Console.WriteLine($"jNum from {j} is {inp.Substring(j)}");
                        Console.WriteLine($"iNumLen {iNumLen}, jNumLen {jNumLen}, lNumLen {lNumLen}"); */

                        outp += inp.Substring(i + 3 + lNumLen + iNumLen, j - (i + 3 + lNumLen + iNumLen));


                        var newN = inp.Substring(i + 2 + lNumLen, iNumLen);
                        var rightN = inp.Substring(j, jNumLen);

                        outp += (Convert.ToInt32(newN) + Convert.ToInt32(rightN));

                        outp += inp.Substring(j + jNumLen);
                    }
                    else
                    {
                        //nothing right
                        outp += inp.Substring(i + 3 + lNumLen + iNumLen);
                    }

                    return true;
                }
            }

            outp = inp;
            return false;
        }

        public static bool split(string inp, out string outp)
        {
            for (int i = 0; i < inp.Length - 1; i++)
            {
                if (Char.IsNumber(inp[i]) && Char.IsNumber(inp[i + 1]))
                {
                    outp = inp.Substring(0, i);
                    var bigNum = Convert.ToInt32(inp.Substring(i, 2));
                    outp += "[" + (bigNum >> 1) + "," + Math.Ceiling(bigNum / 2.0) + "]";
                    outp += inp.Substring(i + 2);

                    return true;
                }
            }

            outp = inp;
            return false;
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


            string sailfish = inputStream.ReadLine();

            List<string> sailfishLines = new List<string>();
            sailfishLines.Add(sailfish);

            while (inputStream.Peek() >= 0)
            {
                string line = inputStream.ReadLine();
                line = line.TrimEnd("\n ".ToCharArray());
                if (line == null || line.Length <= 1)
                {
                    break;
                }

                sailfishLines.Add(line);

                // add
                sailfish = "[" + sailfish + "," + line + "]";

                var changes = true;

                while (changes)
                {
                    changes = false;

                    while (explode(sailfish, out sailfish))
                    {
                        changes = true;
                    }

                    if (split(sailfish, out sailfish))
                    {
                        changes = true;
                    }
                }
            }

            long max = 0;
            for (int i = 0; i < sailfishLines.Count; i++)
            {
                for (int j = 0; j < sailfishLines.Count; j++)
                {
                    var cs = "[" + sailfishLines[i] + "," + sailfishLines[j] + "]";

                    var changes = true;
                    while (changes)
                    {
                        changes = false;

                        while (explode(cs, out cs))
                        {
                            changes = true;
                        }

                        if (split(cs, out cs))
                        {
                            changes = true;
                        }
                    }

                    int at1 = 0;
                    long magn = magnitude(cs, ref at1);
                    if (magn > max) max = magn;

                    cs = "[" + sailfishLines[j] + "," + sailfishLines[i] + "]";

                    changes = true;
                    while (changes)
                    {
                        changes = false;

                        while (explode(cs, out cs))
                        {
                            changes = true;
                        }

                        if (split(cs, out cs))
                        {
                            changes = true;
                        }
                    }

                    at1 = 0;
                    magn = magnitude(cs, ref at1);
                    if (magn > max) max = magn;
                }

            }


            int at = 0;
            Console.WriteLine($"Task1: {magnitude(sailfish, ref at)}");
            Console.WriteLine($"Task2: {max}");
        }
    }
}

