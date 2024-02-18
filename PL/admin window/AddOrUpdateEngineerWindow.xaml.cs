using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.admin_window;
/// <summary>
/// class to the category that make enumerator to put in the combo box option of the category
/// </summary>
//internal class Categories : IEnumerable
//{
//    static readonly IEnumerator s_enumerator = Enum.GetValues(typeof(BO.CoffeeShop)).GetEnumerator();
//    public IEnumerator GetEnumerator() => s_enumerator;
//}

/// <summary>
/// Interaction logic for AddOrUpdateProductWindow.xaml
/// </summary>
public partial class AddOrUpdateEngineerWindow : Window, INotifyPropertyChanged
{
    BlApi.IBl? _bl;
    public event PropertyChangedEventHandler? PropertyChanged;
    private BO.Engineer engineer_e;
    Action changeList;
    public BO.Engineer engineerDetail { get { return engineer_e; } set { engineer_e = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("engineerDetail")); } } }

    /// <summary>
    /// Constructor for the AddOrUpdateProductWindow class.
    /// </summary>
    /// <param name="_blForAdd">An optional parameter of type BlApi.IBl that represents the business logic object.</param>
    /// <param name="c">A boolean value that determines whether the window is being used to add or update a product.</param>
    public AddOrUpdateEngineerWindow(BlApi.IBl? _blForAdd, bool c)
    {
        InitializeComponent();
        _bl = _blForAdd;
        levelSelector.ItemsSource = Enum.GetValues(typeof(BO.EngineerExperience));
    }
    /// <summary>
    /// constructor for update window
    /// </summary>
    /// <param name="_blForAdd"></param>
    /// <param name="c"></param>
    public AddOrUpdateEngineerWindow(BlApi.IBl? _blForAdd, int productId, Action action) : this(_blForAdd, true)
    {
        //////////////////////////////////////////////////////////////////////////
        engineerDetail = _bl?.Engineer.Read(productId)!;
        changeList = action;
        addOrUpdateEngineer.Content = "Update";
        id.IsEnabled = false;
    }
    /// <summary>
    /// constructor for add window
    /// </summary>
    /// <param name="_blForAdd"></param>
    /// <param name="c"></param>
    public AddOrUpdateEngineerWindow(BlApi.IBl? _blForAdd, Action action) : this(_blForAdd, true)
    {
        changeList = action;
        engineerDetail = new BO.Engineer();
        addOrUpdateEngineer.Content = "Add";
    }

    /// <summary>
    /// the button the make the add or update product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addOrUpdateEngineerButton(object sender, RoutedEventArgs e)
    {
        try
        {
            if (addOrUpdateEngineer?.Content == "Add")
                _bl?.Engineer.Create(engineerDetail);
            else
                _bl?.Engineer.Update(engineerDetail);
            changeList();
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
        e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]*(?:\.[0-9]*)?$");
    }

    /// <summary>
    /// function to conect the text box with the slider in too way with dependency property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cost_TextChanged(object sender, TextChangedEventArgs e)
    {
        changeList();
        var textPrice = sender as TextBox;
        if (double.TryParse(textPrice?.Text, out double z))
            engineerDetail.Cost = Convert.ToDouble(textPrice?.Text);
        else
            engineerDetail.Cost = 0;
    }
}
