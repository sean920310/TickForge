## 2026-11-26 struct 相等比較

- 環境：.NET 11.0.0、BenchmarkDotNet 0.16.0-preview.2（0.15.8 不支援 net11.0）、i5-12600K
- 假設：一般 struct 在 List.Contains 會 boxing，比 record struct 慢且有配置
- 結果：PlainStruct 100.9 ns / 0 B；RecordStruct 285.8 ns / 0 B
- 解釋：假設不成立。推測一般 struct 走了位元比對加 SIMD 的捷徑；record struct 有自訂 Equals，只能逐一呼叫
- 追加實驗：含 string 欄位的一般 struct → 35,925 ns、152,000 B/op、Gen0 14.5，比 PlainStruct 慢約 360 倍
- 結論：沒有 IEquatable、又含參考欄位的 struct 會走 ValueType.Equals（裝箱 + 反射）。放進集合的 struct 一律實作 IEquatable<T> 或使用 record struct
- 追加 2×2：RecordStructWithRef 386 ns / 0 B（RecordStruct 298 ns）。record struct 有型別專用的 Equals，含參考欄位也不會裝箱
- 條件：Tag 為同一個字串實例，字串比較走參照相等的捷徑；不同實例時成本會更高
- 雜訊：PlainStructWithRef 兩次執行分別為 35,925 / 29,117 ns，跨次差異約 20%，只比較同一次執行內的數字
