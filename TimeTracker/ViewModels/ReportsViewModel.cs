using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;
using TimeTracker.Models;
using TimeTracker.Services;

namespace TimeTracker.ViewModels
{
    class ReportsViewModel : BaseViewModel
    {
        private MasterViewModel _masterViewModel;
        private SeriesCollection _weeklyActivityChart;

        public SeriesCollection WeeklyActivityChart
        {
            get
            {
                return _weeklyActivityChart;
            }

            set
            {
                _weeklyActivityChart = value;
                OnPropertyChanged();
            }
        }

        public ReportsViewModel(MasterViewModel masterViewModel)
        {
            _masterViewModel = masterViewModel;

            //Test data
            WeeklyActivityChart = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4 }
                },
                new LineSeries
                {
                    Values = new ChartValues<double> { 1,3,5,2,8,9,10,15 }
                }
            };
        }
    }
}
