using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    class Statuses
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        Dictionary<string, int> statusIdByName;
        public Dictionary<string, int> StatusIdByName
        {
            get
            {
                return statusIdByName;
            }
        }

        public Statuses()
        {
            Table = new DataTable();
            Table.TableName = "Statuses";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Statuses";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);

                statusIdByName = GetIdByName();
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

        public void UpdateStatusesData()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            var changes = Table.GetChanges();
            if (changes != null)
            {
                adapter.Update(Table);
                Table.AcceptChanges();
            }
        }

        private Dictionary<string, int> GetIdByName()
        {
            Dictionary<string, int> statusIdByName = new Dictionary<string, int>();
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                string name = (string)Table.Rows[i].ItemArray[1];
                int id = (int)Table.Rows[i].ItemArray[0];
                statusIdByName.Add(name, id);
            }
            return statusIdByName;
        }
    }
}
