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
public partial class GanttChartView : UserControl, INotifyPropertyChanged
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
        double markerWidth = 50;
        double markerHeight = 20;
        double startX = 100;
        double startY = 20;
        DateTime startDate = Tasks[0].ScheduledDate!.Value;
        for (int i = 0; i <= (Tasks[^1].EstimatedDate!.Value - startDate).Days; i++)
        {
            TextBlock dayMarker = new TextBlock
            {
                Text = $"{_bl?.ProjectStartDate!.Value.AddDays(i)}",
                Margin = new Thickness(startX + i * markerWidth, startY, 0, 0)
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

            TextBlock textBlock = new TextBlock
            {
                Text = $"{task.Id}: {task.Alias}",
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
        return false;
    }

    public void UpdateTaskList()
    {
        taskList.Items.Clear();
        foreach (var task in Tasks)
        {
            taskList.Items.Add(task.Id);
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ganttChart1;

public partial class MainWindow : Window, INotifyPropertyChanged
{
public event PropertyChangedEventHandler PropertyChanged;

private List<Task> _tasks;

public List<Task> Tasks
{
    get { return _tasks; }
    set
    {
        _tasks = value;
        OnPropertyChanged(nameof(Tasks));
        DrawGanttChart();
        UpdateTaskList();
    }
}

public MainWindow()
{
    InitializeComponent();

    // Dummy task data
    Tasks = new List<Task>
    {
        new Task("Task 1", new DateTime(2024, 4, 1), 3),
        new Task("Task 2", new DateTime(2024, 4, 4), 2),
        new Task("Task 3", new DateTime(2024, 4, 6), 4),
        new Task("Task 4", new DateTime(2024, 4, 3), 5),
        new Task("Task 5", new DateTime(2024, 4, 8), 2),
    };
}

private void DrawGanttChart()
{
    ganttCanvas.Children.Clear();

    // Draw title with day markers
    double markerWidth = 50;
    double markerHeight = 20;
    double startX = 100;
    double startY = 20;
    DateTime startDate = Tasks[0].StartDate;
    for (int i = 0; i <= (Tasks[^1].EndDate - startDate).Days; i++)
    {
        TextBlock dayMarker = new TextBlock
        {
            Text = $"Day {i + 1}",
            Margin = new Thickness(startX + i * markerWidth, startY, 0, 0)
        };
        ganttCanvas.Children.Add(dayMarker);
    }

    // Draw tasks
    double taskHeight = 30;
    double taskMargin = 10;
    double taskY = startY + markerHeight + taskMargin;
    foreach (var task in Tasks)
    {
        double taskX = (task.StartDate - startDate).TotalDays * markerWidth + startX;
        double taskWidth = task.Duration * markerWidth;

        Rectangle rect = new Rectangle
        {
            Width = taskWidth,
            Height = taskHeight,
            Fill = task.IsDelayed ? Brushes.LightCoral : Brushes.LightGreen,
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };

        Canvas.SetLeft(rect, taskX);
        Canvas.SetTop(rect, taskY);

        TextBlock textBlock = new TextBlock
        {
            Text = task.Name,
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

private void UpdateTaskList()
{
    taskList.Items.Clear();
    foreach (var task in Tasks)
    {
        taskList.Items.Add(task.Name);
    }
}

protected virtual void OnPropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
}

public class Task
{
public string Name { get; set; }
public DateTime StartDate { get; set; }
public int Duration { get; set; }
public bool IsDelayed { get; set; }

public DateTime EndDate => StartDate.AddDays(Duration);

public Task(string name, DateTime startDate, int duration)
{
    Name = name;
    StartDate = startDate;
    Duration = duration;
    IsDelayed = false; // By default, tasks are assumed to be on time
}
}
*/
