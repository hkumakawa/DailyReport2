using System;
namespace DailyReport2.Services
{
    public interface IFileDialogService { string ShowSaveDialog(string suggestedFileName); }
    public interface IConfirmationService { bool Confirm(string message, string title); }
    public interface ISettingsService { string Name { get; set; } }
    public interface IFileWriter { bool Exists(string path); void WriteAllText(string path, string content); }
    public sealed class SaveFileDialogService : IFileDialogService
    {
        public string ShowSaveDialog(string suggestedFileName) { var d = new Microsoft.Win32.SaveFileDialog { FileName = suggestedFileName, Filter = "Markdown (*.md)|*.md|すべてのファイル (*.*)|*.*", DefaultExt = ".md", AddExtension = true }; return d.ShowDialog() == true ? d.FileName : null; }
    }
    public sealed class MessageBoxConfirmationService : IConfirmationService
    {
        public bool Confirm(string message, string title) { return System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes; }
    }
    public sealed class ConfigurationSettingsService : ISettingsService
    {
        public string Name { get { return Properties.Settings.Default.Name; } set { Properties.Settings.Default.Name = value ?? string.Empty; Properties.Settings.Default.Save(); } }
    }
    public sealed class PhysicalFileWriter : IFileWriter
    {
        public bool Exists(string path) { return System.IO.File.Exists(path); }
        public void WriteAllText(string path, string content) { System.IO.File.WriteAllText(path, content, new System.Text.UTF8Encoding(false)); }
    }
}
