using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TickForge.Core;

namespace TickForge.Tests
{
    public class PriceTests
    {
        private static Price P(string s) =>
            Price.FromDecimal(decimal.Parse(s, CultureInfo.InvariantCulture));

        [Theory]
        [InlineData("1035.5", 10_355_000)]
        [InlineData("9.99", 99_900)]
        [InlineData("0.01", 100)]
        public void FromDecimal_converts_to_raw(string text, long expected) =>
            Assert.Equal(expected, P(text).Raw);

        [Theory]
        [InlineData("1.23456")]
        [InlineData("0")]
        [InlineData("-5")]
        public void FromDecimal_rejects_invalid_values(string text) =>
            Assert.Throws<ArgumentException>(() => P(text));

        [Fact]
        public void Prices_compare_and_equal_by_value()
        {
            Assert.True(P("100") < P("100.5"));
            Assert.Equal(P("100"), P("100.0"));
        }

        [Fact]
        public void ToString_trims_trailing_zeros() =>
            Assert.Equal("1035.5", P("1035.5").ToString());
    }

    public class TickSizeTests
    {
        [Theory]
        [InlineData("9.99", true)]
        [InlineData("10", true)]
        [InlineData("10.01", false)]
        [InlineData("10.05", true)]
        [InlineData("50.05", false)]
        [InlineData("50.1", true)]
        [InlineData("100.3", false)]
        [InlineData("100.5", true)]
        [InlineData("501", true)]
        [InlineData("1033", false)]
        [InlineData("1035", true)]
        public void IsValid_follows_tick_table(string text, bool expected) =>
            Assert.Equal(expected,
                TickSize.IsValid(Price.FromDecimal(decimal.Parse(text, CultureInfo.InvariantCulture))));
    }
}
