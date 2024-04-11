using BlApi;
using PL.login_window;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PL.admin_window;

public class AdminEntryWindowData : DependencyObject, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool _isProjectInProgress;

    public bool isProjectInProgress
    {
        get { return _isProjectInProgress; }
        set
        {
            _isProjectInProgress = value;
            OnPropertyChanged(nameof(isProjectInProgress));
        }
    }

    private string? _welcomeString;

    public string? welcomeString
    {
        get => _welcomeString;
        set
        {
            _welcomeString = value;
            OnPropertyChanged(nameof(welcomeString));
        }
    }

    private BO.ProjectStatus? _statusString;

    public BO.ProjectStatus? statusString
    {
        get => _statusString;
        set
        {
            _statusString = value;
            OnPropertyChanged(nameof(statusString));
        }
    }
}

/// <summary>
/// Interaction logic for AdminEntryWindow.xaml
/// </summary>
public partial class AdminEntryWindow : Window
{
    private BlApi.IBl? _bl = BlApi.Factory.Get();
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(AdminEntryWindowData), typeof(AdminEntryWindow));
    public AdminEntryWindowData Data
    {
        get => (AdminEntryWindowData)GetValue(DataDep);
        set
        {
            SetValue(DataDep, value);
            ((AdminEntryWindowData)value).OnPropertyChanged();
        }
    }

    /// <summary>
    /// constructor
    /// </summary>
    public AdminEntryWindow(int userId)
    {
        Data = new AdminEntryWindowData()
        {
            welcomeString = $"Welcome, {_bl.Engineer.Read(userId)?.Name ?? ""}!"
        };
        CheckStatus();
        InitializeComponent();
        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
    }

    /// <summary>
    /// event to double click to go engineer window.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowEngineerAndTaskList_Click(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<EngineerAndTaskList>().FirstOrDefault();
        if (window != null)
        {
            window.Activate();
            window.Focus();
        }
        else
        {
            new EngineerAndTaskList().ShowDialog();
        }
        CheckStatus();
    }

    public void CheckStatus()
    {
        Data.statusString = _bl.CheckProjectStatus();   
    }

    /// <summary>
    /// event to double click to go initialize the database.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void GanttWindow_Click(object sender, RoutedEventArgs e)
    {
        CheckStatus();
        if (Data.statusString == BO.ProjectStatus.InProgress) new GanttChartView().ShowDialog();
        else MessageBox.Show("Gantt Chart is not available in this stage of the project.");
    }


    private void Schedule_button_Click(Object sender, RoutedEventArgs e)
    {
        new SetScheduleWindow().ShowDialog();
        CheckStatus();
    }
}