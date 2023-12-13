using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<string> workScheduleList = new List<string>();
        private List<string> absenceScheduleList = new List<string>();
        public string startDay;
        public string endDay;
        public string startTime;
        public string endTime;
        public string chenge;
        public string selectedText;
        public MainWindow()
        {
            InitializeComponent();
            WorkScheduleListView.ItemsSource = workScheduleList;
            AbsenceScheduleListView.ItemsSource = absenceScheduleList;
            LoadDataFromDatabase();
            LordDataFromDatabase();
            
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
                    command.Parameters.AddWithValue("@beginning_of_the_shift", $"{startDay}");
                    command.Parameters.AddWithValue("@end_of_shift",$"{endDay}");
                    //command.Parameters.AddWithValue("@working_day_length", $"{startDay}-{endDay}");

                    command.ExecuteNonQuery();
                }

            }
            //LoadDataFromDatabase();

        }



        private void ConnectToSqlServerButton_Click(object sender, RoutedEventArgs e)
        {
             
            startTime = StartTimePicker.Text;
            endTime = EndTimePicker.Text;


            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            string connectionString = $"Server={server};Database={database};User ID={username};Password={password}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"INSERT INTO Schedules (Chenge,StartTime, EndTime) VALUES (@Chenge,@StartTime, @EndTime)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Задаем значение для параметра @Chenge


                    command.Parameters.AddWithValue("@Chenge", selectedText);
                    command.Parameters.AddWithValue("@StartTime", $"{startTime}");
                    command.Parameters.AddWithValue("@EndTime", $"{endTime}");
                    //command.Parameters.AddWithValue("@working_day_length", $"{startDay}-{endDay}");

                    command.ExecuteNonQuery();
                }
                connection.Close();
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
                            //string workingDayLength = reader["end_of_shift"].ToString();
                            string[] parts = workingDayLength.Split('-');
                            string WorcingendDay = reader["end_of_shift"].ToString();
                            string[] part = WorcingendDay.Split('-');
                            string startDay = (parts[0]);
                            string endDay = (part[0]);
                            //startDay = parts[3];

                            workScheduleList.Add($"{startDay}-{endDay}");

                        }
                    }
        connection.Close();
                }
            }

            WorkScheduleListView.ItemsSource = workScheduleList;
        }

        private void LordDataFromDatabase()
        {
            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
            {
                connection.Open();

                string sql = "SELECT * FROM Schedules";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //string workingDayLength = reader["beginning_of_the_shift"].ToString();
                            //string workingDayLength = reader["end_of_shift"].ToString();
                            //string[] parts = workingDayLength.Split('-');
                            string Worcingchenge = reader["Chenge"].ToString();
                            string WorcingstartTime = reader["startTime"].ToString();
                            string WorcingendTime = reader["endTime"].ToString();


                            string[] part1 = Worcingchenge.Split('-');
                            string[] part2 = WorcingstartTime.Split('-');
                            string[] part3 = WorcingendTime.Split('-');

                            string chenge = (part1[0]);
                            string startTime = (part2[0]);
                            string endTime = (part3[0]);
                            //startDay = parts[3];

                            absenceScheduleList.Add($" {chenge}   {startTime}{endTime}");

                        }
                    }
                    connection.Close();
                }
            }

            AbsenceScheduleListView.ItemsSource = absenceScheduleList;
        }


        private void ServerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ConnectToSqlButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = WorkScheduleListView.SelectedItems[0];
            selectedText = selectedItem.ToString();
            CengeTextBox.Text = selectedText;

        }

        private void DatabaseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            

        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void DEelete_Button_Click(object sender, RoutedEventArgs e)
        {
            string server = "DESKTOP-UG8F1FT";
            string database = "Employee work schedule";
            string username = "Mss";
            string password = "mikhail4222104";

            if (WorkScheduleListView.SelectedItems.Count > 0)
            {
                var selectedItem = WorkScheduleListView.SelectedItems[0];

                if (selectedItem is DataRowView dataRowView)
                {
                    var id = (int)dataRowView["TemplateID"];

                    using (SqlConnection connection = new SqlConnection($"Server={server};Database={database};User ID={username};Password={password}"))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("DELETE FROM ScheduleTemplates WHERE TemplateID = @TemplateID", connection))
                        {
                            command.Parameters.AddWithValue("@TemplateID", id);
                            command.ExecuteNonQuery();
                        }
                    }

                    // WorkScheduleListView.Items.Remove(selectedItem);
                //}
                //else
                //{
                //    // Handle error case
                //}
            }
        }




        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

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