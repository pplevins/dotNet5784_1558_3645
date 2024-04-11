using PL.admin_window;
using PL.engineer_main_windows;
using System.Windows;

namespace PL.login_window
{

    public class UserLoginWindowData : DependencyObject
    {
        public static readonly DependencyProperty userProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(UserLoginWindowData));

        public BO.User? User
        {
            get => (BO.User?)GetValue(userProperty);
            set => SetValue(userProperty, value);
        }
    }

    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLoginWindow : Window
    {

        private BlApi.IBl? _bl = BlApi.Factory.Get();
        string _locked_page;
        public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(UserLoginWindowData), typeof(UserLoginWindow));
        public UserLoginWindowData Data { get => (UserLoginWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }
        public UserLoginWindow(string locked_page = "engineer")
        {
            this._locked_page = locked_page;
            Data = new UserLoginWindowData()
            {
                User = new BO.User()
            };
            InitializeComponent();
        }

        private void loginButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _bl.User.Login(Data?.User);
                if (_locked_page == "AdminEntryWindow")
                {
                    if (user?.UserPermission == BO.UserPermission.Manager) goToAdminPage(user.Id);
                    else MessageBox.Show("you only an engineer, not a manager");
                }
                else goToEngineerPage(user.Id);
                Close();
            }
            catch (BO.Exceptions.BlAlreadyExistsException ex) when (ex.InnerException is not null)
            {
                MessageBox.Show(ex.Message + ex.InnerException!.Message);
            }
            catch (BO.Exceptions.BlDoesNotExistException ex) when (ex.InnerException is not null)
            {
                MessageBox.Show(ex.Message + ex.InnerException!.Message);
            }
            catch (BO.Exceptions.BlInvalidCredentialsException ex) when (ex.InnerException is not null)
            {
                MessageBox.Show(ex.Message + ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void goToAdminPage(int adminId)
        {

            var window = Application.Current.Windows.OfType<AdminEntryWindow>().FirstOrDefault();
            if (window != null)
            {
                window.Activate();
                window.Focus();
            }
            else
            {
                new AdminEntryWindow(adminId).Show();
            }
        }

        /// <summary>
        /// fo to engineer window.
        /// </summary>
        private void goToEngineerPage(int engineerId)
        {
            new EngineerTrackingWindow(engineerId).Show();
        }
    }
}
