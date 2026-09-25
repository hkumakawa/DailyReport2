using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using DailyReport2.Models;
using DailyReport2.Services;
namespace DailyReport2.ViewModels
{
    /// <summary>メイン画面の状態と保存処理を管理します。</summary>
    public sealed class MainViewModel : ObservableObject
    {
        private readonly IFileDialogService dialogs; private readonly IConfirmationService confirm; private readonly ISettingsService settings; private readonly IFileWriter writer; private readonly MarkdownReportWriter markdown;
        private string name; private DateTime date;
        public string Name { get { return name; } set { name = value ?? ""; settings.Name = name; Raise(); } }
        public DateTime Date { get { return date; } set { date = value; Raise(); } }
        public ObservableCollection<TaskItem> RegularTasks { get; private set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> SkillUpActivities { get; private set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<Deliverable> Deliverables { get; private set; } = new ObservableCollection<Deliverable>();
        public ICommand AddRegularCommand { get; private set; } public ICommand RemoveRegularCommand { get; private set; } public ICommand AddSkillCommand { get; private set; } public ICommand RemoveSkillCommand { get; private set; } public ICommand AddDeliverableCommand { get; private set; } public ICommand RemoveDeliverableCommand { get; private set; } public ICommand SaveCommand { get; private set; }
        public string ErrorMessage { get; private set; }
        public MainViewModel(IFileDialogService d, IConfirmationService c, ISettingsService s, IFileWriter w) { dialogs=d; confirm=c; settings=s; writer=w; markdown=new MarkdownReportWriter(); Name=settings.Name; Date=DateTime.Today; RegularTasks.Add(new TaskItem { Content="朝礼／夕礼" }); SkillUpActivities.Add(new TaskItem()); Deliverables.Add(new Deliverable()); AddRegularCommand=new ActionCommand(() => RegularTasks.Add(new TaskItem())); RemoveRegularCommand=new ActionCommand(() => RemoveTask(RegularTasks)); AddSkillCommand=new ActionCommand(() => SkillUpActivities.Add(new TaskItem())); RemoveSkillCommand=new ActionCommand(() => RemoveTask(SkillUpActivities)); AddDeliverableCommand=new ActionCommand(() => Deliverables.Add(new Deliverable())); RemoveDeliverableCommand=new ActionCommand(() => { if(Deliverables.Count > 0) Deliverables.RemoveAt(Deliverables.Count-1); }); SaveCommand=new ActionCommand(Save); }
        private void RemoveTask(ObservableCollection<TaskItem> list) { if(list.Count > 0) list.RemoveAt(list.Count-1); }
        public void Save()
        {
            ErrorMessage = Validate(); Raise("ErrorMessage"); if (!string.IsNullOrEmpty(ErrorMessage)) return;
            var path=dialogs.ShowSaveDialog(FileNameSanitizer.Sanitize(Name, Date)); if(string.IsNullOrEmpty(path)) return;
            if(writer.Exists(path) && !confirm.Confirm("既存ファイルを上書きしますか？", "確認")) return;
            try
            {
                writer.WriteAllText(path, markdown.Render(new DailyReport { Name=Name, Date=Date, RegularTasks=RegularTasks, SkillUpActivities=SkillUpActivities, Deliverables=Deliverables }));
            }
            catch (IOException ex)
            {
                SetSaveError(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                SetSaveError(ex);
            }
            catch (InvalidOperationException ex)
            {
                SetSaveError(ex);
            }
        }
        private void SetSaveError(Exception ex) { ErrorMessage = "保存に失敗しました: " + ex.Message; Raise("ErrorMessage"); }
        public string Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) return "名前を入力してください。";
            if (Date == DateTime.MinValue) return "日付を入力してください。";
            if (!RegularTasks.Any(x => x != null && !string.IsNullOrWhiteSpace(x.Content)) &&
                !SkillUpActivities.Any(x => x != null && !string.IsNullOrWhiteSpace(x.Content)))
                return "定例作業またはスキルアップ活動を1件以上入力してください。";
            foreach (var d in Deliverables.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(d.Name) && !string.IsNullOrWhiteSpace(d.Link)) return "成果物名を入力してください。";
                if (!string.IsNullOrWhiteSpace(d.Name) && !string.IsNullOrWhiteSpace(d.Link) && !Regex.IsMatch(d.Link.Trim(), @"^https?://[^\s]+$", RegexOptions.IgnoreCase)) return "成果物のリンクはhttp/httpsの絶対URLで入力してください。";
            }
            return null;
        }
    }
    public sealed class ActionCommand : ICommand { private readonly Action action; public ActionCommand(Action a) { action=a; } public bool CanExecute(object p) { return true; } public void Execute(object p) { action(); } public event EventHandler CanExecuteChanged { add {} remove {} } }
}
