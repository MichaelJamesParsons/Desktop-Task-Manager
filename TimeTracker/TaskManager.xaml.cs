using System;
using System.Windows;
using System.Windows.Media.Animation;
using TimeTracker.Ioc;
using TimeTracker.ViewModels;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for TaskManager.xaml
    /// </summary>
    public partial class TaskManager
    {
        private bool _isAnimatingTray;
        private bool _isTrayCollapsed;

        public TaskManager()
        {
            InitializeComponent();
            DataContext = IocKernel.Get<TaskManagerViewModel>();
            _isTrayCollapsed = true;
            _isAnimatingTray = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_isAnimatingTray) return;
            _isAnimatingTray = true;

            if (_isTrayCollapsed)
            {
                TaskTrayOptions.BeginAnimation(HeightProperty, new DoubleAnimation
                {
                    From = 0,
                    To = TaskTrayOptions.ActualHeight,
                    Duration = TimeSpan.FromMilliseconds(300)
                });
            }
            else
            {
                TaskTrayOptions.BeginAnimation(HeightProperty, new DoubleAnimation
                {
                    From = TaskTrayOptions.ActualHeight,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300)
                });
            }

            _isAnimatingTray = false;
            _isTrayCollapsed = !_isTrayCollapsed;
        }
    }
}
