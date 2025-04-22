using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    internal class DBservices
    {
        private string constr = @"Data Source=LAPTOP-KFK2IU3B\TUANANH2K5;Initial Catalog=QLSV;Integrated Security=True";
        private SqlConnection mysqlConnection;

        public DBservices()
        {
            mysqlConnection = new SqlConnection(constr);
        }
        public DataTable GetData(string sSql)
        {
            try
            {
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(sSql, mysqlConnection);
                DataTable mydataTable = new DataTable();
                myDataAdapter.Fill(mydataTable);
                return mydataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public void runQuery(string sSql)
        {
            try
            {
                mysqlConnection.Open();
                SqlCommand mysqlCommand = new SqlCommand(sSql, mysqlConnection);
                mysqlCommand.ExecuteNonQuery();
                mysqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
