using System.Collections.Generic;
using LiveCharts.Wpf;
using TimeTracker.Charts;
using TimeTracker.Services;

namespace TimeTracker.ViewModels
{
    class ReportsViewModel : BaseViewModel
    {
        private MasterViewModel _masterViewModel;

        private CartesianChart _weeklyActivityChart;
        public CartesianChart WeeklyActivityChart
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

        public List<string> labels { get; set; }

        public ReportsViewModel(MasterViewModel masterViewModel, IReportsRestService _reportsService)
        {
            _masterViewModel = masterViewModel;

            var weekReport = _reportsService.GetWeeklyActivity();
            var weekReportChart = WeeklyActivityChartBuilder.BuildWeeklyTaskCountChart(
                weekReport.GetNumberOfTasksEachWeekDay()
            );
            
//            labels = weekReportChart.Label
            WeeklyActivityChart = weekReportChart;
        }
    }
}
