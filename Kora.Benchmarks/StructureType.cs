using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    [Flags]
    public enum StructureType
    {
        Unknown,
        RBTree = 1 << 0,
        VEB = 1 << 1,
        DPH = 1 << 2,
        XTrieDPH = 1 << 3,
        YTrieDPH = 1 << 4,
        XTrieStandard = 1 << 5,
        YTrieStandard = 1 << 6
    }
}
