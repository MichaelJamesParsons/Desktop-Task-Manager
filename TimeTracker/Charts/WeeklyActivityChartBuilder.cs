using System;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;

namespace TimeTracker.Charts
{
    class WeeklyActivityChartBuilder
    {
        /// <summary>
        /// Builds a cartesian line chart based on a list of number of tasks
        /// completed on a set of days.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static CartesianChart BuildWeeklyTaskCountChart(Dictionary<string, int> report)
        {
            var now = DateTime.Now;
            var days = new List<string>();
            var values = new ChartValues<int>();

            //Define the chart values for the past 7 days (including today).
            for (var x = 6; x >= 0; x--)
            {
                var dayOfWeek = now.AddDays((x*-1)).DayOfWeek.ToString();

                //Add day to chart X-Axis labels.
                days.Add(dayOfWeek);

                //Add value to day
                values.Add(report.ContainsKey(dayOfWeek) ? report[dayOfWeek] : 0);
            }

            //Create the cartesian chart and set its values.
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

            //Set the most recent day to "today".
            days[days.Count - 1] = "Today";

            //Set the second most recent day to "yesterday".
            days[days.Count - 2] = "Yesterday";
            chart.AxisX.Add(new Axis
             {
                 Title = "Day of Week",
                 Labels = days
             });

            //Set the starting value of the Y-Axis to 0.
            chart.AxisY.Add(new Axis
            {
                MinValue = 0
            });

            return chart;
        }
    }
}
