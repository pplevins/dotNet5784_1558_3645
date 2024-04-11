using PL.login_window;
using System.ComponentModel;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBl? _bl = BlApi.Factory.Get();
        public DateTime? CurrentTime
        {
            get => (DateTime?)GetValue(currentTimeProperty);
            set => SetValue(currentTimeProperty, value);
        }

        public static readonly DependencyProperty currentTimeProperty =
            DependencyProperty.Register(nameof(CurrentTime), typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null, (d, e) => ((MainWindow)d).OnPropertyChanged(nameof(CurrentTime))));

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// constructor
        /// </summary>
        public MainWindow()
        {
            //_bl.InitializeDB();
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            UpdateCurrentTime(); // Update CurrentTime property initially
        }


        private void UpdateCurrentTime()
        {
            CurrentTime = _bl.Clock;
        }

        private void AdvanceYear_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceYear(1);
            UpdateCurrentTime();
        }

        private void AdvanceMonth_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceMonth(1);
            UpdateCurrentTime();
        }

        private void AdvanceDay_Click(object sender, RoutedEventArgs e)
        {
            _bl.AdvanceDay(1);
            UpdateCurrentTime();
        }

        private void ResetTime_Click(object sender, RoutedEventArgs e)
        {
            _bl.ResetTime();
            UpdateCurrentTime();
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
                new UserLoginWindow("AdminEntryWindow").ShowDialog();
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
}