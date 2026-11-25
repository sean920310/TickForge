## 2026-11-26 struct 相等比較

- 環境：.NET 11.0.0、BenchmarkDotNet 0.16.0-preview.2（0.15.8 不支援 net11.0）、i5-12600K
- 假設：一般 struct 在 List.Contains 會 boxing，比 record struct 慢且有配置
- 結果：PlainStruct 100.9 ns / 0 B；RecordStruct 285.8 ns / 0 B
- 解釋：假設不成立。推測一般 struct 走了位元比對加 SIMD 的捷徑；record struct 有自訂 Equals，只能逐一呼叫
- 待驗證：加入參考型別欄位的 struct 是否會出現 boxing