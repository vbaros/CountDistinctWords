using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountDistinctWords
{
    public static class CountDistinctWords
    {
        public static int Normal(string[] file, Func<string, string[]> splitFunction)
        {
            List<string> distinct = new List<string>();

            foreach (string line in file)
            {
                distinct.AddRange(splitFunction(line));
            }

            return distinct.Distinct().Count();
        }

        public static int NormalHashSet(string[] file, Func<string, string[]> splitFunction)
        {
            HashSet<string> distinct = new HashSet<string>();

            foreach (string line in file)
            {
                distinct.UnionWith(splitFunction(line));
            }

            return distinct.Count;
        }

        public static int ParallelNaive(string[] file, Func<string, string[]> splitFunction)
        {
            ConcurrentBag<string> distinct = new ConcurrentBag<string>();

            System.Threading.Tasks.Parallel.ForEach(file, line =>
            {
                foreach (string s in splitFunction(line))
                {
                    distinct.Add(s);
                }
            });

            return distinct.AsParallel().Distinct().Count();
        }

        public static int Parallel(string[] file, Func<string, string[]> splitFunction)
        {
            object locker = new object();
            List<string> distinct = new List<string>();

            System.Threading.Tasks.Parallel.ForEach(
                    file,                                    // source

                    () => new List<string>(),                // local init

                    (line, state, localTotal) =>             // main body
                    {
                        localTotal.AddRange(splitFunction(line));
                        return localTotal;
                    },

                    localTotal =>                            // Add up the local values
                    {
                        lock (locker) distinct.AddRange(localTotal);
                    }
                );

            return distinct.AsParallel().Distinct().Count();
        }

        public static int ParallelWithoutLocking(string[] file, Func<string, string[]> splitFunction)
        {
            ConcurrentBag<string> distinct = new ConcurrentBag<string>();

            System.Threading.Tasks.Parallel.ForEach(
                    file,                                    // source

                    () => new List<string>(),                // local init

                    (line, state, localTotal) =>             // main body
                    {
                        localTotal.AddRange(splitFunction(line));
                        return localTotal;
                    },

                    localTotal =>                            // Add the local value
                    {
                        foreach (string word in localTotal)
                        {
                            distinct.Add(word);
                        }
                    }
                );

            return distinct.AsParallel().Distinct().Count();
        }

        public static int ParallelLinq(string[] file, Func<string, string[]> splitFunction)
        {
            return file.AsParallel().SelectMany(line => splitFunction(line)).Distinct().Count();
        }

        public static int Linq(string[] file, Func<string, string[]> splitFunction)
        {
            return file.SelectMany(line => splitFunction(line)).Distinct().Count();
        }
    }
}
