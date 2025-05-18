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
    public partial class QuanLyNamHoc : Form
    {
        private bool AddNew = false;
        public QuanLyNamHoc()
        {
            InitializeComponent();
        }
        private void QuanLyNamHoc_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
       
        private void setEnable(bool enable)
        {
            txtNamHoc.Enabled = enable;
            txtMoTa.Enabled = enable;
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
            string sql = "SELECT * FROM tblNamHoc";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }

        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtNamHoc.Text = dgvUsers.Rows[i].Cells["NH_TenNamHoc"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NH_MoTa"].Value.ToString();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
            txtNamHoc.Clear();
            txtMoTa.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string nh = txtNamHoc.Text;
            string mt = txtMoTa.Text;
            if (string.IsNullOrEmpty(nh))
            {
                MessageBox.Show("Không để trống thông tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamHoc.Focus();
                return;
            }
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblNamHoc (NH_TenNamHoc, NH_MoTa) VALUES " +
                    "(N'{0}', N'{1}')", nh, mt);
                DBservices db = new DBservices();
                db.runQuery(sql);
                LayDuLieu();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NH_ID"].Value);
                    string sql = string.Format("UPDATE tblNamHoc SET " +
                        "NH_TenNamHoc=N'{0}'," +
                        "NH_MoTa=N'{1}' WHERE NH_ID={2}", nh, mt, id);
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
            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NH_ID"].Value);
            string sql = string.Format("DELETE FROM tblNamHoc WHERE NH_ID={0}", id);
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
            string TenNamHoc = txtTimKiem.Text.Trim();
            string sql = string.Format("SELECT * FROM tblNamHoc WHERE NH_TenNamHoc = N'{0}'", TenNamHoc);
            DBservices db = new DBservices();
            dgvUsers.DataSource = db.GetData(sql);
        }
    }
}
