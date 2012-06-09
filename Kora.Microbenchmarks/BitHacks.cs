using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UAM.Kora.Microbenchmarks
{
    internal static class BitHacks
    {
        private static uint[] BitTable = { 0xAAAAAAAA, 0xCCCCCCCC, 0xF0F0F0F0, 0xFF00FF00, 0xFFFF0000 };
        private static int[] MultiplyDeBruijnBitPosition2 = { 0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8, 31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9 };

        internal static int Log2Ceiling(uint v)
        {
            return MultiplyDeBruijnBitPosition2[(v * 0x077CB531U) >> 27];
        }

        internal static uint RoundToPower(uint v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            return ++v;
        }
    }
}
