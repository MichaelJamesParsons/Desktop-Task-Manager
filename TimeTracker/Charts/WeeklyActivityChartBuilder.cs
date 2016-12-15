using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;

namespace TimeTracker.Charts
{
    class WeeklyActivityChartBuilder
    {
        public static CartesianChart BuildWeeklyTaskCountChart(Dictionary<string, int> report)
        {
            var now = DateTime.Now;
            var days = new List<string>();
            var values = new ChartValues<int>();

            for (var x = 6; x >= 0; x--)
            {
                var dayOfWeek = now.AddDays((x*-1)).DayOfWeek.ToString();
                days.Add(dayOfWeek);
                values.Add(report.ContainsKey(dayOfWeek) ? report[dayOfWeek] : 0);
            }

            var chart = new CartesianChart
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = values
                    }
                }
            };

            chart.AxisX.Add(new Axis
             {
                 Title = "Day of Week",
                 Labels = days
             });

            return chart;
        }
    }
}
