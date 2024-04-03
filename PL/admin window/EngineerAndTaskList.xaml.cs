using BO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;
/// <summary>
/// Interaction logic for EngineerList.xaml
/// </summary>
public class EngineerAndTaskListData : DependencyObject
{

    /// <summary>
    /// Gets or sets the engineers.
    /// </summary>
    public IEnumerable<Engineer>? EngineerList
    {
        get { return (IEnumerable<Engineer>?)GetValue(engineerProperty); }
        set { SetValue(engineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty engineerProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<Engineer>), typeof(EngineerAndTaskListData));

    /// <summary>
    /// Gets or sets the list of tasks.
    /// </summary>
    public IEnumerable<BO.TaskInList?>? TaskList
    {
        get { return (List<BO.TaskInList>?)GetValue(tasksListProperty); }
        set { SetValue(tasksListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty tasksListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList?>), typeof(EngineerAndTaskListData));


    /// <summary>
    /// Gets or sets the EngineerLevelSelector.
    /// </summary>
    public Array? EngineerLevelSelector { get; set; }
}
public partial class EngineerAndTaskList : Window
{
    /// <summary>
    /// access to the logical layer.
    /// </summary>
    private BlApi.IBl? _bl;

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(EngineerAndTaskListData), typeof(EngineerAndTaskList));

    /// <summary>
    /// Gets or sets the data for the AdminWindow.
    /// </summary>
    public EngineerAndTaskListData Data { get => (EngineerAndTaskListData)GetValue(DataDep); set => SetValue(DataDep, value); }

    /// <summary>
    /// constructor
    /// </summary>
    public EngineerAndTaskList(BlApi.IBl? _bl)
    {
        this._bl = _bl;
        InitializeComponent();
        Data = new()
        {
            EngineerList = _bl?.Engineer.ReadAll()!,
            TaskList = _bl?.Task.ReadAllTaskInList(),
            EngineerLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience))
        };
    }

    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if (selected != null) Data.EngineerList = Data.EngineerList!.Where(item => (BO.EngineerExperience)EngineerLevelSelector.SelectedItem == item?.Level); ;
    }
    /// <summary>
    /// button to reset the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset_button_Click(object sender, RoutedEventArgs e)
    {
        _bl?.ResetDB();

        Data.EngineerList = null;
        Data.TaskList = null;
        Data.EngineerLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience));
    }
    /// <summary>
    /// button to add engineer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Add_Engineer_Button_Click(object sender, RoutedEventArgs e)
    {
        new AddOrUpdateEngineerWindow(_bl).ShowDialog();
        OnChangeEngineer();
        //this.Close();
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
    /// event double click to open the new window of update engineer with the id that chooce.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var selected = ((ListView)sender).SelectedItem;

        BO.Engineer item = (((FrameworkElement)e.OriginalSource).DataContext as BO.Engineer)!;

        if (item != null)
        {
            if (selected is Engineer engineer)
            {
                if (IsMouseCaptureWithin)
                {
                    new AddOrUpdateEngineerWindow(_bl, engineer.Id).ShowDialog();
                    OnChangeEngineer();
                    //this.Close();
                }
            }
        }
    }
    ///// <summary>
    ///// function to the mouse double click that send us the window with the details of the task
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //if (IsMouseCaptureWithin)
        //    new TaskDatails(_bl, ((BO.Task)TaskListView.SelectedItem).Id, OnChangeTask).ShowDialog();
        var selected = ((ListView)sender).SelectedItem;

        BO.TaskInList item = (((FrameworkElement)e.OriginalSource).DataContext as BO.TaskInList)!;

        if (item != null)
        {
            if (selected is BO.TaskInList task)
            {
                if (IsMouseCaptureWithin)
                {
                    BO.Task taskk = _bl.Task.Read(task.Id);
                    new AddOrUpdateTaskWindow(_bl, taskk.Id).ShowDialog();
                    OnChangeTask();
                    //this.Close();
                }
            }
        }
    }
    /// <summary>
    /// deleget for the engineer list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeEngineer()
    {
        Data.EngineerList = _bl?.Engineer.ReadAll();
    }
    /// <summary>
    /// deleget for the order list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeTask()
    {
        Data.TaskList = _bl?.Task.ReadAllTaskInList();
    }
}