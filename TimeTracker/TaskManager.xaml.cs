using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for TaskManager.xaml
    /// </summary>
    public partial class TaskManager : Page
    {
        private bool _isAnimatingTray;
        private bool _isTrayCollapsed;

        public TaskManager()
        {
            InitializeComponent();
            this._isTrayCollapsed = true;
            this._isAnimatingTray = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this._isAnimatingTray)
            {
                this._isAnimatingTray = true;

                if (_isTrayCollapsed)
                {
                    TaskTrayOptions.BeginAnimation(StackPanel.HeightProperty, new DoubleAnimation
                    {
                        From = 0,
                        To = TaskTrayOptions.ActualHeight,
                        Duration = TimeSpan.FromMilliseconds(300)
                    });
                }
                else
                {
                    TaskTrayOptions.BeginAnimation(StackPanel.HeightProperty, new DoubleAnimation
                    {
                        From = TaskTrayOptions.ActualHeight,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(300)
                    });
                }

                this._isAnimatingTray = false;
                this._isTrayCollapsed = !this._isTrayCollapsed;
            }
        }
    }
}
