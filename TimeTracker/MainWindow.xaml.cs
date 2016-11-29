using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TimeTracker.Ioc;
using TimeTracker.ViewModels;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = IocKernel.Get<MasterViewModel>();
            Navigate_ContentViewPort(new Dashboard());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Math.Abs(rect.Width - 52) < 1)
            {
                rect.BeginAnimation(StackPanel.WidthProperty, new DoubleAnimation
                {
                    From = rect.ActualWidth,
                    To = 200,
                    Duration = TimeSpan.FromMilliseconds(100)
                });
            }
            else
            {
                rect.BeginAnimation(StackPanel.WidthProperty, new DoubleAnimation
                {
                    From = rect.ActualWidth,
                    To = 52,
                    Duration = TimeSpan.FromMilliseconds(100)
                });
            }
        }

        private void Nav_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            Navigate_ContentViewPort(new Dashboard());
        }

        private void Nav_My_Tasks_Click(object sender, RoutedEventArgs e)
        {
            Navigate_ContentViewPort(new TaskManager());
        }

        private void Nav_Reports_Click(object sender, RoutedEventArgs e)
        {
            Navigate_ContentViewPort(new ReportsView());
        }

        private void Navigate_ContentViewPort(Page page)
        {
            Animate_ContentViewPort();
            contentViewPort.Navigate(page);
        }

        private void Animate_ContentViewPort()
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = new Thickness(0, 50, 0, 0);
            ta.To = new Thickness(0, 0, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            contentViewPort.BeginAnimation(Grid.MarginProperty, ta);

        }
    }
}
