using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UAM.Kora
{
    internal static class BitHacks
    {
        private static uint[] BitTable = { 0xAAAAAAAA, 0xCCCCCCCC, 0xF0F0F0F0, 0xFF00FF00, 0xFFFF0000 };

        internal static uint Log2Ceiling(uint v)
        {
           v = RoundToPower(v);
            // v is now rounded to the next power of 2
            int r = (v & BitTable[0]) == 0 ? 0 : 1;
            r |= ((v & BitTable[4]) == 0 ? 0 : 1) << 4;
            r |= ((v & BitTable[3]) == 0 ? 0 : 1) << 3;
            r |= ((v & BitTable[2]) == 0 ? 0 : 1) << 2;
            r |= ((v & BitTable[1]) == 0 ? 0 : 1) << 1;
            return (uint)r;
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
