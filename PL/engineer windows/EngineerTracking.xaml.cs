using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PL.engineer_main_windows;

/// <summary>
/// Interaction logic for EngineerTrackingWindow.xaml
/// </summary>
public partial class EngineerTrackingWindow : Window, INotifyPropertyChanged
{
    private BlApi.IBl? _bl = BlApi.Factory.Get();
    public event PropertyChangedEventHandler? PropertyChanged;

    private BO.TaskInEngineer task;
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
    public EngineerTrackingWindow()
    {
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
            task = _bl?.Engineer.Read(engineerId)?.Task;
            taskTrackingString = task?.ToString()!;
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
