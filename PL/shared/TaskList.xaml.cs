using BlApi;
using BO;
using PL.admin_window;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Task = BO.Task;

namespace PL.Shared;


/// <summary>
/// Interaction logic for EngineerList.xaml
/// </summary>
public class TaskListData : DependencyObject
{
    //public static readonly DependencyProperty TaskItemsSourceProperty =
    //    DependencyProperty.Register("TaskItemsSource", typeof(ObservableCollection<TaskInList>), typeof(TaskListData), new PropertyMetadata(null));

    //public ObservableCollection<TaskInList> TaskItemsSource
    //{
    //    get { return (ObservableCollection<TaskInList>)GetValue(TaskItemsSourceProperty); }
    //    set { SetValue(TaskItemsSourceProperty, value); }
    //}


    /// <summary>
    /// Gets or sets the DifficultyLevelSelector.
    /// </summary>
    public Array? DifficultyLevelSelector { get; set; }
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


    ///// <summary>
    ///// Gets or sets the DifficultyLevelSelector.
    ///// </summary>
    //public Array? DifficultyLevelSelector { get; set; }

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
    //    originalList = _bl?.Task.ReadAllTaskInList();
    //    TaskItemsSource = new ObservableCollection<TaskInList>(originalList)!;
    //    DifficultyLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience));
    //    InitializeComponent();

        originalList = _bl?.Task.ReadAllTaskInList();
        Data = new TaskListData
        {
            DifficultyLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience))
        };
        //TaskItemsSource = new ObservableCollection<TaskInList>(originalList)!;
        InitializeComponent();
    }

    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DifficultyLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if (selected != null)
            TaskItemsSource = new ObservableCollection<BO.TaskInList>(_bl.Task.ReadAllTaskInList(item => (BO.EngineerExperience)DifficultyLevelSelector.SelectedItem == (BO.EngineerExperience)item.DifficultyLevel));
    }


    ///// <summary>
    ///// function to the mouse double click that send us the window with the details of the task
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //var test = BlInstance?.Engineer?.ReadAll();
        //if (IsMouseCaptureWithin)
        //    new TaskDatails(_bl, ((BO.Task)TaskListView.SelectedItem).Id, OnChangeTask).ShowDialog();
        var selected = ((ListView)sender).SelectedItem;

        BO.TaskInList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.TaskInList)!;

        if (item != null)
        {
            if (selected is TaskInList task)
            {
                if (IsMouseCaptureWithin)
                {
                    new AddOrUpdateTaskWindow(_bl, task.Id).ShowDialog();
                    OnChangeTask();
                    //this.Close();
                }
            }
        }
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
        Data.DifficultyLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience));
    }
}
