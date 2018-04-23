using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    public class Subsidiaries
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        Dictionary<string, int> subsidiaryIdByName;
        public Dictionary<string, int> SubsidiaryIdByName
        {
            get
            {
                return subsidiaryIdByName;
            }
        }

        public Subsidiaries()
        {
            Table = new DataTable();
            Table.TableName = "Subsidiaries";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Subsidiaries";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Table);

                subsidiaryIdByName = GetIdByName();
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

        public void UpdateSubsidiariesData()
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
            Dictionary<string, int> subsidiaryIdByName = new Dictionary<string, int>();
            for(int i=0; i<Table.Rows.Count; i++)
            {                
                string name = (string)Table.Rows[i].ItemArray[1];
                int id = (int)Table.Rows[i].ItemArray[0];
                subsidiaryIdByName.Add(name, id);
            }
            return subsidiaryIdByName;
        }
    }
}
