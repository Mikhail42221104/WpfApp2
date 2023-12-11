using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<string> workScheduleList = new List<string>();
        private List<AbsenceSchedule> absenceScheduleList = new List<AbsenceSchedule>();
        public string startDay;
        public string endDay;
        public MainWindow()
        {
            InitializeComponent();
            WorkScheduleListView.ItemsSource = workScheduleList;
            AbsenceScheduleListView.ItemsSource = absenceScheduleList;
            LoadDataFromDatabase();
        }

        private void CreateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            startDay = StartDayPicker.SelectedDate.ToString();
            endDay = EndDayPicker.SelectedDate.ToString();
            startDay = startDay.Substring(0, startDay.Length - 8);
            endDay = endDay.Substring(0, endDay.Length - 8);
            //workScheduleList.Add($"{startDay} {endDay} {startHour}-{endHour}") ;

            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {
                connection.Open();



                string sql = $"INSERT INTO ScheduleTemplates (beginning_of_the_shift,end_of_shift)  VALUES (@beginning_of_the_shift, @end_of_shift)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@beginning_of_the_shift", $"{startDay}-{endDay}");
                    command.Parameters.AddWithValue("@end_of_shift", endDay);
                    //command.Parameters.AddWithValue("@working_day_length", $"{startDay}-{endDay}");

                    command.ExecuteNonQuery();
                }

            }
            LoadDataFromDatabase();

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

        private void LoadDataFromDatabase()
        {
            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {
                connection.Open();

                string sql = "SELECT * FROM ScheduleTemplates";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string workingDayLength = reader["beginning_of_the_shift"].ToString();

                            string[] parts = workingDayLength.Split('-');

                            string startDay = (parts[0]);
                            string endDay = (parts[1]);
                            //startDay = parts[3];

                            workScheduleList.Add($"{startDay}-{endDay}");

                        }
                    }
                }
            }

            WorkScheduleListView.ItemsSource = workScheduleList;
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