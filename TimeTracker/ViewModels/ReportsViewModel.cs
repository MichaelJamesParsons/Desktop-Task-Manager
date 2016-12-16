using LiveCharts.Wpf;
using TimeTracker.Charts;
using TimeTracker.Services;

namespace TimeTracker.ViewModels
{
    class ReportsViewModel : BaseViewModel
    {
        /// <summary>
        /// The weekly activity chart to be rendered in the reports view.
        /// </summary>
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
        
        /// <summary>
        /// Initializes the reports view model.
        /// </summary>
        /// <param name="reportsService"></param>
        public ReportsViewModel(IReportsRestService reportsService)
        {
            //Get the weekly activity from the service.
            var weekReport = reportsService.GetWeeklyActivity();

            //Build a chart from the service's report.
            var weekReportChart = WeeklyActivityChartBuilder.BuildWeeklyTaskCountChart(
                weekReport.GetNumberOfTasksEachWeekDay()
            );
            
            //Render the chart in the view.
            WeeklyActivityChart = weekReportChart;
        }
    }
}
