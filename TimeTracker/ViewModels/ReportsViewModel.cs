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


            /*
                        SeriesCollection = new SeriesCollection
                        {
                            new ColumnSeries
                            {
                                Title = "2015",
                                Values = new ChartValues<double> { 10, 50, 39, 50 }
                            }
                        };

                        //adding series will update and animate the chart automatically
                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = "2016",
                            Values = new ChartValues<double> { 11, 56, 42 }
                        });

                        //also adding values updates and animates the chart automatically
                        SeriesCollection[1].Values.Add(48d);

                        var Labels = new[] { "Wednesday","Thursday", "Friday", "Saturday", "Sunday", "Monday", "Tuesday" };
                        System.Runtime.Serialization.Formatter = value => value.ToString("N");*/





  /*          WeeklyActivityChart = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<int> { 1,3,5,2,8,9,10 }
                }
            };

*/




            //Test data
            WeeklyActivityChart = new SeriesCollection
            {
                /*new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4 }
                },*/
                new LineSeries
                {
                    Values = new ChartValues<int> { 1,3,5,2,8,9,10 }
                }
            };
        }
    }
}
