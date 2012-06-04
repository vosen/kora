using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UAM.Kora;
using System.Diagnostics;
using FromMono = Mono.Collections.Generic;
using System.IO;

namespace Kora.Benchmarks
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {

            if (args.Length != 6 && args.Length != 7)
            {
                Console.WriteLine("USAGE: Kora.Benchmark.exe PATH START COUNT STEP <F|S> <ad|r> [RETRIEVALS]");
                Console.WriteLine("PATH - where to dump the results");
                Console.WriteLine("START, COUNT, STEP - controls amount of elements in collections.");
                Console.WriteLine("<F|S> - F is for fast structures only: rbtree, veb, xtrie-std, ytrie-std, S is for all");
                Console.WriteLine("<ad|r> - benchmark add&del or retrieval");
                Console.WriteLine("RETRIEVALS - When measuring retrievals how many retrievals should we attempt.");
                return;
            }
            string path = args[0];
            int start = int.Parse(args[1]);
            int count = int.Parse(args[2]);
            int step = int.Parse(args[3]);
            bool fastOnly;
            if (args[4].Equals("f", StringComparison.OrdinalIgnoreCase))
                fastOnly = true;
            else if (args[4].Equals("s", StringComparison.OrdinalIgnoreCase))
                fastOnly = false;
            else
                throw new ArgumentException();

            bool addDel;
            if (args[5].Equals("ad", StringComparison.OrdinalIgnoreCase))
                addDel = true;
            else if (args[5].Equals("r", StringComparison.OrdinalIgnoreCase))
                addDel = false;
            else
                throw new ArgumentException();

            int rets = !addDel ? int.Parse(args[6]) : 0;

            if (addDel)
                MeasureAddDel(path, start, count, step, fastOnly);
            else
                MeasureSearch(path, start, count, step, fastOnly, rets);
        }

        private static void MeasureSearch(string path, int start, int count, int step, bool fastOnly, int rets)
        {
            // initialize result sets
            Tuple<int, long>[] rbTreeResults = new Tuple<int, long>[count];
            Tuple<int, long>[] vebResults = new Tuple<int, long>[count];
            Tuple<int, long>[] xfastStandardResults = new Tuple<int, long>[count];
            Tuple<int, long>[] yfastStandardResults = new Tuple<int, long>[count];
            Tuple<int, long>[] dphResults = new Tuple<int, long>[count];
            Tuple<int, long>[] xfastDPHResults = new Tuple<int, long>[count];
            Tuple<int, long>[] yfastDPHResults = new Tuple<int, long>[count];

            // calc the ranges
            int maxval = (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);
            int positive = (int)(0.9 * rets);
            int negative = rets - positive;

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                HashSet<uint> itemSet = GenerateRandomSet(size, maxval);
                uint[] itemCopy = itemSet.ToArray();
                Shuffle(itemCopy);
                var posSet = itemCopy.Take(positive);
                HashSet<uint> negSet = new HashSet<uint>();
                while (negSet.Count < negative)
                {
                    uint rand = (uint)random.Next(maxval);
                    if (!itemSet.Contains(rand))
                        negSet.Add(rand);
                }
                uint[] searchSet = posSet.Concat(negSet).ToArray();

                Console.WriteLine("Measuring search performance for size {0}.", size);
                rbTreeResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, new FromMono.SortedDictionary<uint, uint>()));
                GC.Collect();
                vebResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, new VEBTree<uint>(width)));
                GC.Collect();
                xfastStandardResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                GC.Collect();
                yfastStandardResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                GC.Collect();
                if (!fastOnly)
                {
                    dphResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, new HashTable<uint>()));
                    GC.Collect();
                    xfastDPHResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, new XFastTrie<uint>(width)));
                    GC.Collect();
                    yfastDPHResults[i] = Tuple.Create(size, MeasureSearch(itemSet, searchSet, new YFastTrie<uint>(width)));
                    GC.Collect();
                }
                i++;
            }

            // dump results
            DumpResults(rbTreeResults, Path.Combine(path, "RBTree"));
            DumpResults(vebResults, Path.Combine(path, "VEB"));
            DumpResults(xfastStandardResults, Path.Combine(path, "XFast-Standard"));
            DumpResults(yfastStandardResults, Path.Combine(path, "YFast-Standard"));
            if (!fastOnly)
            {
                DumpResults(dphResults, Path.Combine(path, "DPH"));
                DumpResults(xfastDPHResults, Path.Combine(path, "XFast-DPH"));
                DumpResults(yfastDPHResults, Path.Combine(path, "YFast-DPH"));
            }
        }

        private static void MeasureAddDel(string path, int start, int count, int step, bool fastOnly)
        {
            // initialize result sets
            Tuple<int, Tuple<long, long>>[] rbTreeResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] vebResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] xfastStandardResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] yfastStandardResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] dphResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] xfastDPHResults = new Tuple<int, Tuple<long, long>>[count];
            Tuple<int, Tuple<long, long>>[] yfastDPHResults = new Tuple<int, Tuple<long, long>>[count];
            
            // calc the ranges
            int maxval = (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                Console.WriteLine("Measuring add/delete performance for size {0}.", size);
                rbTreeResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, new FromMono.SortedDictionary<uint, uint>()));
                GC.Collect();
                vebResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, new VEBTree<uint>(width)));
                GC.Collect();
                xfastStandardResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                GC.Collect();
                yfastStandardResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                GC.Collect();
                if (!fastOnly)
                {
                    dphResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, new HashTable<uint>()));
                    GC.Collect();
                    xfastDPHResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, new XFastTrie<uint>(width)));
                    GC.Collect();
                    yfastDPHResults[i] = Tuple.Create(size, MeasureSingleAddDel(size, maxval, new YFastTrie<uint>(width)));
                    GC.Collect();
                }
                i++;
            }

            // dump addition results
            DumpResults(rbTreeResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "RBTree-ADD"));
            DumpResults(vebResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "VEB-ADD"));
            DumpResults(xfastStandardResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "XFast-Standard-ADD"));
            DumpResults(yfastStandardResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "YFast-Standard-ADD"));
            if (!fastOnly)
            {
                DumpResults(dphResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "DPH-ADD"));
                DumpResults(xfastDPHResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "XFast-DPH-ADD"));
                DumpResults(yfastDPHResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item1)), Path.Combine(path, "YFast-DPH-ADD"));
            }

            // dump deletion results
            DumpResults(rbTreeResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "RBTree-DEL"));
            DumpResults(vebResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "VEB-DEL"));
            DumpResults(xfastStandardResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "XFast-Standard-DEL"));
            DumpResults(yfastStandardResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "YFast-Standard-DEL"));
            if (!fastOnly)
            {
                DumpResults(dphResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "DPH-DEL"));
                DumpResults(xfastDPHResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "XFast-DPH-DEL"));
                DumpResults(yfastDPHResults.Select(t => Tuple.Create(t.Item1, t.Item2.Item2)), Path.Combine(path, "YFast-DPH-DEL"));
            }
        }

        static void DumpResults(IEnumerable<Tuple<int, long>> results, string name)
        {
            using (var writer = File.CreateText(name))
            {
                foreach (var result in results)
                    writer.WriteLine(result.Item1 + "," + result.Item2);
            }
        }

        static long MeasureSearch(IEnumerable<uint> itemSet, uint[] searchSet, IDictionary<uint, uint> dict)
        {
            foreach(uint elm in itemSet)
                dict.Add(elm, elm);
            var timer = new Stopwatch();
            timer.Start();
            uint temp;
            for (int i = 0; i < searchSet.Length; i++)
                dict.TryGetValue(searchSet[i], out temp);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static Tuple<long, long> MeasureSingleAddDel(int size, int range, IDictionary<uint, uint> dict)
        {
            long addTime, removeTime;
            uint[] additionSet = GenerateRandomSet(size, range).ToArray();
            Shuffle(additionSet);
            uint[] removalSet = (uint[])additionSet.Clone();
            Shuffle(removalSet);
            var timer = new Stopwatch();
            timer.Restart();
            for (int i = 0; i < size; i++)
            {
                dict.Add(additionSet[i], additionSet[i]);
            }
            timer.Stop();
            addTime = timer.ElapsedMilliseconds;
            timer.Restart();
            for (int i = 0; i < size; i++)
            {
                bool result = dict.Remove(removalSet[i]);
            }
            timer.Stop();
            removeTime = timer.ElapsedMilliseconds;
            return new Tuple<long, long>(addTime, removeTime);
        }

        static private HashSet<uint> GenerateRandomSet(int size, int range)
        {
            var rand = new Random();
            var set = new HashSet<uint>();
            while (set.Count < size)
            {
                int next = rand.Next(range);
                set.Add((uint)next);
            }
            return set;
        }

        public static void Shuffle<T>(T[] array)
        {
            var random = new Random();
            for (int i = array.Length; i > 1; i--)
            {
                // Pick random element to swap.
             int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }
    }
}
