using System.Windows;

namespace PL.engineer_windows
{
    /// <summary>
    /// Interaction logic for EndOrStartEngineerTask.xaml
    /// </summary>
    public partial class EndOrStartEngineerTask : Window
    {
        public static readonly DependencyProperty taskInEngineer =
            DependencyProperty.Register("TaskInEngineer", typeof(BO.TaskInEngineer), typeof(EndOrStartEngineerTask));

        public BO.TaskInEngineer? TaskInEngineer
        {
            get => (BO.TaskInEngineer?)GetValue(taskInEngineer);
            set => SetValue(taskInEngineer, value);
        }
        public static readonly DependencyProperty EngineerIdSourceProperty =
            DependencyProperty.Register("EngineerIdSource", typeof(int), typeof(EndOrStartEngineerTask), new PropertyMetadata(default(int)));

        public int EngineerIdSource
        {
            get { return (int)GetValue(EngineerIdSourceProperty); }
            set { SetValue(EngineerIdSourceProperty, value); }
        }


        public static readonly DependencyProperty StartButtonVisibilityProperty =
            DependencyProperty.Register(nameof(StartButtonVisibility), typeof(Visibility), typeof(EndOrStartEngineerTask), new PropertyMetadata(Visibility.Collapsed));

        public Visibility StartButtonVisibility
        {
            get { return (Visibility)GetValue(StartButtonVisibilityProperty); }
            set { SetValue(StartButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty EndButtonVisibilityProperty =
            DependencyProperty.Register(nameof(EndButtonVisibility), typeof(Visibility), typeof(EndOrStartEngineerTask), new PropertyMetadata(Visibility.Visible));

        public Visibility EndButtonVisibility
        {
            get { return (Visibility)GetValue(EndButtonVisibilityProperty); }
            set { SetValue(EndButtonVisibilityProperty, value); }
        }


        private BlApi.IBl? _bl = BlApi.Factory.Get();
        public EndOrStartEngineerTask(Boolean isStartTask, int engineerId, BO.TaskInEngineer? taskInEngineer)
        {
            EngineerIdSource = engineerId;
            TaskInEngineer = taskInEngineer;
            var engineer = _bl.Engineer.Read(EngineerIdSource);
            if (isStartTask)
            {
                EndButtonVisibility = Visibility.Collapsed;
                StartButtonVisibility = Visibility.Visible;
            }
            InitializeComponent();
        }

        private void endTaskButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EngineerIdSource != null && TaskInEngineer?.Id != null)
                {
                    int taskId = (int)TaskInEngineer?.Id;
                    _bl.Task.CompleteDateAction(taskId);
                }
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

        private void startTaskButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EngineerIdSource != null && TaskInEngineer?.Id != null)
                {
                    int taskId = (int)TaskInEngineer?.Id;
                    _bl.Task.StartDateCreation(taskId);
                }
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
    }
}
