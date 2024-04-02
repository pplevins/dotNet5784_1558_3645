using PL.admin_window;
using PL.engineer_main_windows;
using System.Windows;
using System.Windows.Input;

namespace PL.login_window
{

    public class UserLoginWindowData : DependencyObject
    {
        // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
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

        BlApi.IBl? _bl;
        string _locked_page;
        public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(UserLoginWindowData), typeof(UserLoginWindow));
        public UserLoginWindowData Data { get => (UserLoginWindowData)GetValue(DataDep); set => SetValue(DataDep, value); }
        public UserLoginWindow(BlApi.IBl? _bl, string locked_page = "engineer")
        {
            this._bl = _bl;
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
                if (_locked_page == "EngineerAndTaskList")
                {
                    if (user?.UserPermission == BO.UserPermission.Manager)
                    {
                        goToAdminPage();
                    }

                    MessageBox.Show("you only an engineer, not a manager");
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
            //catch (BO.Exceptions. ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void goToAdminPage()
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

        /// <summary>
        /// fo to engineer window.
        /// </summary>
        private void goToEngineerPage(int engineerId)
        {
            var window = Application.Current.Windows.OfType<EngineerTrackingWindow>().FirstOrDefault();
            //var window = Application.Current.Windows.OfType<login_window>().FirstOrDefault();
            if (window != null)
            {
                window.Activate();
                window.Focus();
            }
            else
            {
                new EngineerTrackingWindow(engineerId).Show();
            }
            //var window = Application.Current.Windows.OfType<clientwindow>().FirstOrDefault();
            ////var window = Application.Current.Windows.OfType<login_window>().FirstOrDefault();
            //if (window != null)
            //{
            //    window.Activate();
            //    window.Focus();
            //}
            //else
            //{
            //    new clientwindow(_bl).Show();
            //}
        }

        private void id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
