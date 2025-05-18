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

namespace QuanLyKhoa
{
    public partial class QuanLyKhoaHoc : Form
    {
        private bool AddNew = false;
        public QuanLyKhoaHoc()
        {
            InitializeComponent();
        }
        private void QuanLyKhoaHoc_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void setEnable(bool enable)
        {
            txtKhoaHoc.Enabled = enable;
            txtNamBatDau.Enabled = enable;
            txtNamKetThuc.Enabled = enable;
            btnSave.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnExit.Enabled = !enable;
            btnLamMoi.Enabled = !enable;
            btnTimKiem.Enabled = !enable;
        }
        private void LayDuLieu()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblKhoaHoc";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtKhoaHoc.Text = dgvUsers.Rows[i].Cells["KH_TenKhoaHoc"].Value.ToString();
                txtNamBatDau.Text = dgvUsers.Rows[i].Cells["KH_NamBatDau"].Value.ToString();
                txtNamKetThuc.Text = dgvUsers.Rows[i].Cells["KH_NamKetThuc"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
            txtKhoaHoc.Clear();
            txtNamBatDau.Clear();
            txtNamKetThuc.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string kh = txtKhoaHoc.Text;
            string nbd = txtNamBatDau.Text;
            string nkt = txtNamKetThuc.Text;
            if (string.IsNullOrEmpty(kh))
            {
                MessageBox.Show("Không để trống thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKhoaHoc.Focus();
                return;
            }
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblKhoaHoc (KH_TenKhoaHoc, KH_NamBatDau, KH_NamKetThuc) VALUES " +
                    "(N'{0}', N'{1}', N'{2}')", kh, nbd, nkt);
                DBservices db = new DBservices();
                db.runQuery(sql);
                LayDuLieu();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["KH_ID"].Value);
                    string sql = string.Format("UPDATE tblKhoaHoc SET " +
                        "KH_TenKhoaHoc=N'{0}'," +
                        "KH_NamBatDau=N'{1}'," +
                        "KH_NamKetThuc=N'{2}' WHERE KH_ID={3}", kh, nbd, nkt, id);
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
            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["KH_ID"].Value);
            string sql = string.Format("DELETE FROM tblKhoaHoc WHERE KH_ID={0}", id);
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
            string TenKhoaHoc = txtTimKiem.Text.Trim();
            string sql = string.Format("SELECT * FROM tblKhoaHoc WHERE KH_TenKhoaHoc = N'{0}'", TenKhoaHoc);
            DBservices db = new DBservices();
            dgvUsers.DataSource = db.GetData(sql);
        }
    }
}
