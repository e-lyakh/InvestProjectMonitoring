using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    class Managers
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        Dictionary<string, int> managerIdByName;
        public Dictionary<string, int> ManagerIdByName
        {
            get
            {
                return managerIdByName;
            }
        }

        public Managers()
        {
            Table = new DataTable();
            Table.TableName = "Managers";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Managers ORDER BY ManagerName";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);

                managerIdByName = GetIdByName();
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

        public void UpdateManagersData()
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
            Dictionary<string, int> managerIdByName = new Dictionary<string, int>();
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                string name = (string)Table.Rows[i].ItemArray[2];
                int id = (int)Table.Rows[i].ItemArray[0];
                managerIdByName.Add(name, id);
            }
            return managerIdByName;
        }
    }
}
