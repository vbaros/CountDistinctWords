using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CountDistinctWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\TestFiles\";

            //string filepath = Path.Combine(path, "The Trial.txt");
            string filepath = Path.Combine(path, "test.txt");
            //string filepath = Path.Combine(path, "test2.txt");
            //string filepath = Path.Combine(path, "dict.txt");
            //string filepath = Path.Combine(path, "random.txt");

            if (!File.Exists(filepath))
            {
                Console.WriteLine("The file path: '{0}' doesn't exist.\nAborting program...\n", Path.GetFullPath(filepath));
                return;
            }

            // read a file into a list
            string[] file = File.ReadAllLines(filepath);

            Function[] functions =
            {
               new Function() {function = CountDistinctWords.Normal,                 name = "Normal" },
               new Function() {function = CountDistinctWords.NormalHashSet,          name = "Normal (HashSet)" },
               new Function() {function = CountDistinctWords.ParallelNaive,          name = "Parallel (Naive)" },
               new Function() {function = CountDistinctWords.Parallel,               name = "Parallel"},
               new Function() {function = CountDistinctWords.ParallelWithoutLocking, name = "Parallel (ConcurrentBag)"},
               new Function() {function = CountDistinctWords.Linq,                   name = "Linq" },
               new Function() {function = CountDistinctWords.ParallelLinq,           name = "Parallel Linq" },
            };

            PrintResults(functions, SplitFunctions.SplitToWordsReplace, file);
            PrintResults(functions, SplitFunctions.SplitWithLinq, file);
            PrintResults(functions, SplitFunctions.SplitToWords, file);
            PrintResults(functions, SplitFunctions.SplitToWordsDistinct, file);
            PrintResults(functions, SplitFunctions.SplitToWordsRegex, file);

            //Console.WriteLine("Press any key to continue.");
            //Console.ReadKey();
        }

        static void PrintResults(Function[] functions, Func<string, string[]> splitFunction, string[] file)
        {
            // print a nice table
            Console.WriteLine("{0,-25}{1,12}{2,15}{3,17}", "Function", "Time (msec)", "Distinct words", "Memory used (MB)");
            Console.WriteLine("{0,-25}{1,12}{2,15}{3,17}", "-------------------------", "-----------", "--------------", "----------------");

            // get output to cmd
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            Console.SetOut(new MultiTextWriter(sw, Console.Out));

            // execute functions
            foreach (var function in functions)
            {
                ExecuteAndMesureFunction(function.function, splitFunction, function.name, file);
            }

            Console.WriteLine();

            // color the output (green -> fastest, red -> slowest)
            int posStart = 25, length = 12;
            List<Tuple<string, int>> results = new List<Tuple<string, int>>();

            StringReader sr = new StringReader(sb.ToString());
            string line;
            int max = 0, min = Int32.MaxValue;

            // get maximum and minimum results
            while ((line = sr.ReadLine()) != null)
            {
                try
                {
                    line = line.Substring(posStart, length);
                    int result = int.Parse(line.Trim());
                    results.Add(Tuple.Create(line, result));

                    max = Math.Max(max, result);
                    min = Math.Min(min, result);
                }
                catch (Exception ex)
                {
                    // not a functional crash
                    // do not crash the application if parse doesn't pass or we get the same result for dictionary
                }
            }

            // color the results
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            ConsoleColor currentColor = Console.ForegroundColor; 

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].Item2 == max)
                {
                    Console.SetCursorPosition(25, cursorTop-results.Count-1 + i);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(results[i].Item1);

                    // return to previous position and color
                    Console.ForegroundColor = currentColor;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                }

                if (results[i].Item2 == min)
                {
                    Console.SetCursorPosition(25, cursorTop - results.Count-1 + i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(results[i].Item1);

                    // return to previous position and color
                    Console.ForegroundColor = currentColor;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                }
            }
        }

        static void ExecuteAndMesureFunction (Func<string[],Func<string, string[]>, int> method, Func<string, string[]> splitFunction, string methodName, string[] file)
        {
            GC.Collect(); // ensures optiomal performance

            int count = 0;
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            count = method(file, splitFunction);
            stopwatch.Stop();

            string memoryUsed = (GC.GetTotalMemory(false)/(1024*1024)).ToString();
            Console.WriteLine("{0,-25}{1,12}{2,15}{3,17}", methodName, stopwatch.ElapsedMilliseconds, count, memoryUsed);
        }

        struct Function
        {
            public Func<string[], Func<string, string[]>, int> function;
            public string name;
        }
    }
}
