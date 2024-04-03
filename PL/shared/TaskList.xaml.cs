using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Task = BO.Task;

namespace PL.Shared;


/// <summary>
/// Interaction logic for TaskList.xaml
/// </summary>
public partial class TaskList : UserControl
{
    public static readonly DependencyProperty TaskItemsSourceProperty =
        DependencyProperty.Register("TaskItemsSource", typeof(ObservableCollection<Task>), typeof(TaskList), new PropertyMetadata(null));

    public ObservableCollection<Task> TaskItemsSource
    {
        get { return (ObservableCollection<Task>)GetValue(TaskItemsSourceProperty); }
        set { SetValue(TaskItemsSourceProperty, value); }
    }


    public static readonly DependencyProperty BlInstanceProperty =
        DependencyProperty.Register("BlInstance", typeof(BlApi.IBl), typeof(TaskList));

    public BlApi.IBl BlInstance
    {
        get { return (BlApi.IBl)GetValue(BlInstanceProperty); }
        set { SetValue(BlInstanceProperty, value); }
    }
    public TaskList()
    {
        InitializeComponent();
    }


    ///// <summary>
    ///// function to the mouse double click that send us the window with the details of the task
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    private void TaskListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var test = BlInstance?.Engineer?.ReadAll();
        //if (IsMouseCaptureWithin)
        //    new TaskDatails(_bl, ((BO.Task)TaskListView.SelectedItem).Id, OnChangeTask).ShowDialog();
    }
    /// <summary>
    /// deleget for the order list that we want to update him aoutomaticly.
    /// </summary>
    private void OnChangeTask()
    {
        //Data.TaskList = new ObservableCollection<Task>(_bl?.Task.ReadAll());
    }
}
