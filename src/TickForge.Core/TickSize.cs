using System;
using System.Collections.Generic;
using System.Text;

namespace TickForge.Core
{
    /// <summary>台股一般股票的升降單位。</summary>
    public static class TickSize
    {
        /// <summary>回傳此價格所在區間的升降單位（Raw 單位）。</summary>
        public static long Of(Price price)
        {
            // 用 Raw 直接比較區間邊界，例如
            // 未滿 10 元（Raw < 100_000）→ 0.01 元 = 100
            // 注意：剛好 10 元屬於「10 – 未滿 50」那一檔
            if(price.Raw< 100_000)
                return 100;
            if (price.Raw < 500_000)
                return 500;
            if (price.Raw < 1_000_000)
                return 1_000;
            if (price.Raw < 5_000_000)
                return 5_000;
            if (price.Raw < 10_000_000)
                return 10_000;
            
            return 50_000;
        }

        public static bool IsValid(Price price) =>
            price.Raw > 0 && price.Raw % Of(price) == 0;
    }
}
