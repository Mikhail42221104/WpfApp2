using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SqlClient;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot.Axes;

namespace wpfapp2
{
    public partial class MainWindow : Window
    {
        private List<WorkScheduleTemplate> scheduletemplates;

        public PlotModel Model { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            scheduletemplates = new List<WorkScheduleTemplate>
            {
                new WorkScheduleTemplate("standard", 8),
                new WorkScheduleTemplate("flextime", 10),
                new WorkScheduleTemplate("part-time", 9),
                // другие шаблоны
            };

            Model = new PlotModel { Title = "Work Schedule" };
            CreateBarChart(GetScheduleTemplates());
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private List<WorkScheduleTemplate> GetScheduleTemplates()
        {
            return scheduletemplates;
        }

        private void CreateBarChart(List<WorkScheduleTemplate> scheduletemplates)
        {
            CategoryAxis xaxis = new CategoryAxis { Position = AxisPosition.Bottom };
            LinearAxis yaxis = new LinearAxis { Position = AxisPosition.Left, Minimum = 0 };

            BarSeries barseries = new BarSeries
            {
                Title = "Work Hours",
                LabelPlacement = LabelPlacement.Inside
            };

            foreach (var template in scheduletemplates)
            {
                //barseries.Items.Add(item: new BarItem { Value = template.WorkHours, Title = template.TemplateName });
                xaxis.Labels.Add(template.TemplateName);
            }

            Model.Axes.Add(xaxis);
            Model.Axes.Add(yaxis);
            Model.Series.Add(barseries);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "DESKTOP-UG8F1FT;initial catalog=Employee work schedule;integrated security=true";
            string query = "insert into scheduletemplates (templatename, workhours) values (@templatename, @workhours)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (WorkScheduleTemplate template in GetScheduleTemplates())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@templatename", template.TemplateName);
                        command.Parameters.AddWithValue("@workhours", template.WorkHours);
                        command.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Work schedule templates saved to database.");
        }
    }

    public class WorkScheduleTemplate
    {
        public string TemplateName { get; set; }
        public int WorkHours { get; set; }

        public WorkScheduleTemplate(string templateName, int workHours)
        {
            TemplateName = templateName;
            WorkHours = workHours;
        }
    }
}