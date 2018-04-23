using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    public class Levels
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        Dictionary<string, string> levelIdByName;
        public Dictionary<string, string> LevelIdByName
        {
            get
            {
                return levelIdByName;
            }
        }

        public Levels()
        {
            Table = new DataTable();
            Table.TableName = "Levels";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Levels Order By RowOrder";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);

                levelIdByName = GetIdByName();
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

        public void UpdateLevelsData()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            var changes = Table.GetChanges();
            if (changes != null)
            {
                adapter.Update(Table);
                Table.AcceptChanges();
            }
        }

        private Dictionary<string, string> GetIdByName()
        {
            Dictionary<string, string> levelIdByName = new Dictionary<string, string>();
            for (int i = 0; i < Table.Rows.Count; i++)
            {
                string name = (string)Table.Rows[i].ItemArray[1];
                string id = (string)Table.Rows[i].ItemArray[0];
                levelIdByName.Add(name, id);
            }
            return levelIdByName;
        }
    }
}
