using BO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;
/// <summary>
/// Interaction logic for EngineerList.xaml
/// </summary>
public partial class EngineerAndTaskList : Window, INotifyPropertyChanged
{
    /// <summary>
    /// access to the logical layer.
    /// </summary>
    private BlApi.IBl? _bl;

    public event PropertyChangedEventHandler? PropertyChanged;
    //IEnumerable<IGrouping<BO.Engineer?, BO.Task>> groups;
    /// <summary>
    /// initialize depency property
    /// </summary>
    private IEnumerable<BO.Engineer?>? engineerList_e;
    public IEnumerable<BO.Engineer?>? engineerList { get { return engineerList_e; } set { engineerList_e = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("engineerList")); } } }

    private IEnumerable<BO.Task?>? taskList_t;
    public IEnumerable<BO.Task?>? taskList { get { return taskList_t; } set { taskList_t = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("taskList")); } } }

    /// <summary>
    /// constructor
    /// </summary>
    public EngineerAndTaskList(BlApi.IBl? _bl)
    {
        this._bl = _bl;
        InitializeComponent();
        engineerList = _bl?.Engineer.ReadAll()!;
        taskList = _bl?.Task.ReadAll()!;

        EngineerLevelSelector.ItemsSource = Enum.GetValues(typeof(BO.EngineerExperience));
    }

    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EngineerLevelSelector.SelectedIndex >= 0)
            engineerList = engineerList?.Where(item => (BO.EngineerExperience)EngineerLevelSelector.SelectedItem == item?.Level);
    }
    /// <summary>
    /// button to reset the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset_button_Click(object sender, RoutedEventArgs e)
    {
        _bl?.ResetDB();
        engineerList = null;
        taskList = null;
        EngineerLevelSelector.ItemsSource = Enum.GetValues(typeof(BO.EngineerExperience));
    }
    /// <summary>
    /// button to add engineer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Add_Engineer_Button_Click(object sender, RoutedEventArgs e) => new AddOrUpdateEngineerWindow(_bl, OnChangeEngineer).ShowDialog();
    /// <summary>
    /// event double click to open the new window of update product with the id that chooce.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EngineerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (EngineerListView.SelectedItem is Engineer engineer)
        {
            if (IsMouseCaptureWithin)
                new AddOrUpdateEngineerWindow(_bl, engineer.Id, OnChangeEngineer).ShowDialog();
        }
    }
    ///// <summary>
    ///// function to the mouse double click that send us the window with the details of the product
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //if (IsMouseCaptureWithin)
        //    new TaskDatails(_bl, ((BO.Task)TaskListView.SelectedItem).Id, OnChangeTask).ShowDialog();
    }
    /// <summary>
    /// deleget for the product list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeEngineer()
    {
        engineerList = _bl?.Engineer.ReadAll();
    }
    /// <summary>
    /// deleget for the order list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeTask()
    {
        taskList = _bl?.Task.ReadAll();
    }
}
