using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace DailyReport2.ViewModels { public abstract class ObservableObject : INotifyPropertyChanged { public event PropertyChangedEventHandler PropertyChanged; protected void Raise([CallerMemberName] string n = null) { var h = PropertyChanged; if (h != null) h(this, new PropertyChangedEventArgs(n)); } } }
