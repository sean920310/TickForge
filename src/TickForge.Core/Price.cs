using System;
using System.Globalization;

namespace TickForge.Core
{
    /// <summary>價格，以「元 × 10,000」的整數儲存。</summary>
    public readonly record struct Price(long Raw) : IComparable<Price>
    {
        public const long Scale = 10_000;

        public static Price FromDecimal(decimal value)
        {
            decimal scaled = value * Scale;
            
            if (!decimal.IsInteger(scaled) || scaled <= 0)
            {
                throw new ArgumentException($"價格必須為正數且最多四位小數：{value}", nameof(value));
            }
            return new Price((long)scaled); 
        }

        public decimal ToDecimal() => (decimal)Raw / Scale;

        public int CompareTo(Price other) => Raw.CompareTo(other.Raw);

        public static bool operator <(Price a, Price b) => a.Raw < b.Raw;
        public static bool operator >(Price a, Price b) => a.Raw > b.Raw;
        public static bool operator <=(Price a, Price b) => a.Raw <= b.Raw;
        public static bool operator >=(Price a, Price b) => a.Raw >= b.Raw;

        public override string ToString() =>
            ToDecimal().ToString("0.####", CultureInfo.InvariantCulture);
    }
}
