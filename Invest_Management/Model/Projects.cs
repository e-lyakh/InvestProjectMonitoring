using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;

namespace Invest_Management.Model
{
    class Projects
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        public Projects()
        {
            Table = new DataTable();
            Table.TableName = "Projects";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Projects";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void UpdateProjectsData()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            var changes = Table.GetChanges();
            if (changes != null)
            {
                adapter.Update(Table);
                Table.AcceptChanges();
            }
        }
    }
}
