using PL.admin_window;
using PL.client_window;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// accses for the logical layyer.
        /// </summary>
        private BlApi.IBl? _bl = BlApi.Factory.Get();

        /// <summary>
        /// constructor
        /// </summary>
        public MainWindow()
        {
            _bl.InitializeDB();
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// event to double click to go update the product.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowAdminButton_Click(object sender, RoutedEventArgs e)
        {

            var window = Application.Current.Windows.OfType<EngineerAndTaskList>().FirstOrDefault();
            if (window != null)
            {
                window.Activate();
                window.Focus();
            }
            else
            {
                new EngineerAndTaskList(_bl).Show();
            }
        }
        private void ShowEngineerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<clientwindow>().FirstOrDefault();
            if (window != null)
            {
                window.Activate();
                window.Focus();
            }
            else
            {
                new clientwindow(_bl).Show();
            }
        }

    }
}