using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using UAM.Kora;

namespace UAM.Kora.Console
{
    class Program
    {
        static Random random = new Random();
        static StructureType Fast = StructureType.RBTree | StructureType.VEB | StructureType.XTrieStandard | StructureType.YTrieStandard;
        static StructureType All = Fast | StructureType.DPH | StructureType.XTrieDPH | StructureType.YTrieDPH;

        static void Main(string[] args)
        {
            if (args.Length != 6 && args.Length != 7)
            {
                System.Console.WriteLine("USAGE: Kora.Benchmark.exe PATH START COUNT STEP <F|S> <ad|r> [CONTROL]");
                System.Console.WriteLine("PATH - where to dump the results");
                System.Console.WriteLine("START, COUNT, STEP - controls amount of elements in collections.");
                System.Console.WriteLine("<F|S> - F is for fast structures only: rbtree, veb, xtrie-std, ytrie-std, S is for all");
                System.Console.WriteLine("<a|d|f|s|m> - benchmark type: (a)dd, (d)el, (f)ind, (s)ucc, (m)emory");
                System.Console.WriteLine("CONTROL - When measuring find and succ how many keys should we search");
                return;
            }
            string path = args[0];
            int start = int.Parse(args[1]);
            int count = int.Parse(args[2]);
            int step = int.Parse(args[3]);

            StructureType types = 0;
            if (args[4].Equals("f", StringComparison.OrdinalIgnoreCase))
                types = Fast;
            else if (args[4].Equals("s", StringComparison.OrdinalIgnoreCase))
                types = All;
            else
                throw new ArgumentException();

            BenchmarkType type;
            if (args[5].Equals("a", StringComparison.OrdinalIgnoreCase))
                type = BenchmarkType.Add;
            else if (args[5].Equals("d", StringComparison.OrdinalIgnoreCase))
                type = BenchmarkType.Delete;
            else if (args[5].Equals("f", StringComparison.OrdinalIgnoreCase))
                type = BenchmarkType.Search;
            else if (args[5].Equals("s", StringComparison.OrdinalIgnoreCase))
                type = BenchmarkType.Successor;
            else if (args[5].Equals("m", StringComparison.OrdinalIgnoreCase))
                type = BenchmarkType.Memory;
            else
                throw new ArgumentException();

            int rets = (type == BenchmarkType.Search || type == BenchmarkType.Successor) ? int.Parse(args[6]) : 0;

            MeasureResults results = null;

            switch(type)
            {
                case BenchmarkType.Add:
                    results = Kora.Benchmarking.MeasureSeriesAdd(types, start, count, start);
                    break;
                case BenchmarkType.Delete:
                    results = Kora.Benchmarking.MeasureSeriesDelete(types, start, count, start);
                    break;
                case BenchmarkType.Search:
                    results = Kora.Benchmarking.MeasureSeriesSearch(types, start, count, start, rets);
                    break;
                case BenchmarkType.Successor:
                    results = Kora.Benchmarking.MeasureSeriesSuccessor(types, start, count, start, rets);
                    break;
                case BenchmarkType.Memory:
                    results = Kora.Benchmarking.MeasureSeriesMemory(types, start, count, start);
                    break;
            }

            bool isMemory = type == BenchmarkType.Memory;

            Tuple<long, long>[] currentResults;
            // dump rbtree
            currentResults = results.GetResults(StructureType.RBTree);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "RBTree"), isMemory);
            // dump veb
            currentResults = results.GetResults(StructureType.VEB);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "VEB"), isMemory);
            // dump dph
            currentResults = results.GetResults(StructureType.DPH);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "DPH"), isMemory);
            // dump dph
            currentResults = results.GetResults(StructureType.DPH);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "DPH"), isMemory);
            // dump xt-dph
            currentResults = results.GetResults(StructureType.XTrieDPH);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "XTrie-DPH"), isMemory);
            // dump yt-dph
            currentResults = results.GetResults(StructureType.YTrieDPH);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "YTrie-DPH"), isMemory);
            // dump xt-std
            currentResults = results.GetResults(StructureType.XTrieStandard);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "XTrie-Standard"), isMemory);
            // dump yt-std
            currentResults = results.GetResults(StructureType.YTrieStandard);
            if (currentResults != null)
                DumpResults(currentResults, Path.Combine(path, "YTrie-Standard"), isMemory);
        }

        static void DumpResults(IEnumerable<Tuple<long, long>> results, string name, bool isMemory)
        {
            using (var writer = File.CreateText(name))
            {
                foreach (var result in results)
                {
                    if (isMemory)
                        writer.WriteLine(result.Item1 + "," + result.Item2 / 1048576d);
                    else
                        writer.WriteLine(result.Item1 + "," + result.Item2);
                }
            }
        }
    }
}
