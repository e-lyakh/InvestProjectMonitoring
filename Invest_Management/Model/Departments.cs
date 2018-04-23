using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.Generic;

namespace Invest_Management.Model
{
    public class Departments
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;

        public Departments()
        {
            Table = new DataTable();
            Table.TableName = "Departments";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            
            string sql = "SELECT * FROM Departments";
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

        public void UpdateDepartmentsData()
        {            
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            var changes = Table.GetChanges();
            if (changes != null)
            {
                adapter.Update(Table);
                Table.AcceptChanges();
            }
        }

        public string GetIdByNameAndSubsidiary(string departmentName, int subsidiaryId)
        {
            string departmentId = null;
            for (int i = 0; i < Table.Rows.Count; i++)
            {                
                string id = (string)Table.Rows[i].ItemArray[0];
                string name_t = (string)Table.Rows[i].ItemArray[1];
                int subsidiary_t = (int)Table.Rows[i].ItemArray[2];
                if (name_t == departmentName && subsidiary_t == subsidiaryId)
                    departmentId = id;
            }
            return departmentId;
        }
    }
}
