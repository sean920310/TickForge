using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TickForge.Core;

BenchmarkRunner.Run<EqualityBench>();

// 沒有實作 IEquatable 的一般 struct
public struct PlainPrice
{
    public long Raw;
    public PlainPrice(long raw) => Raw = raw;
}

[MemoryDiagnoser]
public class EqualityBench
{
    private readonly List<PlainPrice> _plain =
        Enumerable.Range(1, 1000).Select(i => new PlainPrice(i)).ToList();
    private readonly List<Price> _record =
        Enumerable.Range(1, 1000).Select(i => new Price(i)).ToList();

    [Benchmark(Baseline = true)]
    public bool PlainStruct() => _plain.Contains(new PlainPrice(1000));

    [Benchmark]
    public bool RecordStruct() => _record.Contains(new Price(1000));
}