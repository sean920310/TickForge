using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TickForge.Core;

BenchmarkRunner.Run<EqualityBench>();

// 沒有實作 IEquatable、只有值型別欄位的一般 struct
public struct PlainPrice
{
    public long Raw;
    public PlainPrice(long raw) => Raw = raw;
}

// 沒有實作 IEquatable、但含有參考型別欄位的一般 struct
public struct PlainWithRef
{
    public long Raw;
    public string Tag;
    public PlainWithRef(long raw, string tag) { Raw = raw; Tag = tag; }
}

[MemoryDiagnoser]
public class EqualityBench
{
    private const string Tag = "TSE";

    private readonly List<PlainPrice> _plain =
        Enumerable.Range(1, 1000).Select(i => new PlainPrice(i)).ToList();
    private readonly List<PlainWithRef> _withRef =
        Enumerable.Range(1, 1000).Select(i => new PlainWithRef(i, Tag)).ToList();
    private readonly List<Price> _record =
        Enumerable.Range(1, 1000).Select(i => new Price(i)).ToList();

    [Benchmark(Baseline = true)]
    public bool PlainStruct() => _plain.Contains(new PlainPrice(1000));

    [Benchmark]
    public bool PlainStructWithRef() => _withRef.Contains(new PlainWithRef(1000, Tag));

    [Benchmark]
    public bool RecordStruct() => _record.Contains(new Price(1000));
}