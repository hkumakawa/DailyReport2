using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyReport2.Services;
using DailyReport2.Models;
using System;
namespace DailyReport2.UnitTest
{
    [TestClass] public class MarkdownReportWriterUnitTest
    {
        [TestMethod] public void Render_特殊文字_エスケープされる() { var r=new DailyReport {Name="A*_[x]",Date=new DateTime(2024,1,1)}; var text=new MarkdownReportWriter().Render(r); Assert.IsTrue(text.Contains(@"A\*\_\[x\]")); }
        [TestMethod] public void Render_空コレクション_各セクションになし表示される() { var text=new MarkdownReportWriter().Render(new DailyReport()); Assert.IsTrue(text.Contains("## 定例作業\r\n\r\n- なし")); Assert.IsTrue(text.Contains("## スキルアップ活動\r\n\r\n- なし")); Assert.IsTrue(text.Contains("## 成果物\r\n\r\n- なし")); }
        [TestMethod] public void Render_成果物が名前のみ_名前だけ出力される() { var r=new DailyReport { Deliverables=new System.Collections.ObjectModel.ObservableCollection<Deliverable> { new Deliverable { Name="仕様書" }, new Deliverable { Link="https://example.com" } } }; var text=new MarkdownReportWriter().Render(r); Assert.IsTrue(text.Contains("- 仕様書")); Assert.IsFalse(text.Contains("example.com")); }
        [TestMethod] public void Sanitize_日本語と不正文字_名前日付順で安全化される() { var n=FileNameSanitizer.Sanitize("報告:太郎",new DateTime(2024,1,2)); Assert.AreEqual("報告_太郎_20240102.md",n); }
    }
}
