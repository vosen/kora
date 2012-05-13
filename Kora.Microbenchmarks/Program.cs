using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kora.Microbenchmarks
{


    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            uint m = BitHacks.RoundToPower((uint)random.Next(1000, int.MaxValue/2));
            var fks = GetRandomHashMethodFKS(m);
            var nomod = GetRandomHashMethod(m);
            var randoms = Enumerable.Range(0, 100000000).Select((i) => (uint)random.Next((int)m)).ToArray();
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < randoms.Length; i++)
                fks(randoms[i]);
            watch.Stop();
            Console.WriteLine("fks: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                nomod(randoms[i]);
            watch.Stop();
            Console.WriteLine("nomod: {0}", watch.ElapsedMilliseconds);
            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                UnrolledHash1(a,b, (int)m, randoms[i]);
            watch.Stop();
            Console.WriteLine("unrolled1: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                UnrolledHash2(a, b, (int)m, randoms[i]);
            watch.Stop();
            Console.WriteLine("unrolled2: {0}", watch.ElapsedMilliseconds);
        }

        internal static Func<uint, uint> GetRandomHashMethodFKS(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));

            uint a = (uint)random.Next(1, int.MaxValue);
            uint b = (uint)(random.Next());
            return (x) => ((a * x + b) % 31) % size;
        }

        internal static Func<uint, uint> GetRandomHashMethod(uint size)
        {
            System.Diagnostics.Debug.Assert(size == BitHacks.RoundToPower(size));

            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 32 - (int)size;
            return (x) => (a * x + b) >> shift;
        }

        internal static uint UnrolledHash1(uint a, uint b, int size, uint x)
        {
            return size == 0 ? 0 : (a * x + b) >> (32 - size);
        }

        internal static uint UnrolledHash2(uint a, uint b, int size, uint x)
        {
            return ((a * x + b) >> (33 - size)) >> 1;
        }
    }
}
