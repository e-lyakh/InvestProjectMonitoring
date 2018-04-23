using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    public class Types
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        Dictionary<string, string> typeIdByName;
        public Dictionary<string, string> TypeIdByName
        {
            get
            {
                return typeIdByName;
            }
        }

        public Types()
        {
            Table = new DataTable();
            Table.TableName = "Types";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Types Order By RowOrder";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);

                typeIdByName = GetIdByName();
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

        public void UpdateTypesData()
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
            Dictionary<string, string> typeIdByName = new Dictionary<string, string>();
            for (int i = 0; i < Table.Rows.Count; i++)
            {                
                string name = (string)Table.Rows[i].ItemArray[1];
                string id = (string)Table.Rows[i].ItemArray[0];
                typeIdByName.Add(name, id);
            }
            return typeIdByName;
        }
    }    
}
