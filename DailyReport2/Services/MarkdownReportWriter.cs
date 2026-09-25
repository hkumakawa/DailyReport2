using System;
using System.Linq;
using System.Text;
using DailyReport2.Models;
namespace DailyReport2.Services
{
    /// <summary>日報をUTF-8 BOMなしMarkdownへ変換します。</summary>
    public sealed class MarkdownReportWriter
    {
        public string Render(DailyReport report)
        {
            if (report == null) throw new ArgumentNullException("report");
            var b = new StringBuilder();
            b.AppendLine("# 日報"); b.AppendLine(); b.AppendLine("- **名前**: " + Escape(report.Name)); b.AppendLine("- **日付**: " + report.Date.ToString("yyyy年MM月dd日")); b.AppendLine();
            Section(b, "定例作業", report.RegularTasks == null ? null : report.RegularTasks.Select(x => x.Content));
            Section(b, "スキルアップ活動", report.SkillUpActivities == null ? null : report.SkillUpActivities.Select(x => x.Content));
            b.AppendLine("## 成果物"); b.AppendLine();
            var deliverables = (report.Deliverables ?? new System.Collections.ObjectModel.ObservableCollection<Deliverable>())
                .Where(x => x != null && !string.IsNullOrWhiteSpace(x.Name)).ToList();
            if (deliverables.Count == 0) b.AppendLine("- なし");
            foreach (var d in deliverables)
                b.AppendLine("- " + Escape(d.Name) + (string.IsNullOrWhiteSpace(d.Link) ? "" : " ([リンク](" + EscapeUrl(d.Link) + "))"));
            return b.ToString();
        }
        private static void Section(StringBuilder b, string title, System.Collections.Generic.IEnumerable<string> values)
        {
            b.AppendLine("## " + title); b.AppendLine();
            var items = values == null ? new System.Collections.Generic.List<string>() : values.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (items.Count == 0) b.AppendLine("- なし");
            foreach (var v in items) b.AppendLine("- " + Escape(v));
            b.AppendLine();
        }
        public static string Escape(string value) { return (value ?? "").Replace("\\", "\\\\").Replace("`", "\\`").Replace("*", "\\*").Replace("_", "\\_").Replace("[", "\\[").Replace("]", "\\]").Replace("#", "\\#"); }
        public static string EscapeUrl(string value) { return (value ?? "").Replace(")", "%29").Replace("(", "%28").Replace(" ", "%20"); }
    }
}
