using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Globalization;
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

public record struct RecordWithRef
{
    public long Raw;
    public string Tag;
    public RecordWithRef(long raw, string tag) { Raw = raw; Tag = tag; }
}

[MemoryDiagnoser]
public class EqualityBench
{
    private const string Tag = "TSE";

    private readonly List<PlainPrice> _plain =
        Enumerable.Range(1, 1000).Select(i => new PlainPrice(i)).ToList();
    private readonly List<PlainWithRef> _withRef =
        Enumerable.Range(1, 1000).Select(i => new PlainWithRef(i, Tag)).ToList();
    private readonly List<RecordWithRef> _recoredWithRef =
        Enumerable.Range(1, 1000).Select(i => new RecordWithRef(i, Tag)).ToList();
    private readonly List<Price> _record =
        Enumerable.Range(1, 1000).Select(i => new Price(i)).ToList();

    [Benchmark(Baseline = true)]
    public bool PlainStruct() => _plain.Contains(new PlainPrice(1000));

    [Benchmark]
    public bool PlainStructWithRef() => _withRef.Contains(new PlainWithRef(1000, Tag));

    [Benchmark]
    public bool RecordStruct() => _record.Contains(new Price(1000));

    [Benchmark]
    public bool RecordStructWithRef() => _recoredWithRef.Contains(new RecordWithRef(1000, Tag));
}