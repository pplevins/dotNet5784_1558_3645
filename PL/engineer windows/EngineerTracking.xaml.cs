using BO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Task = BO.Task;

namespace PL.engineer_main_windows;

/// <summary>
/// Interaction logic for EngineerTrackingWindow.xaml
/// </summary>
public partial class EngineerTrackingWindow : Window, INotifyPropertyChanged
{
    private BlApi.IBl? _bl = BlApi.Factory.Get();
    private int _engineerId;
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
    public ObservableCollection<BO.Task?>? TaskList
    {
        get { return (ObservableCollection<BO.Task>?)GetValue(tasksListProperty); }
        set { SetValue(tasksListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty tasksListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.Task?>), typeof(EngineerTrackingWindow));


    public static readonly DependencyProperty CurrentTaskVisibilityProperty =
        DependencyProperty.Register(nameof(CurrentTaskVisibility), typeof(Visibility), typeof(EngineerTrackingWindow), new PropertyMetadata(default(Visibility)));

    public Visibility CurrentTaskVisibility
    {
        get { return (Visibility)GetValue(CurrentTaskVisibilityProperty); }
        set { SetValue(CurrentTaskVisibilityProperty, value); }
    }
    public static readonly DependencyProperty TasksVisibilityProperty =
        DependencyProperty.Register(nameof(TasksVisibility), typeof(Visibility), typeof(EngineerTrackingWindow), new PropertyMetadata(default(Visibility)));

    public Visibility TasksVisibility
    {
        get { return (Visibility)GetValue(TasksVisibilityProperty); }
        set { SetValue(TasksVisibilityProperty, value); }
    }
    /// <summary>
    /// we creat a new property change name.
    /// </summary>
    private string taskTrackingString_e;
    public string taskTrackingString { get { return taskTrackingString_e; } set { taskTrackingString_e = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("taskTrackingString")); } } }
    /// <summary>
    ///  we creat a new property change name.
    /// </summary>
    private int engineerId_e;
    public int engineerId { get { return engineerId_e; } set { engineerId_e = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("engineerId")); } } }
    /// <summary>
    /// constractor
    /// </summary>
    public EngineerTrackingWindow(int engineerId)
    {
        _engineerId = engineerId;
        TasksVisibility = Visibility.Collapsed;
        CurrentTaskVisibility = Visibility.Collapsed;
        TaskList = new ObservableCollection<Task>(_bl?.Task?.ReadAll());
        InitializeComponent();
    }
    /// <summary>
    /// function that if we put id of order in the text box we got the status on him.
    /// aotomticly with the property change
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void GetDetails_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            TasksVisibility = Visibility.Collapsed;
            CurrentTaskVisibility = Visibility.Visible;
            //task = _bl?.Engineer.Read(_engineerId)?.Task;
            //taskTrackingString = task?.ToString()!;
            Task = new TaskInEngineer()
            {
                Alias = "dshsdh",
                Id = 42
            };
        }
        catch (BO.Exceptions.BlDoesNotExistException ex) when (ex.InnerException is not null)
        {
            MessageBox.Show(ex.Message + ex.InnerException!.Message);
        }
    }
    /// <summary>
    /// function to remove the option to input latters in the text box of id.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ID_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = Regex.IsMatch(e.Text, "^[^0-9]+$");
    }
    /// <summary>
    /// function to get the order details .
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void GetItems_Click(object sender, RoutedEventArgs e)
    {
        TasksVisibility = Visibility.Visible;
        CurrentTaskVisibility = Visibility.Collapsed;
        //try
        //{
        //    new TaskDatails(_bl, engineerId).Show();

        //}
        //catch (BO.IdNotExsitException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (BO.NullExeptionForDO ex) when (ex.InnerException is not null)
        //{
        //    MessageBox.Show(ex.Message + ex.InnerException!.Message);
        //}
    }
}
