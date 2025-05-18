using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    public partial class GiaoDien : Form
    {
        DBservices db = new DBservices();
        private string QuyenHan;
        private int SVID;
        public GiaoDien(string QuyenHan, int SVID)
        {
            InitializeComponent();
            this.QuyenHan = QuyenHan;
            this.SVID = SVID;
        }
        public GiaoDien()
        {
            InitializeComponent();
        }
        
        private void GiaoDien_Load(object sender, EventArgs e)
        {

        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string TenDangNhap = txtTenDangNhap.Text.Trim();
            string MatKhau = txtMatKhau.Text.Trim();
            if(string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(MatKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                return;
            }
            string sql = $"SELECT * FROM tblDangNhap WHERE TK_TenDangNhap = N'{TenDangNhap}' AND TK_MatKhau = N'{MatKhau}'";
            DataTable dt = db.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                string QuyenHan = dt.Rows[0]["TK_QuyenHan"].ToString();
                int svID = Convert.ToInt32(dt.Rows[0]["SV_ID"]);
                QuanLySinhVien f = new QuanLySinhVien(QuyenHan, SVID);
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
        }
    }
}
