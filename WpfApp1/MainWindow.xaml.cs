﻿using System;
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

            var selectedItem = WorkScheduleListView.SelectedItems[0];
            string selectedText = selectedItem.ToString();
            ServerTextBox.Text = selectedText;

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

        private void ServerTextBox_TextChanged(object sender, TextChangedEventArgs e)
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