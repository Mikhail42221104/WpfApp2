using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Data.SqlClient;
using OxyPlot.Series;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private List<DataPoint> dataPoints;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация графика
            dataPoints = new List<DataPoint>()
            {
                new DataPoint("Янв", 120),
                new DataPoint("Фев", 80),
                new DataPoint("Мар", 150),
                new DataPoint("Апр", 100),
                new DataPoint("Май", 130)
            };

            CreateBarChart();
        }

        private void CreateBarChart()
        {
            // Создание столбчатой диаграммы
            char chart = new сhart();

            BarSeries barSeries = new BarSeries();

            // Привязка данных к диаграмме
            barSeries.ItemsSource = dataPoints;
            barSeries.DependentValuePath = "Value";
            barSeries.IndependentValuePath = "Month";

            // Настройка цвета столбцов
            barSeries.DataPointStyle = new Style(typeof(DataPoint));

            Setter backgroundSetter = new Setter(DataPoint.BackgroundProperty, Brushes.Blue);
            barSeries.DataPointStyle.Setters.Add(backgroundSetter);

            chart.Series.Add(barSeries);
            ChartArea.Children.Add(chart);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение данных в базе данных SQL Server
            string connectionString = "DESKTOP-UG8F1FT";
            string query = "INSERT INTO WorkSchedule (Month, Value) VALUES (@Month, @Value)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataPoint dataPoint in dataPoints)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Month", dataPoint.Month);
                        command.Parameters.AddWithValue("@Value", dataPoint.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Данные сохранены в базе данных.");
        }
    }

    public class DataPoint
    {
        public string Month { get; set; }
        public int Value { get; set; }

        public DataPoint(string month, int value)
        {
            Month = month;
            Value = value;
        }
    }
}
