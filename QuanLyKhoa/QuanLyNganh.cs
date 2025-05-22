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
    public partial class QuanLyNganh : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;
        public QuanLyNganh()
        {
            InitializeComponent();
        }
        private void QuanLyNganh_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            LoadNganh();
            setEnable(false);
            cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
        }
        private void LoadKhoa()
        {
            string sql = "SELECT * FROM tblKhoa";
            DataTable dt = db.GetData(sql);
            cboKhoa.DataSource = dt;
            cboKhoa.DisplayMember = "K_TenKhoa";
            cboKhoa.ValueMember = "K_ID";
        }
        private void LoadNganh()
        {
            string sql = "SELECT NG_ID, NG_TenNganh, NG_SoTinChi, NG_MoTa, K_TenKhoa, NG.K_ID " +
                "FROM tblNganh NG JOIN tblKhoa K ON NG.K_ID = K.K_ID";
            dgvUsers.DataSource = db.GetData(sql);
        }
        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedValue != null && cboKhoa.SelectedValue is int)
            {
                int HienThiKhoaID = (int)cboKhoa.SelectedValue;
                string sql = string.Format(
                    "SELECT NG_ID, NG_TenNganh, NG_SoTinChi, NG_MoTa, K_TenKhoa, NG.K_ID " +
                    "FROM tblNganh NG JOIN tblKhoa K ON NG.K_ID = K.K_ID " +
                    "WHERE NG.K_ID = {0}", HienThiKhoaID);
                this.BeginInvoke((MethodInvoker)delegate {
                    dgvUsers.DataSource = db.GetData(sql);
                });
            }
        }
        private void setEnable(bool enable)
        {
            cboKhoa.Enabled = true;
            txtNganh.Enabled = enable;
            txtSoTinChi.Enabled = enable;
            txtMoTa.Enabled = enable;
            btnSave.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnExit.Enabled = !enable;
            btnLamMoi.Enabled = !enable;
            btnTimKiem.Enabled = !enable;
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtNganh.Text = dgvUsers.Rows[i].Cells["NG_TenNganh"].Value.ToString();
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["NG_SoTinChi"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NG_MoTa"].Value.ToString();

            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
            txtNganh.Clear();
            txtSoTinChi.Clear();
            txtMoTa.Clear();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string nganh = txtNganh.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string tinchi = txtSoTinChi.Text.Trim();
            int KhoaID = Convert.ToInt32(cboKhoa.SelectedValue);
            if (string.IsNullOrWhiteSpace(nganh) || string.IsNullOrWhiteSpace(tinchi))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblNganh (NG_TenNganh, NG_SoTinChi, NG_MoTa, K_ID)" +
                    "VALUES (N'{0}', N'{1}', N'{2}', {3})", nganh, tinchi, mota, KhoaID);
                db.runQuery(sql);
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NG_ID"].Value);
                    string sql = string.Format("UPDATE tblNganh SET " +
                        "NG_TenNganh=N'{0}', NG_SoTinChi=N'{1}', NG_MoTa=N'{2}', K_ID={3} " +
                        "WHERE NG_ID={4}", nganh, tinchi, mota, KhoaID, id);
                    db.runQuery(sql);
                }
            }
            LoadNganh();
            setEnable(false);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NG_ID"].Value);
                string sql = string.Format("DELETE FROM tblNganh WHERE NG_ID={0}", id);
                db.runQuery(sql);
                LoadNganh();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadNganh();
            txtTimKiem.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenNganh = txtTimKiem.Text.Trim();
            string sql = string.Format("SELECT NG_ID, NG_TenNganh, NG_SoTinChi, NG_MoTa, K_TenKhoa, NG.K_ID " +
                "FROM tblNganh NG JOIN tblKhoa K ON NG.K_ID = K.K_ID");

            if (!string.IsNullOrWhiteSpace(TenNganh))
            {
                sql += string.Format(" WHERE NG_TenNganh = N'{0}'", TenNganh);
            }
            dgvUsers.DataSource = db.GetData(sql);
        }

    }
}
