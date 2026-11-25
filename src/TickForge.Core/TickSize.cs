using System;

namespace TickForge.Core
{
    /// <summary>台股一般股票的升降單位。</summary>
    public static class TickSize
    {
        private static readonly (long UpperExclusive, long Tick)[] Table =
        {
            (100_000, 100),          // 未滿 10 元：0.01
            (500_000, 500),          // 10 – 未滿 50：0.05
            (1_000_000, 1_000),      // 50 – 未滿 100：0.1
            (5_000_000, 5_000),      // 100 – 未滿 500：0.5
            (10_000_000, 10_000),    // 500 – 未滿 1000：1
            (long.MaxValue, 50_000), // 1000 以上：5
        };

        /// <summary>回傳此價格所在區間的升降單位（Raw 單位）。</summary>
        public static long Of(Price price)
        {
            foreach (var (upper, tick) in Table)
            {
                if (price.Raw < upper)
                    return tick;
            }
            return Table[^1].Tick;
        }

        public static bool IsValid(Price price) =>
            price.Raw > 0 && price.Raw % Of(price) == 0;
    }
}
