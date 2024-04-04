using BO;
using PL.engineer_windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace PL.engineer_main_windows;

/// <summary>
/// Interaction logic for EngineerTrackingWindow.xaml
/// </summary>
public partial class EngineerTrackingWindow : Window, INotifyPropertyChanged
{
    private BlApi.IBl? _bl = BlApi.Factory.Get();
    private TaskInEngineer? _taskInEngineer;
    public event PropertyChangedEventHandler? PropertyChanged;

    public BO.TaskInEngineer? Task
    {
        get { return (BO.TaskInEngineer?)GetValue(taskProperty); }
        set { SetValue(taskProperty, value); }
    }


    public static readonly DependencyProperty taskProperty =
        DependencyProperty.Register("Task", typeof(BO.TaskInEngineer), typeof(EngineerTrackingWindow));


    /// <summary>
    /// Gets or sets the list of tasks.
    /// </summary>
    public ObservableCollection<BO.TaskInList?>? TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>?)GetValue(tasksListProperty); }
        set { SetValue(tasksListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty tasksListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList?>), typeof(EngineerTrackingWindow));


    public static readonly DependencyProperty TasksVisibilityProperty =
        DependencyProperty.Register(nameof(TasksVisibility), typeof(Visibility), typeof(EngineerTrackingWindow), new PropertyMetadata(default(Visibility)));

    public Visibility TasksVisibility
    {
        get { return (Visibility)GetValue(TasksVisibilityProperty); }
        set { SetValue(TasksVisibilityProperty, value); }
    }

    public static readonly DependencyProperty SearchTextBoxProperty =
        DependencyProperty.Register("SearchTextBox", typeof(string), typeof(EngineerTrackingWindow), new PropertyMetadata(null));


    public string SearchTextBox
    {
        get { return (string)GetValue(SearchTextBoxProperty); }
        set { SetValue(SearchTextBoxProperty, value); }
    }
    public static readonly DependencyProperty EngineerIdProperty =
        DependencyProperty.Register("EngineerId", typeof(int), typeof(EngineerTrackingWindow), new PropertyMetadata(default(int)));


    public int EngineerId
    {
        get { return (int)GetValue(EngineerIdProperty); }
        set { SetValue(EngineerIdProperty, value); }
    }

    public EngineerTrackingWindow(int engineerId)
    {
        EngineerId = engineerId;
        var engineer = _bl.Engineer.Read(engineerId);
        _taskInEngineer = engineer.Task;
        checkTaskExistence();
        TaskList = new ObservableCollection<TaskInList>(_bl?.Task?.GetSuitableTasks(EngineerId));
        InitializeComponent();
    }

    private void checkTaskExistence()
    {
        var engineer = _bl.Engineer.Read(EngineerId);
        if (engineer?.Task != null)
        {
            TasksVisibility = Visibility.Collapsed;
        }
        else
        {
            TasksVisibility = Visibility.Visible;
        }
    }
    /// <summary>
    /// function that if we put id of order in the text box we got the status on him.
    /// aotomticly with the property change
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void GetTasks_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            checkTaskExistence();
            if (TasksVisibility.Equals(Visibility.Collapsed))
            {
                MessageBox.Show("You already have an associated task for you so you cant pick another one from the list");
            }

        }
        catch (BO.Exceptions.BlDoesNotExistException ex) when (ex.InnerException is not null)
        {
            MessageBox.Show(ex.Message + ex.InnerException!.Message);
        }
    }
    /// <summary>
    /// function to get the order details .
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void GetCurrentTask_Click(object sender, RoutedEventArgs e)
    {
        checkTaskExistence();
        if (TasksVisibility.Equals(Visibility.Visible))
        {
            MessageBox.Show("There is no associated task for you, please pick one from the list");
        }
        else OpenEndOrStartEngineerTaskWindow();
    }

    private void OpenEndOrStartEngineerTaskWindow()
    {
        var window = Application.Current.Windows.OfType<EndOrStartEngineerTask>().FirstOrDefault();
        if (window != null)
        {
            window.Activate();
            window.Focus();
        }
        else
        {
            new EndOrStartEngineerTask(false, EngineerId, _taskInEngineer).ShowDialog();
        }
    }
}
