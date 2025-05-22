using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyKhoa
{
    public partial class QuanLyKhoa : Form
    {
        private bool AddNew = false;
        public QuanLyKhoa()
        {
            InitializeComponent();
        }

        private void QuanLyKhoa_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void setEnable(bool check)
        {
            txtKhoa.Enabled = check;
            txtTruongKhoa.Enabled = check;
            txtDiaChi.Enabled = check;
            txtDienThoai.Enabled = check;
            txtEmail.Enabled = check;
            btnSave.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnExit.Enabled = !check;
            btnLamMoi.Enabled = !check;
            btnTimKiem.Enabled = !check;
        }
        private void LayDuLieu()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblKhoa";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtKhoa.Text = dgvUsers.Rows[i].Cells["K_TenKhoa"].Value.ToString();
                txtTruongKhoa.Text = dgvUsers.Rows[i].Cells["K_TenTruongKhoa"].Value.ToString();
                txtDiaChi.Text = dgvUsers.Rows[i].Cells["K_DiaChi"].Value.ToString();
                txtDienThoai.Text = dgvUsers.Rows[i].Cells["K_DienThoai"].Value.ToString();
                txtEmail.Text = dgvUsers.Rows[i].Cells["K_Email"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
            txtKhoa.Clear();
            txtTruongKhoa.Clear();
            txtDiaChi.Clear();
            txtDienThoai.Clear();
            txtEmail.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string khoa = txtKhoa.Text;
            string tk = txtTruongKhoa.Text;
            string dc = txtDiaChi.Text;
            string dt = txtDienThoai.Text;
            string em = txtEmail.Text;
            if (string.IsNullOrEmpty(khoa) )
            {
                MessageBox.Show("Không để trống thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhoa.Focus();
                return;
            }

            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblKhoa (K_TenKhoa, K_TenTruongKhoa, K_DiaChi, K_DienThoai, K_Email) VALUES " +
                    "(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}')", khoa, tk, dc, dt, em);
                DBservices db = new DBservices();
                db.runQuery(sql);
                LayDuLieu();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["K_ID"].Value);
                    string sql = string.Format("UPDATE tblKhoa SET " +
                        "K_TenKhoa=N'{0}'," +
                        "K_TenTruongKhoa=N'{1}'," +
                        "K_DiaChi=N'{2}'," +
                        "K_DienThoai=N'{3}'," +
                        "K_Email=N'{4}' WHERE K_ID={5}", khoa, tk, dc, dt, em, id);
                    DBservices db = new DBservices();
                    db.runQuery(sql);
                    LayDuLieu();
                } 
            }

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["K_ID"].Value);
            string sql = string.Format("DELETE FROM tblKhoa WHERE K_ID={0}", id);
            DBservices db = new DBservices();
            db.runQuery(sql);
            LayDuLieu();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LayDuLieu();
            txtTimKiem.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenKhoa = txtTimKiem.Text.Trim();
            string sql = string.Format("SELECT * FROM tblKhoa WHERE K_TenKhoa = N'{0}'", TenKhoa);
            DBservices db = new DBservices();
            dgvUsers.DataSource = db.GetData(sql);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtKhoa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
