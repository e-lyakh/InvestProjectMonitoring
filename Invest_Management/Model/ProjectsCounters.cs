using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Invest_Management.Model
{
    public class ProjectsCounters
    {
        public DataTable Table { get; set; }
        string connectionString;
        SqlDataAdapter adapter;       

        public ProjectsCounters()
        {
            Table = new DataTable();
            Table.TableName = "ProjectsCounters";

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sql = "SELECT * FROM ProjectsCounters";
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

        public void UpdateProjectsCountersData()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            var changes = Table.GetChanges();
            if (changes != null)
            {
                adapter.Update(Table);
                Table.AcceptChanges();
            }            
        }

        public string GetCounter(int subsidiaryId, string typeId, int startYear)
        {
            string sCounter;
            int iCounter = 0;
            for (int i=0; i<Table.Rows.Count; i++)
            {                
                if ((int)Table.Rows[i].ItemArray[1] == subsidiaryId &&
                    (string)Table.Rows[i].ItemArray[2] == typeId &&
                    (int)Table.Rows[i].ItemArray[3] == startYear)
                {                    
                    iCounter = (int)Table.Rows[i].ItemArray[4] + 1;                    
                    Table.Rows[i][4] = iCounter;                    
                    break;
                }                
            }
            if (iCounter == 0)
            {
                DataRow newRow = Table.NewRow();
                newRow[0] = Table.Rows.Count + 1;
                newRow[1] = subsidiaryId;
                newRow[2] = typeId;
                newRow[3] = startYear;
                newRow[4] = 1;
                Table.Rows.Add(newRow);
                iCounter = 1;
            }           

            sCounter = Convert.ToString(iCounter);
            if (sCounter.Count() == 1)
                sCounter = "00" + sCounter;
            if (sCounter.Count() == 2)
                sCounter = "0" + sCounter;

            return sCounter;
        }
    }
}
