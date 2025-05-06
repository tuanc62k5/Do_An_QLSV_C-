using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace QuanLyKhoa
{
    internal class DBservices
    {
        private string constr = @"Data Source=LAPTOP-KFK2IU3B\TUANANH2K5;Initial Catalog=QLSV;Integrated Security=True";

        public DataTable GetData(string sSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sSql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu: " + ex.Message);
                return null;
            }
        }
        public void runQuery(string sSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực thi câu lệnh: " + ex.Message);
            }
        }
    }
}
