using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

public class AddDependencyWindowData : DependencyObject
{

    // Using a DependencyProperty as the backing store for Engineers.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty taskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(AddDependencyWindowData));
    public BO.Task? Task
    {
        get => (BO.Task)GetValue(taskProperty);
        set => SetValue(taskProperty, value);
    }

    public static readonly DependencyProperty depListProperty =
        DependencyProperty.Register("DepList", typeof(List<BO.TaskInList>), typeof(AddDependencyWindowData));
    public List<TaskInList> DepList
    {
        get => (List<BO.TaskInList>)GetValue(depListProperty);
        set => SetValue(depListProperty, value);
    }

    public BO.TaskInList? DependentToChoose { get; set; }
}

/// <summary>
/// Interaction logic for SetScheduleWindow.xaml
/// </summary>
public partial class AddDependencyWindow : Window
{
    BlApi.IBl? _bl;
    public static readonly DependencyProperty DataDep = DependencyProperty.Register(nameof(Data), typeof(AddDependencyWindowData), typeof(AddDependencyWindow));
    public AddDependencyWindowData Data
    {
        get => (AddDependencyWindowData)GetValue(DataDep);
        set
        {
            SetValue(DataDep, value);
        }
    }
    public AddDependencyWindow(BO.Task boTask)
    {
        _bl = BlApi.Factory.Get();
        Data = new()
        {
            Task = boTask,
            DepList = _bl.Task.CalcDependenciesToChoose(boTask.Id)
        };
        InitializeComponent();
    }

    private void Add_click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Data.DependentToChoose is not null)
            {
                _bl?.Task.addDependency(Data.Task, Data.DependentToChoose.Id);
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// category selector in the combo box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DependenciesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = ((ComboBox)sender).SelectedItem;
        if (selected != null)
        {
            Data.DependentToChoose = (BO.TaskInList)DependenciesList.SelectedItem;
        }
    }
}
