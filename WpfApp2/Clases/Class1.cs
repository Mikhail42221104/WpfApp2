using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using System.Data.SqlClient;

namespace WpfApp2
{
    public class DatabaseManager
    {
        private SqlConnection sqlConnection;
        private string connectionString = "DESKTOP-UG8F1FT;Initial Catalog=Employee work schedule;Integrated Security=True";

        public DatabaseManager()
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Connection Opened Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                Console.WriteLine("Connection Closed Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}