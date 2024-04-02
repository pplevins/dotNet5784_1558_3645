using PL.engineer_main_windows;
using System.Windows;

namespace PL.client_window;

/// <summary>
/// Interaction logic for clientwindow.xaml
/// </summary>
public partial class clientwindow : Window
{
    /// <summary>
    /// access to the logical layyer.
    /// </summary>
    private BlApi.IBl? _bl;

    public clientwindow(BlApi.IBl? _bl)
    {
        this._bl = _bl;
        InitializeComponent();
        WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
    }
    /// <summary>
    /// finction to the button the open to me the list with the item in store.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowTaskButton2_Click(object sender, RoutedEventArgs e)
    {
        //var window = Application.Current.Windows.OfType<NewTask>().FirstOrDefault();
        //if (window != null)
        //{
        //    window.Activate();
        //    window.Focus();
        //}
        //else
        //    new NewTask(_bl).Show();
    }
    /// <summary>
    /// function to the button the open to me the window to order tracking.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowTaskButton1_Click(object sender, RoutedEventArgs e) => new EngineerTrackingWindow(100).ShowDialog();
}
