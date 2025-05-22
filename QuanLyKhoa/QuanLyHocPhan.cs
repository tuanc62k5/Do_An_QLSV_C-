using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    public partial class QuanLyHocPhan : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;
        public QuanLyHocPhan()
        {
            InitializeComponent();
        }

        private void QuanLyHocPhan_Load(object sender, EventArgs e)
        {
            LoadNganh();
            LoadHocPhan();
            setEnable(false);
            cboNganh.SelectedIndexChanged += cboNganh_SelectedIndexChanged;
        }
        private void LoadNganh()
        {
            string sql = "SELECT * FROM tblNganh";
            DataTable dt = db.GetData(sql);
            cboNganh.DataSource = dt;
            cboNganh.DisplayMember = "NG_TenNganh";
            cboNganh.ValueMember = "NG_ID";
        }

        private void LoadHocPhan()
        {
            string sql = "SELECT HP_ID, HP_TenHocPhan, HP_SoTinChi, HP_SoTietLyThuyet, HP_SoTietThucHanh, HP_MoTa, NG_TenNganh, HP.NG_ID " +
                         "FROM tblHocPhan HP JOIN tblNganh NG ON HP.NG_ID = NG.NG_ID";
            dgvUsers.DataSource = db.GetData(sql);
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNganh.SelectedValue != null && cboNganh.SelectedValue is int)
            {
                int HienThiNganhID = (int)cboNganh.SelectedValue;
                string sql = string.Format(
                    "SELECT HP_ID, HP_TenHocPhan, HP_SoTinChi, HP_SoTietLyThuyet, HP_SoTietThucHanh, NG_TenNganh, NG.NG_ID " +
                    "FROM tblHocPhan HP JOIN tblNganh NG ON HP.NG_ID = NG.NG_ID " +
                    "WHERE HP.NG_ID = {0}", HienThiNganhID);
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    dgvUsers.DataSource = db.GetData(sql);
                }));
            }
        }
        private void setEnable(bool enable)
        {
            cboNganh.Enabled = true;
            txtHocPhan.Enabled = enable;
            txtSoTinChi.Enabled = enable;
            txtSoTietLyThuyet.Enabled = enable;
            txtSoTietThucHanh.Enabled = enable;
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
                txtHocPhan.Text = dgvUsers.Rows[i].Cells["HP_TenHocPhan"].Value.ToString();
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["HP_SoTinChi"].Value.ToString();
                txtSoTietLyThuyet.Text = dgvUsers.Rows[i].Cells["HP_SoTietLyThuyet"].Value.ToString();
                txtSoTietThucHanh.Text = dgvUsers.Rows[i].Cells["HP_SoTietThucHanh"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["HP_MoTa"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
            txtHocPhan.Clear();
            txtSoTinChi.Clear();
            txtSoTietLyThuyet.Clear();
            txtSoTietThucHanh.Clear();
            txtMoTa.Clear();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string hocphan = txtHocPhan.Text.Trim();
            string stc = txtSoTinChi.Text.Trim();
            string stlt = txtSoTietLyThuyet.Text.Trim();
            string stth = txtSoTietThucHanh.Text.Trim();
            string mt = txtMoTa.Text.Trim();
            int NganhID = Convert.ToInt32(cboNganh.SelectedValue);

            if (string.IsNullOrWhiteSpace(hocphan) || string.IsNullOrWhiteSpace(stc))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql;
            if (AddNew)
            {
                sql = string.Format("INSERT INTO tblHocPhan (HP_TenHocPhan, HP_SoTinChi, HP_SoTietLyThuyet, HP_SoTietThucHanh, HP_MoTa, NG_ID) " +
                                    "VALUES (N'{0}', {1}, {2}, {3}, N'{4}', {5})", hocphan, stc, stlt, stth, mt, NganhID);
            }
            else
            {
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["HP_ID"].Value);
                sql = string.Format("UPDATE tblHocPhan SET HP_TenHocPhan = N'{0}', HP_SoTinChi = {1}, HP_SoTietLyThuyet = {2}, " +
                                    "HP_SoTietThucHanh = {3}, HP_MoTa = N'{4}', NG_ID = {5} WHERE HP_ID = {6}",
                                    hocphan, stc, stlt, stth, mt, NganhID, id);
            }

            db.runQuery(sql);
            LoadHocPhan();
            setEnable(false);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["HP_ID"].Value);
                string sql = string.Format("DELETE FROM tblHocPhan WHERE HP_ID = {0}", id);
                db.runQuery(sql);
                LoadHocPhan();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadHocPhan();
            txtTimKiem.Clear();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenHocPhan = txtTimKiem.Text.Trim();
            string sql = "SELECT HP_ID, HP_TenHocPhan, HP_SoTinChi, HP_SoTietLyThuyet, HP_SoTietThucHanh, HP_MoTa, NG_TenNganh, HP.NG_ID " +
                         "FROM tblHocPhan HP JOIN tblNganh NG ON HP.NG_ID = NG.NG_ID";

            if (!string.IsNullOrWhiteSpace(TenHocPhan))
            {
                sql += string.Format(" WHERE HP_TenHocPhan = N'{0}'", TenHocPhan);
            }

            dgvUsers.DataSource = db.GetData(sql);
        }
    }

}