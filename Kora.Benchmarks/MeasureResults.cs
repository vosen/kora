using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAM.Kora
{
    public class MeasureResults
    {
        public StructureType Types { get; private set; }
        public int MaxValue { get; private set; }
        private Tuple<long, long>[][] results = new Tuple<long, long>[7][];

        public MeasureResults(StructureType type, int maxval)
        {
            Types = type;
            MaxValue = maxval;
        }

        public Tuple<long, long>[] GetResults(StructureType type)
        {
            return results[BitHacks.Power2MSB((uint)type)];
        }

        public void SetResults(StructureType type, Tuple<long, long>[] res)
        {
            if((Types & type) > 0)
                results[BitHacks.Power2MSB((uint)type)] = res;
        }
    }
}
