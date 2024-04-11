using BO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;
/// <summary>
/// Interaction logic for EngineerList.xaml
/// </summary>
public class EngineerAndTaskListData : DependencyObject
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the engineers.
    /// </summary>
    public ObservableCollection<Engineer>? EngineerList
    {
        get { return (ObservableCollection<Engineer>?)GetValue(engineerProperty); }
        set { SetValue(engineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty engineerProperty =
        DependencyProperty.Register("EngineerList", typeof(ObservableCollection<Engineer>), typeof(EngineerAndTaskListData));

    /// <summary>
    /// Gets or sets the list of tasks.
    /// </summary>
    public ObservableCollection<TaskInList>? TaskList
    {
        get { return (ObservableCollection<TaskInList>?)GetValue(tasksListProperty); }
        set { SetValue(tasksListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty tasksListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<TaskInList>), typeof(EngineerAndTaskListData));
    // <summary>
    // Gets or sets the EngineerLevelSelector.
    // </summary>

    public List<string>? EngineerLevelSelector
    {
        get
        {
            var enumNames = Enum.GetNames(typeof(BO.EngineerExperience)).ToList();
            enumNames.Insert(0, "All");
            return enumNames;
        }
        set { SetValue(engineerLevelSelectorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tasksListProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty engineerLevelSelectorProperty =
        DependencyProperty.Register("EngineerLevelSelector", typeof(List<string>), typeof(EngineerAndTaskListData));
}
public partial class EngineerAndTaskList : Window
{
    //private IEnumerable<Engineer>? originalList;
    /// <summary>
    /// access to the logical layer.
    /// </summary>
    private BlApi.IBl? _bl = BlApi.Factory.Get();

    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(EngineerAndTaskListData), typeof(EngineerAndTaskList));

    /// <summary>
    /// Gets or sets the data for the AdminWindow.
    /// </summary>
    public EngineerAndTaskListData Data
    {
        get => (EngineerAndTaskListData)GetValue(DataDep);
        set
        {
            SetValue(DataDep, value);
            ((EngineerAndTaskListData)value).OnPropertyChanged();
        }
    }

    public static readonly DependencyProperty isProjectSchduledProperty =
        DependencyProperty.Register("isRelatedToEngineerSource", typeof(bool), typeof(EngineerAndTaskList), new PropertyMetadata(false));

    public bool isProjectSchduled
    {
        get { return (bool)GetValue(isProjectSchduledProperty); }
        set { SetValue(isProjectSchduledProperty, value); }
    }

    /// <summary>
    /// constructor
    /// </summary>
    public EngineerAndTaskList()
    {
        Data = new()
        {
            EngineerList = new ObservableCollection<Engineer>(_bl?.Engineer.ReadAll())!,
            TaskList = new ObservableCollection<TaskInList>(_bl?.Task.ReadAllTaskInList())!,
            EngineerLevelSelector = AddAllOptionAtStart()
        };
        InitializeComponent();
    }

    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if (selected != null)
        {
            if (Equals((string)EngineerLevelSelector?.SelectedItem, "All")) Data.EngineerList = new ObservableCollection<Engineer>(_bl?.Engineer.ReadAll());
            else Data.EngineerList = new ObservableCollection<Engineer>(_bl?.Engineer.ReadAll()?.Where(item => Equals((string)EngineerLevelSelector?.SelectedItem, item?.Level.ToString())));
        }

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
        //Data.EngineerLevelSelector = Enum.GetValues(typeof(BO.EngineerExperience));
    }
    /// <summary>
    /// button to add engineer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Add_Engineer_Button_Click(object sender, RoutedEventArgs e)
    {
        new AddOrUpdateEngineerWindow().ShowDialog();
        OnChangeEngineer();
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
                    new AddOrUpdateEngineerWindow(engineer.Id).ShowDialog();
                    OnChangeTask();
                    OnChangeEngineer();
                }
            }
        }
    }

    /// <summary>
    /// deleget for the engineer list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeEngineer()
    {
        Data.EngineerList.Clear();
        Data.EngineerList = new ObservableCollection<Engineer>(_bl?.Engineer.ReadAll());
    }
    /// <summary>
    /// deleget for the order list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeTask()
    {
        Data.TaskList = new ObservableCollection<TaskInList>(_bl?.Task.ReadAllTaskInList());
    }


    private List<string> AddAllOptionAtStart()
    {
        var enumNames = Enum.GetNames(typeof(BO.EngineerExperience)).ToList();
        enumNames.Insert(0, "All");
        return enumNames;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TabItem_SelectionChanged(object sender, RoutedEventArgs e)
    {
        OnChangeEngineer();
        OnChangeTask();
    }
}