using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.admin_window;

/// <summary>
/// Interaction logic for GanttChartView.xaml
/// </summary>
public partial class GanttChartView : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private List<BO.Task> _tasks;

    public List<BO.Task> Tasks
    {
        get { return _tasks; }
        set
        {
            _tasks = value;
            OnPropertyChanged(nameof(Tasks));
        }
    }

    private BlApi.IBl? _bl = BlApi.Factory.Get();

    public GanttChartView()
    {
        InitializeComponent();
        Tasks = _bl.Task.ReadAll().ToList();
        if (_bl.CheckProjectStatus() == BO.ProjectStatus.InProgress)
            DrawGanttChart();
        UpdateTaskList();
    }

    public void DrawGanttChart()
    {
        ganttCanvas.Children.Clear();

        // Draw title with day markers
        double markerWidth = 70;
        double markerHeight = 20;
        double startX = 100;
        double startY = 20;
        DateTime startDate = Tasks[0].ScheduledDate!.Value;
        for (int i = 0; i <= (Tasks[^1].EstimatedDate!.Value - startDate).Days; i++)
        {
            TextBlock dayMarker = new TextBlock
            {
                Text = $"{_bl?.ProjectStartDate!.Value.AddDays(i).ToShortDateString()}",
                Margin = new Thickness(startX + i * markerWidth, startY, 0, 0),
                //FontSize = 10
            };
            ganttCanvas.Children.Add(dayMarker);
        }

        // Draw tasks
        double taskHeight = 30;
        double taskMargin = 10;
        double taskY = startY + markerHeight + taskMargin;
        foreach (var task in Tasks)
        {
            double taskX = (task.ScheduledDate - startDate)!.Value.TotalDays * markerWidth + startX;
            double taskWidth = task.RequiredEffortTime!.Value.TotalDays * markerWidth;

            Rectangle rect = new Rectangle
            {
                Width = taskWidth,
                Height = taskHeight,
                Fill = IsDelayed(task) ? Brushes.LightCoral : Brushes.LightGreen,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(rect, taskX);
            Canvas.SetTop(rect, taskY);

            string dependencies = task.Dependencies.Any() ? string.Join(", ", task.Dependencies.Select(dep => dep.Id)) : "None";

            TextBlock textBlock = new TextBlock
            {
                Text = $"{task.Id}: {task.Alias}. Depend in: {dependencies}",
            Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            ganttCanvas.Children.Add(rect);
            ganttCanvas.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, taskX + 5);
            Canvas.SetTop(textBlock, taskY + 5);

            taskY += taskHeight + taskMargin;
        }
    }

    private bool IsDelayed(BO.Task task)
    {
        return (!task.CompleteDate.HasValue && DateTime.Compare(_bl.Clock, task.EstimatedDate!.Value) > 0) 
                || (task.CompleteDate.HasValue && DateTime.Compare(task.ScheduledDate!.Value, task.CompleteDate.Value) < 0);
    }

    public void UpdateTaskList()
    {
        //taskList.Items.Clear();
        //foreach (var task in Tasks)
        //{
        //    taskList.Items.Add(task.Id);
        //}
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
