using System;
using System.IO;
using System.Linq;
namespace DailyReport2.Services
{
    /// <summary>OSで使用できる安全な日報ファイル名を生成します。</summary>
    public static class FileNameSanitizer
    {
        public static string Sanitize(string name, DateTime date)
        {
            var s = string.IsNullOrWhiteSpace(name) ? "日報" : name.Trim();
            foreach (var c in Path.GetInvalidFileNameChars()) s = s.Replace(c.ToString(), "_");
            return s + "_" + date.ToString("yyyyMMdd") + ".md";
        }
    }
}
