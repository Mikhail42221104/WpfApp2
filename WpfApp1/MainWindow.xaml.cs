using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<WorkSchedule> workScheduleList = new List<WorkSchedule>();
        private List<AbsenceSchedule> absenceScheduleList = new List<AbsenceSchedule>();
        public string startDay;
        public MainWindow()
        {
            InitializeComponent();
            WorkScheduleListView.ItemsSource = workScheduleList;
            AbsenceScheduleListView.ItemsSource = absenceScheduleList;
        }

        private void CreateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            startDay = StartDayPicker.ToString();

            startDay = startDay.Substring(0, startDay.Length - 8);
            string endDay = EndDayPicker.ToString();
            endDay = endDay.Substring(0, endDay.Length - 8);
            int startHour = int.Parse(StartHourTextBox.Text);
            int endHour = int.Parse(EndHourTextBox.Text);



            workScheduleList.Add(new WorkSchedule { Day = $"{startDay}", StartHour = startHour, EndHour = endHour });


            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {



                connection.Open();

                foreach (var workSchedule in workScheduleList)
                {
                    string sql = $"INSERT INTO ScheduleTemplates (beginning_of_the_shift,end_of_shift, working_day_length) VALUES (@beginning_of_the_shift, @end_of_shift, @working_day_length)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@beginning_of_the_shift", workSchedule.Day);
                        command.Parameters.AddWithValue("@end_of_shift", endDay);
                        command.Parameters.AddWithValue("@working_day_length", $"{workSchedule.StartHour}-{workSchedule.EndHour}");
                        command.ExecuteNonQuery();
                    }
                    break;
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
        public string Day { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }

    public class AbsenceSchedule
    {
        public DateTime Date { get; set; }
    }
}