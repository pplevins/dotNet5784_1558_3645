using PL.admin_window;
using PL.login_window;
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
            //_bl.InitializeDB();
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

            //var window = Application.Current.Windows.OfType<UserLoginWindow>().FirstOrDefault();
            ////var window = Application.Current.Windows.OfType<login_window>().FirstOrDefault();
            //if (window != null)
            //{
            //    window.Activate();
            //    window.Focus();
            //}
            //else
            //{
            //    new UserLoginWindow(_bl, "EngineerAndTaskList").ShowDialog();
            //}
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


            new UserLoginWindow(_bl, "engineer").Show();
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
}