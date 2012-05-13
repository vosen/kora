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
            int width = (int)BitHacks.Log2Ceiling(m);
            var fks = GetRandomHashMethodFKS(m);
            var nomod1 = GetRandomHashMethod1(m);
            var nomod2 = GetRandomHashMethod2(width);
            var randoms = Enumerable.Range(0, 100000000).Select((i) => (uint)random.Next((int)m)).ToArray();
            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < randoms.Length; i++)
                fks(randoms[i]);
            watch.Stop();
            Console.WriteLine("fks: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                nomod1(randoms[i]);
            watch.Stop();
            Console.WriteLine("nomod by size: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                nomod2(randoms[i]);
            watch.Stop();
            Console.WriteLine("nomod by width: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                UnrolledHash1(a, b, width, randoms[i]);
            watch.Stop();
            Console.WriteLine("unrolled conditional: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                UnrolledHash2(a, b, width, randoms[i]);
            watch.Stop();
            Console.WriteLine("unrolled shift by width: {0}", watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < randoms.Length; i++)
                UnrolledHash3(a, b, m, randoms[i]);
            watch.Stop();
            Console.WriteLine("unrolled shift by size: {0}", watch.ElapsedMilliseconds);
        }

        internal static Func<uint, uint> GetRandomHashMethodFKS(uint size)
        {
            uint a = (uint)random.Next(1, int.MaxValue);
            uint b = (uint)(random.Next());
            return (x) => ((a * x + b) % 31) % size;
        }

        internal static Func<uint, uint> GetRandomHashMethod1(uint size)
        {
            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 31 - (int)BitHacks.Log2Ceiling(size);
            return (x) => ((a * x + b) >> shift) >> 1;
        }

        internal static Func<uint, uint> GetRandomHashMethod2(int width)
        {
            uint a = (uint)random.Next();
            uint b = (uint)(random.Next(65536) << 16);
            int shift = 31 - (int)width;
            return (x) => ((a * x + b) >> shift) >> 1;
        }

        internal static uint UnrolledHash1(uint a, uint b, int width, uint x)
        {
            return width == 0 ? 0 : ((a * x + b) >> (31 - (int)width)) >> 1;
        }

        internal static uint UnrolledHash2(uint a, uint b, int width, uint x)
        {
            return ((a * x + b) >> (31 - (int)width)) >> 1;
        }

        internal static uint UnrolledHash3(uint a, uint b, uint size, uint x)
        {
            return ((a * x + b) >> (31 - (int)BitHacks.Log2Ceiling(size))) >> 1;
        }
    }
}
