using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;
public class AddOrUpdateEngineerWindowData : DependencyObject
{
    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty engineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(AddOrUpdateEngineerWindowData));
    public BO.Engineer? Engineer
    {
        get => (BO.Engineer?)GetValue(engineerProperty);
        set => SetValue(engineerProperty, value);
    }

    public Array? EngineerLevel { get; set; }
    public Visibility updateMode { get; set; }
    public Visibility addMode { get; set; }

    public bool isReadOnlyID { get; set; }
    public string buttonName { get; set; }
}

/// <summary>
/// Interaction logic for AddOrUpdateEngineerWindow.xaml
/// </summary>
public partial class AddOrUpdateEngineerWindow : Window
{
    BlApi.IBl? _bl;
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(AddOrUpdateEngineerWindowData), typeof(AddOrUpdateEngineerWindow));
    public AddOrUpdateEngineerWindowData Data { get => (AddOrUpdateEngineerWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }

    /// <summary>
    /// constructor for update window
    /// </summary>
    /// <param name="_blForAdd"></param>
    /// <param name="c"></param>
    public AddOrUpdateEngineerWindow(BlApi.IBl? _blForAdd, int id = 0)
    {
        //////////////////////////////////////////////////////////////////////////
        _bl = _blForAdd;
        Data = new()
        {
            isReadOnlyID = id == 0 ? false : true,
            Engineer = id == 0 ? new() : _bl?.Engineer.Read(id),
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
    private void addOrUpdateEngineerButton(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Data.addMode == Visibility.Visible)
                _bl?.Engineer.Create(Data?.Engineer);
            else
                _bl?.Engineer.Update(Data?.Engineer);
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
        //e.Handled = !Regex.IsMatch(e.Text, @"^(?=.*[a-zA-Z].{2})[a-zA-Z ,.'-]+$");
    }


    /// <summary>
    /// dont put latters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void email_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        //e.Handled = !Regex.IsMatch(e.Text, @"^(?=.*[a-zA-Z].{2})[^\s@]+@[^\s@]+\.[^\s@]+$");
    }


    /// <summary>
    /// dont put latters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void id_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        //e.Handled = !Regex.IsMatch(e.Text, "^[^0-9]+$");
    }


    /// <summary>
    /// dont put letters in the text box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        //e.Handled = !Regex.IsMatch(e.Text, @"^[0-9](?:\.[0-9])?$");
    }

    /// <summary>
    /// function to conect the text box with the slider in too way with dependency property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cost_TextChanged(object sender, TextChangedEventArgs e)
    {
        var textPrice = sender as TextBox;
        if (double.TryParse(textPrice?.Text, out double z))
            Data.Engineer.Cost = Convert.ToDouble(textPrice?.Text);
        else
            Data.Engineer.Cost = 0;
    }
}