using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private List<WorkSchedule> workScheduleList = new List<WorkSchedule>();
        private List<AbsenceSchedule> absenceScheduleList = new List<AbsenceSchedule>();

        public MainWindow()
        {
            InitializeComponent();
            WorkScheduleListView.ItemsSource = workScheduleList;
            AbsenceScheduleListView.ItemsSource = absenceScheduleList;
        }

        private void CreateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDay = (DateTime)StartDayPicker.SelectedDate;
            DateTime endDay = (DateTime)EndDayPicker.SelectedDate;
            int startHour = int.Parse(StartHourTextBox.Text);
            int endHour = int.Parse(EndHourTextBox.Text);

            for (DateTime day = startDay; day <= endDay; day = day.AddDays(1))
            {
                workScheduleList.Add(new WorkSchedule { Day = day, StartHour = startHour, EndHour = endHour });
            }

            string server = ServerTextBox.Text;
            string database = DatabaseTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {
                connection.Open();

                foreach (var workSchedule in workScheduleList)
                {
                    string sql = $"INSERT INTO ScheduleTemplates (Day, StartHour, EndHour) VALUES (@Day, @StartHour, @EndHour)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Day", workSchedule.Day);
                        command.Parameters.AddWithValue("@StartHour", workSchedule.StartHour);
                        command.Parameters.AddWithValue("@EndHour", workSchedule.EndHour);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void AddAbsenceButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime absenceDate = (DateTime)AbsenceDatePicker.SelectedDate;
            absenceScheduleList.Add(new AbsenceSchedule { Date = absenceDate });
        }

        private void ConnectToSqlServerButton_Click(object sender, RoutedEventArgs e)
        {
            string server = ServerTextBox.Text;
            string database = DatabaseTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {
                connection.Open();
                // TODO: использовать подключение к SQL Server
            }
        }
    }

    public class WorkSchedule
    {
        public DateTime Day { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }

    public class AbsenceSchedule
    {
        public DateTime Date { get; set; }
    }
}