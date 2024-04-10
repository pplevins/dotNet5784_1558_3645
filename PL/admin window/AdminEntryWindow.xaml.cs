using PL.login_window;
using System.ComponentModel;
using System.Windows;

namespace PL.admin_window;

/// <summary>
/// Interaction logic for AdminEntryWindow.xaml
/// </summary>
public partial class AdminEntryWindow : Window
{
    private BlApi.IBl? _bl = BlApi.Factory.Get();
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string welcomeString;

    public string statusString;
    /// <summary>
    /// constructor
    /// </summary>
    public AdminEntryWindow(int userId)
    {
        welcomeString = $"Welcome, {_bl.Engineer.Read(userId)?.Name ?? ""}!";
        statusString = $"Project Status: {_bl.CheckProjectStatus()}";
        InitializeComponent();
        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
    }

    /// <summary>
    /// event to double click to go admin window.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowAdminButton_Click(object sender, RoutedEventArgs e)
    {

        //var window = Application.Current.Windows.OfType<EngineerAndTaskList>().FirstOrDefault();
        //if (window != null)
        //{
        //    window.Activate();
        //    window.Focus();
        //}
        //else
        //{
        //    new EngineerAndTaskList().Show();
        //}

        var window = Application.Current.Windows.OfType<UserLoginWindow>().FirstOrDefault();
        if (window != null)
        {
            window.Activate();
            window.Focus();
        }
        else
        {
            new UserLoginWindow("EngineerAndTaskList").ShowDialog();
        }
    }

    /// <summary>
    /// event to double click to go engineer window.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowEngineerButton_Click(object sender, RoutedEventArgs e)
    {
        //var window = Application.Current.Windows.OfType<EngineerTrackingWindow>().FirstOrDefault();
        ////var window = Application.Current.Windows.OfType<login_window>().FirstOrDefault();
        //if (window != null)
        //{
        //    window.Activate();
        //    window.Focus();
        //}
        //else
        //{
        //    new EngineerTrackingWindow(100).Show();
        //}

        new UserLoginWindow("engineer").Show();
    }

    /// <summary>
    /// event to double click to go initialize the database.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void InitializeDB_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to initialize the DB?", "Initialize",
            MessageBoxButton.YesNoCancel, MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        switch (mbResult)
        {
            case MessageBoxResult.Yes:
                _bl.InitializeDB();
                break;
            case MessageBoxResult.No:
                break;
            case MessageBoxResult.Cancel:
                break;
        }
    }

}