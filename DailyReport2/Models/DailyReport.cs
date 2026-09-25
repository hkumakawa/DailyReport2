using System;
using System.Collections.ObjectModel;
namespace DailyReport2.Models
{
    /// <summary>日報の入力内容を表します。</summary>
    public class DailyReport
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<TaskItem> RegularTasks { get; set; }
        public ObservableCollection<TaskItem> SkillUpActivities { get; set; }
        public ObservableCollection<Deliverable> Deliverables { get; set; }
    }
}
