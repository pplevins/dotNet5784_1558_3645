using BO;
using PL.admin_window;
using PL.engineer_windows;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Shared;


/// <summary>
/// Interaction logic for EngineerList.xaml
/// </summary>
public class TaskListData : DependencyObject
{
    /// <summary>
    /// Gets or sets the DifficultyLevelSelector.
    /// </summary>

    public List<string>? DifficultyLevelSelector
    {
        get
        {
            var enumNames = Enum.GetNames(typeof(BO.EngineerExperience)).ToList();
            enumNames.Insert(0, "All");
            return enumNames;
        }
        set { SetValue(difficultyLevelSelectorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty difficultyLevelSelectorProperty =
        DependencyProperty.Register("DifficultyLevelSelector", typeof(List<string>), typeof(TaskListData));
}


/// <summary>
/// Interaction logic for TaskList.xaml
/// </summary>
public partial class TaskList : UserControl
{

    public static readonly DependencyProperty TaskItemsSourceProperty =
        DependencyProperty.Register("TaskItemsSource", typeof(ObservableCollection<TaskInList>), typeof(TaskList), new PropertyMetadata(null));

    public ObservableCollection<TaskInList> TaskItemsSource
    {
        get { return (ObservableCollection<TaskInList>)GetValue(TaskItemsSourceProperty); }
        set { SetValue(TaskItemsSourceProperty, value); }
    }
    public static readonly DependencyProperty SearchTextBoxSourceProperty =
        DependencyProperty.Register("SearchTextBoxSource", typeof(string), typeof(TaskList), new PropertyMetadata(null));


    public string SearchTextBoxSource
    {
        get { return (string)GetValue(SearchTextBoxSourceProperty); }
        set { SetValue(SearchTextBoxSourceProperty, value); }
    }

    public static readonly DependencyProperty isRelatedToEngineerSourceProperty =
        DependencyProperty.Register("isRelatedToEngineerSource", typeof(bool), typeof(TaskList), new PropertyMetadata(false));


    public bool isRelatedToEngineerSource
    {
        get { return (bool)GetValue(isRelatedToEngineerSourceProperty); }
        set { SetValue(isRelatedToEngineerSourceProperty, value); }
    }

    public static readonly DependencyProperty EngineerIdSourceProperty =
        DependencyProperty.Register("EngineerIdSource", typeof(int), typeof(TaskList), new PropertyMetadata(0));


    public int EngineerIdSource
    {
        get { return (int)GetValue(EngineerIdSourceProperty); }
        set { SetValue(EngineerIdSourceProperty, value); }
    }


    public static readonly DependencyProperty CurrentTaskVisibilitySourceProperty =
        DependencyProperty.Register(nameof(CurrentTaskVisibilitySource), typeof(Visibility), typeof(TaskList), new PropertyMetadata(default(Visibility)));

    public Visibility CurrentTaskVisibilitySource
    {
        get { return (Visibility)GetValue(CurrentTaskVisibilitySourceProperty); }
        set { SetValue(CurrentTaskVisibilitySourceProperty, value); }
    }
    public static readonly DependencyProperty TasksVisibilitySourceProperty =
        DependencyProperty.Register(nameof(TasksVisibilitySource), typeof(Visibility), typeof(TaskList), new PropertyMetadata(default(Visibility)));

    public Visibility TasksVisibilitySource
    {
        get { return (Visibility)GetValue(TasksVisibilitySourceProperty); }
        set { SetValue(TasksVisibilitySourceProperty, value); }
    }

    ///// <summary>
    ///// Gets or sets the DifficultyLevelSelector.
    ///// </summary>

    private IEnumerable<TaskInList>? originalList;
    /// <summary>
    /// access to the logical layer.
    /// </summary>
    private BlApi.IBl? _bl = BlApi.Factory.Get();

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(TaskListData), typeof(TaskList));

    /// <summary>
    /// Gets or sets the data for the AdminWindow.
    /// </summary>
    public TaskListData Data { get => (TaskListData)GetValue(DataDep); set => SetValue(DataDep, value); }

    /// <summary>
    /// constructor
    /// </summary>
    public TaskList()
    {
        originalList = _bl?.Task.ReadAllTaskInList();
        Data = new TaskListData
        {
            DifficultyLevelSelector = AddAllOptionAtStart(),
        };
        InitializeComponent();
    }


    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = SearchTextBoxSource?.ToLower();
        if (string.IsNullOrWhiteSpace(searchText))
        {
            TaskItemsSource = new ObservableCollection<TaskInList>(_bl.Task.ReadAllTaskInList());
        }
        else
        {
            TaskItemsSource = new ObservableCollection<TaskInList>(_bl.Task.ReadAllTaskInList().Where(task => task.Alias.ToLower().StartsWith(searchText) || task.Description.ToLower().StartsWith(searchText)));
        }
    }

    private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
    {
        SearchTextBoxSource = string.Empty;
        TaskItemsSource = new ObservableCollection<TaskInList>(_bl.Task.ReadAllTaskInList());
    }


    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DifficultyLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        //if (selected != null)
        //    TaskItemsSource = new ObservableCollection<BO.TaskInList>(_bl.Task.ReadAllTaskInList(item => (BO.EngineerExperience)DifficultyLevelSelector.SelectedItem == (BO.EngineerExperience)item.DifficultyLevel));
        if (selected != null)
        {
            if (Equals((string)DifficultyLevelSelector?.SelectedItem, "All")) TaskItemsSource = new ObservableCollection<BO.TaskInList>(_bl?.Task.ReadAllTaskInList());
            else TaskItemsSource = new ObservableCollection<BO.TaskInList>(_bl?.Task.ReadAllTaskInList(item => Equals((string)DifficultyLevelSelector?.SelectedItem, item.DifficultyLevel.ToString())));
        }
    }


    ///// <summary>
    ///// function to the mouse double click that send us the window with the details of the task
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var selected = ((ListView)sender).SelectedItem;

        BO.TaskInList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.TaskInList)!;

        if (item != null)
        {
            if (selected is TaskInList task)
            {
                if (IsMouseCaptureWithin)
                {
                    if (isRelatedToEngineerSource)
                    {
                        TaskInEngineer taskInEngineer = new TaskInEngineer() { Alias = task.Alias, Id = task.Id };
                        new EndOrStartEngineerTask(true, EngineerIdSource, taskInEngineer).ShowDialog();
                        OnChangeTask();
                        // Close the parent window
                        Window parentWindow = Window.GetWindow(this);
                        parentWindow?.Close();
                    }
                    else
                    {
                        new AddOrUpdateTaskWindow(_bl, task.Id).ShowDialog();
                        OnChangeTask();
                    }
                }
            }
        }
    }

    private List<string> AddAllOptionAtStart()
    {
        var enumNames = Enum.GetNames(typeof(BO.EngineerExperience)).ToList();
        enumNames.Insert(0, "All");
        return enumNames;
    }

    /// <summary>
    /// deleget for the order list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeTask()
    {
        //Data.TaskList = new ObservableCollection<Task>(_bl?.Task.ReadAll());
        TaskItemsSource = new ObservableCollection<TaskInList>(_bl?.Task.ReadAllTaskInList());
    }

    /// <summary>
    /// button to add task
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Add_Task_Button_Click(object sender, RoutedEventArgs e)
    {
        new AddOrUpdateTaskWindow(_bl).ShowDialog();
        OnChangeTask();
        //this.Close();
    }

    /// <summary>
    /// button to reset the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset_button_Click(object sender, RoutedEventArgs e)
    {
        _bl?.ResetDB();
        TaskItemsSource = null;
        Data.DifficultyLevelSelector = AddAllOptionAtStart();
    }

    private void Schedule_button_Click(Object sender, RoutedEventArgs e)
    {
        new SetScheduleWindow().ShowDialog();
    }
}
