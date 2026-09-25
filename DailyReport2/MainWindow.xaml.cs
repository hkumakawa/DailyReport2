using System.Windows;
using DailyReport2.Services;
using DailyReport2.ViewModels;
namespace DailyReport2 { public partial class MainWindow : Window { public MainWindow() { InitializeComponent(); DataContext = new MainViewModel(new SaveFileDialogService(), new MessageBoxConfirmationService(), new ConfigurationSettingsService(), new PhysicalFileWriter()); } } }
