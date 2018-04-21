using System.Collections.Generic;

namespace TheHeartbeat.Utilities
{
    public class FSMTransition<TS>
    {
        private readonly HashSet<TS> _from;
        private readonly TS _to;

        public FSMTransition(HashSet<TS> from, TS to)
        {
            _from = from;
            _to = to;
        }

        public bool IsAvailable(TS from)
        {
            return _from.Contains(from);
        }

        public TS To()
        {
            return _to;
        }
    }
}
