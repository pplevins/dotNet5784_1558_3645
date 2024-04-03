using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;

public class AddOrUpdateTaskWindowData : DependencyObject
{
    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty taskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(AddOrUpdateTaskWindowData));
    public BO.Task? Task
    {
        get => (BO.Task?)GetValue(taskProperty);
        set => SetValue(taskProperty, value);
    }

    public Array? EngineerLevel { get; set; }
    public Visibility updateMode { get; set; }
    public Visibility addMode { get; set; }

    public bool isReadOnlyID { get; set; }
    public string buttonName { get; set; }
}

/// <summary>
/// Interaction logic for AddOrUpdateTaskWindow.xaml
/// </summary>
public partial class AddOrUpdateTaskWindow : Window
{
    BlApi.IBl? _bl;
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(AddOrUpdateTaskWindowData), typeof(AddOrUpdateTaskWindow));
    public AddOrUpdateTaskWindowData Data { get => (AddOrUpdateTaskWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

    /// <summary>
    /// constructor for update window
    /// </summary>
    /// <param name="_blForAdd"></param>
    /// <param name="c"></param>
    public AddOrUpdateTaskWindow(BlApi.IBl? _blForAdd, int id = 0)
    {
        //////////////////////////////////////////////////////////////////////////
        _bl = _blForAdd;
        Data = new()
        {
            isReadOnlyID = id == 0 ? false : true,
            Task = id == 0 ? new() : _bl?.Task.Read(id),
            EngineerLevel = Enum.GetValues(typeof(BO.EngineerExperience)),
            addMode = id == 0 ? Visibility.Visible : Visibility.Hidden,
            updateMode = id != 0 ? Visibility.Visible : Visibility.Hidden,
            buttonName = id == 0 ? "CREATE" : "UPDATE",
        };

        InitializeComponent();
    }

    /// <summary>
    /// the button the make the add or update engineer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addOrUpdateTaskButton(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Data.addMode == Visibility.Visible)
                _bl?.Task.Create(Data?.Task);
            else
                _bl?.Task.Update(Data?.Task);
            Close();
        }
        catch (BO.Exceptions.BlAlreadyExistsException ex) when (ex.InnerException is not null)
        {
            MessageBox.Show(ex.Message + ex.InnerException!.Message);
        }
        //catch (BO.Exceptions. ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// dont put numbers in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void name_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = Regex.IsMatch(e.Text, "/^[a - z,.'-]+$/i");
    }

    /// <summary>
    /// dont put latters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void email_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        //e.Handled = Regex.IsMatch(e.Text, "^[^0-9]+$");
    }


    /// <summary>
    /// dont put latters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void id_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = Regex.IsMatch(e.Text, "^[^0-9]+$");
    }


    /// <summary>
    /// dont put letters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, @"^[0-9](?:\.[0-9])?$");
    }

    /// <summary>
    /// function to conect the text box with the slider in too way with dependency property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cost_TextChanged(object sender, TextChangedEventArgs e)
    {
        //var textPrice = sender as TextBox;
        //if (double.TryParse(textPrice?.Text, out double z))
        //    Data.Engineer.Cost = Convert.ToDouble(textPrice?.Text);
        //else
        //    Data.Engineer.Cost = 0;
    }
}


