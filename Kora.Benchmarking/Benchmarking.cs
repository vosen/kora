using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UAM.Kora;
using System.Diagnostics;
using FromMono = Mono.Collections.Generic;
using System.IO;

namespace UAM.Kora
{
    public static class Benchmarking
    {
        static Random random = new Random();
        const StructureType SortedDictionaryMask = StructureType.RBTree | StructureType.VEB | StructureType.XTrieDPH | StructureType.YTrieDPH | StructureType.XTrieStandard | StructureType.YTrieStandard;
        const StructureType DictionaryMask = SortedDictionaryMask | StructureType.DPH;

        static void Fill<T>(uint[] keySet, T[] valueSet, IDictionary<uint, T> dict)
        {
            for (int i = 0; i < keySet.Length; i++)
                dict.Add(keySet[i], valueSet[i]);
        }

        public static MeasureResults MeasureSeriesSearch(StructureType types, int start, int count, int step, int control)
        {
            // validate types
            types &= SortedDictionaryMask;

            // initialize result sets
            Tuple<long, long>[] rbTreeResults = new Tuple<long, long>[count];
            Tuple<long, long>[] vebResults = new Tuple<long, long>[count];
            Tuple<long, long>[] dphResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastStandardResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastStandardResults = new Tuple<long, long>[count];

            // calc the ranges
            int maxval = 2 * Math.Max((int)BitHacks.RoundToPower((uint)(start + (count - 1) * step)), control);
            int width = BitHacks.Power2MSB((uint)maxval);
            int positive = (int)(0.9 * control);
            int negative = control - positive;

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                uint[] searchSet = new uint[control];
                HashSet<uint> itemSet = GenerateRandomSet(size, maxval);
                uint[] itemArray = itemSet.ToArray();
                int j = 0;
                for (; j < positive; j++)
                    searchSet[j] = itemArray[random.Next(itemArray.Length)];
                while(j < searchSet.Length)
                {
                    uint next = (uint)random.Next(maxval);
                    if(!itemSet.Contains(next))
                    {
                        searchSet[j] = next;
                        j++;
                    }
                }

                if (types.HasFlag(StructureType.RBTree))
                {
                    var dict = new FromMono.SortedDictionary<uint, uint>();
                    Fill(itemArray, itemArray, dict);
                    rbTreeResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.VEB))
                {
                    var dict = new VEBTree<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    vebResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.DPH))
                {
                    var dict = new HashTable<uint>();
                    Fill(itemArray, itemArray, dict);
                    dphResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieDPH))
                {
                    var dict = new XFastTrie<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    xfastDPHResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieDPH))
                {
                    var dict = new YFastTrie<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    yfastDPHResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieStandard))
                {
                    var dict = XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(itemArray, itemArray, dict);
                    xfastStandardResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieStandard))
                {
                    var dict = YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(itemArray, itemArray, dict);
                    yfastStandardResults[i] = Tuple.Create((long)size, MeasureSearch(searchSet, dict));
                }
                i++;
            }

            // dump the results
            MeasureResults results = new MeasureResults(types, maxval);
            results.SetResults(StructureType.RBTree, rbTreeResults);
            results.SetResults(StructureType.VEB, vebResults);
            results.SetResults(StructureType.DPH, dphResults);
            results.SetResults(StructureType.XTrieDPH, xfastDPHResults);
            results.SetResults(StructureType.YTrieDPH, yfastDPHResults);
            results.SetResults(StructureType.XTrieStandard, xfastStandardResults);
            results.SetResults(StructureType.YTrieStandard, yfastStandardResults);
            return results;
        }

        public static MeasureResults MeasureSeriesSuccessor(StructureType types, int start, int count, int step, int control)
        {
            // validate types
            types &= SortedDictionaryMask;

            // initialize result sets
            Tuple<long, long>[] rbTreeResults = new Tuple<long, long>[count];
            Tuple<long, long>[] vebResults = new Tuple<long, long>[count];
            Tuple<long, long>[] dphResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastStandardResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastStandardResults = new Tuple<long, long>[count];

            // calc the ranges
            int maxval = 2 * (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                uint[] itemArray = GenerateRandomSet(size, maxval).ToArray();
                uint[] searchSet = GenerateRandomSet(control, maxval).ToArray();

                if (types.HasFlag(StructureType.RBTree))
                {
                    var dict = new FromMono.SortedDictionary<uint, uint>();
                    Fill(itemArray, itemArray, dict);
                    rbTreeResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                if (types.HasFlag(StructureType.VEB))
                {
                    var dict = new VEBTree<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    vebResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieDPH))
                {
                    var dict = new XFastTrie<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    xfastDPHResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieDPH))
                {
                    var dict = new YFastTrie<uint>(width);
                    Fill(itemArray, itemArray, dict);
                    yfastDPHResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieStandard))
                {
                    var dict = XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(itemArray, itemArray, dict);
                    xfastStandardResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieStandard))
                {
                    var dict = YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(itemArray, itemArray, dict);
                    yfastStandardResults[i] = Tuple.Create((long)size, MeasureSuccessor(searchSet, dict));
                }
                i++;
            }

            // dump the results
            MeasureResults results = new MeasureResults(types, maxval);
            results.SetResults(StructureType.RBTree, rbTreeResults);
            results.SetResults(StructureType.VEB, vebResults);
            results.SetResults(StructureType.DPH, dphResults);
            results.SetResults(StructureType.XTrieDPH, xfastDPHResults);
            results.SetResults(StructureType.YTrieDPH, yfastDPHResults);
            results.SetResults(StructureType.XTrieStandard, xfastStandardResults);
            results.SetResults(StructureType.YTrieStandard, yfastStandardResults);
            return results;
        }

        // that one parameter is there to 
        public static MeasureResults MeasureSeriesMemory(StructureType types, int start, int count, int step)
        {
            // validate types
            types &= DictionaryMask;

            // initialize result sets
            Tuple<long, long>[] rbTreeResults = new Tuple<long, long>[count];
            Tuple<long, long>[] vebResults = new Tuple<long, long>[count];
            Tuple<long, long>[] dphResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastStandardResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastStandardResults = new Tuple<long, long>[count];

            // calc the ranges
            int maxval = (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                var itemSet = GenerateRandomSet(size, maxval).ToArray();
                if (types.HasFlag(StructureType.RBTree))
                {
                    rbTreeResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = new FromMono.SortedDictionary<uint, uint>();
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.VEB))
                {
                    vebResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = new VEBTree<uint>(width);
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.DPH))
                {
                    dphResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = new HashTable<uint>();
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.XTrieDPH))
                {
                    xfastDPHResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = new XFastTrie<uint>(width);
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.YTrieDPH))
                {
                    yfastDPHResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = new YFastTrie<uint>(width);
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.XTrieStandard))
                {
                    xfastStandardResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                if (types.HasFlag(StructureType.YTrieStandard))
                {
                    yfastStandardResults[i] = Tuple.Create(
                        (long)size,
                        MeasureMemory(() =>
                        {
                            var dict = YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                            Fill(itemSet, itemSet, dict);
                            return dict;
                        }));
                }
                i++;
            }

            // dump the results
            MeasureResults results = new MeasureResults(types, maxval);
            results.SetResults(StructureType.RBTree, rbTreeResults);
            results.SetResults(StructureType.VEB, vebResults);
            results.SetResults(StructureType.DPH, dphResults);
            results.SetResults(StructureType.XTrieDPH, xfastDPHResults);
            results.SetResults(StructureType.YTrieDPH, yfastDPHResults);
            results.SetResults(StructureType.XTrieStandard, xfastStandardResults);
            results.SetResults(StructureType.YTrieStandard, yfastStandardResults);
            return results;
        }

        public static MeasureResults MeasureSeriesAdd(StructureType types, int start, int count, int step)
        {
            // validate types
            types &= DictionaryMask;

            // initialize result sets
            Tuple<long, long>[] rbTreeResults = new Tuple<long, long>[count];
            Tuple<long, long>[] vebResults = new Tuple<long, long>[count];
            Tuple<long, long>[] dphResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastStandardResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastStandardResults = new Tuple<long, long>[count];

            // calc the ranges
            int maxval = 2 * (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                var itemSet = GenerateRandomSet(size, maxval).ToArray();
                if(types.HasFlag(StructureType.RBTree))
                    rbTreeResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet,itemSet, new FromMono.SortedDictionary<uint, uint>()));
                if(types.HasFlag(StructureType.VEB))
                    vebResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, new VEBTree<uint>(width)));
                if (types.HasFlag(StructureType.DPH))
                    dphResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, new HashTable<uint>()));
                if (types.HasFlag(StructureType.XTrieDPH))
                    xfastDPHResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, new XFastTrie<uint>(width)));
                if (types.HasFlag(StructureType.YTrieDPH))
                    yfastDPHResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, new YFastTrie<uint>(width)));
                if (types.HasFlag(StructureType.XTrieStandard))
                    xfastStandardResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                if (types.HasFlag(StructureType.YTrieStandard))
                    yfastStandardResults[i] = Tuple.Create((long)size, MeasureAdd(itemSet, itemSet, YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width)));
                i++;
            }

            // dump the results
            MeasureResults results = new MeasureResults(types, maxval);
            results.SetResults(StructureType.RBTree, rbTreeResults);
            results.SetResults(StructureType.VEB, vebResults);
            results.SetResults(StructureType.DPH, dphResults);
            results.SetResults(StructureType.XTrieDPH, xfastDPHResults);
            results.SetResults(StructureType.YTrieDPH, yfastDPHResults);
            results.SetResults(StructureType.XTrieStandard, xfastStandardResults);
            results.SetResults(StructureType.YTrieStandard, yfastStandardResults);
            return results;
        }

        public static MeasureResults MeasureSeriesDelete(StructureType types, int start, int count, int step)
        {
            // validate types
            types &= DictionaryMask;

            // initialize result sets
            Tuple<long, long>[] rbTreeResults = new Tuple<long, long>[count];
            Tuple<long, long>[] vebResults = new Tuple<long, long>[count];
            Tuple<long, long>[] dphResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastDPHResults = new Tuple<long, long>[count];
            Tuple<long, long>[] xfastStandardResults = new Tuple<long, long>[count];
            Tuple<long, long>[] yfastStandardResults = new Tuple<long, long>[count];

            // calc the ranges
            int maxval = 2 * (int)BitHacks.RoundToPower((uint)(start + (count - 1) * step));
            int width = BitHacks.Power2MSB((uint)maxval);

            // run benchmarks
            int i = 0;
            foreach (var size in Enumerable.Range(0, count).Select(x => start + (x * step)))
            {
                var itemSet = GenerateRandomSet(size, maxval).ToArray();
                var delSet = itemSet.ToArray();
                Shuffle(delSet);

                if (types.HasFlag(StructureType.RBTree))
                {
                    var dict = new FromMono.SortedDictionary<uint, uint>();
                    Fill(delSet, delSet, dict);
                    rbTreeResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.VEB))
                {
                    var dict = new VEBTree<uint>(width);
                    Fill(delSet, delSet, dict);
                    vebResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.DPH))
                {
                    var dict = new HashTable<uint>();
                    Fill(delSet, delSet, dict);
                    dphResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieDPH))
                {
                    var dict = new XFastTrie<uint>(width);
                    Fill(delSet, delSet, dict);
                    xfastDPHResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieDPH))
                {
                    var dict = new YFastTrie<uint>(width);
                    Fill(delSet, delSet, dict);
                    yfastDPHResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.XTrieStandard))
                {
                    var dict = XFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(delSet, delSet, dict);
                    xfastStandardResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                if (types.HasFlag(StructureType.YTrieStandard))
                {
                    var dict = YFastTrie<uint>.FromDictionary<Dictionary<uint, XFastNode>>(width);
                    Fill(delSet, delSet, dict);
                    yfastStandardResults[i] = Tuple.Create((long)size, MeasureDelete(delSet, dict));
                }
                i++;
            }

            // dump the results
            MeasureResults results = new MeasureResults(types, maxval);
            results.SetResults(StructureType.RBTree, rbTreeResults);
            results.SetResults(StructureType.VEB, vebResults);
            results.SetResults(StructureType.DPH, dphResults);
            results.SetResults(StructureType.XTrieDPH, xfastDPHResults);
            results.SetResults(StructureType.YTrieDPH, yfastDPHResults);
            results.SetResults(StructureType.XTrieStandard, xfastStandardResults);
            results.SetResults(StructureType.YTrieStandard, yfastStandardResults);
            return results;
        }

        static long MeasureAdd<T>(uint[] keySet, T[] valueSet, IDictionary<uint, T> dict)
        {
            GC.Collect();
            var timer = Stopwatch.StartNew();
            for (int i = 0; i < keySet.Length; i++)
                dict.Add(keySet[i], valueSet[i]);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long MeasureDelete<T>(uint[] itemSet, IDictionary<uint, T> dict)
        {
            GC.Collect();
            var timer = Stopwatch.StartNew();
            for (int i = 0; i < itemSet.Length; i++)
                dict.Remove(itemSet[i]);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long MeasureSearch<T>(uint[] itemSet, IDictionary<uint, T> dict)
        {
            GC.Collect();
            var timer = Stopwatch.StartNew();
            T temp;
            for (int i = 0; i < itemSet.Length; i++)
                dict.TryGetValue(itemSet[i], out temp);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long MeasureSuccessor<T>(uint[] itemSet, ISortedDictionary<uint, T> dict)
        {
            GC.Collect();
            var timer = Stopwatch.StartNew();
            for (int i = 0; i < itemSet.Length; i++)
                dict.Higher(itemSet[i]);
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        static long MeasureMemory<T>(Func<T> generator)
        {
            T temp;
            GC.Collect();
            long start = GC.GetTotalMemory(true);
            temp = generator();
            GC.Collect();
            long space = GC.GetTotalMemory(false) - start;
            GC.KeepAlive(temp);
            return space;
        }

        public static HashSet<uint> GenerateRandomSet(int size, int range)
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
