using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.admin_window;

public class SetScheduleWindowData : DependencyObject
{

    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty taskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(SetScheduleWindowData));
    public BO.Task? Task
    {
        get => (BO.Task?)GetValue(taskProperty);
        set => SetValue(taskProperty, value);
    }
    public DateTime? ProjectDate { get; set; }
}

/// <summary>
/// Interaction logic for SetScheduleWindow.xaml
/// </summary>
public partial class SetScheduleWindow : Window
{
    BlApi.IBl? _bl;
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(SetScheduleWindowData), typeof(SetScheduleWindow));
    public SetScheduleWindowData Data
    {
        get => (SetScheduleWindowData)GetValue(DataDep);
        set
        {
            SetValue(DataDep, value);
           // ((SetScheduleWindowData)value).OnPropertyChanged();
        }
    }
    public SetScheduleWindow()
    {
        _bl = BlApi.Factory.Get();
        Data = new()
        {
            ProjectDate = _bl.ProjectStartDate
        };
        InitializeComponent();
    }

    private void SetTheDate_click(object sender, RoutedEventArgs e)
    {
        try
        {
            _bl!.ProjectStartDate = Data.ProjectDate;
            MessageBox.Show("Done! Now the project has a start date!\n\nGo to the tasks list and schedule their scheduleDate.\nTip: use the button 'suggest scheduleDate'",
                "Project Date saved", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
