using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    public partial class QuanLyHocPhan : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;
        private bool LoadingData = false;
        public QuanLyHocPhan()
        {
            InitializeComponent();
        }

        private void QuanLyHocPhan_Load(object sender, EventArgs e)
        {
            LoadNganh();
            setEnable(false);
        }
        private void LoadNganh()
        {
            LoadingData = true;
            string sql = "SELECT * FROM tblNganh";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["NG_ID"] = 0;
            row["NG_TenNganh"] = "Tất cả Nganh";
            dt.Rows.InsertAt(row, 0);

            cboNganh.DataSource = dt;
            cboNganh.DisplayMember = "NG_TenNganh";
            cboNganh.ValueMember = "NG_ID";

            if (cboNganh.SelectedIndex == -1)
            {
                cboNganh.SelectedIndex = 0;
            }
            LoadingData = false;
            LoadHocPhanTheoNganh(0);
        }
        private void LoadHocPhanTheoNganh(int NG_ID)
        {
            if (LoadingData) return;
            string sql = NG_ID == 0 ? "SELECT * FROM tblHocPhan" : $"SELECT * FROM tblHocPhan WHERE NG_ID = {NG_ID}";
            DataTable dt = db.GetData(sql);

            cboHocPhan.SelectedIndexChanged -= cboHocPhan_SelectedIndexChanged;

            cboHocPhan.DataSource = dt;
            cboHocPhan.DisplayMember = "HP_TenHocPhan";
            cboHocPhan.ValueMember = "HP_ID";

            cboHocPhan.SelectedIndexChanged += cboHocPhan_SelectedIndexChanged;

            this.BeginInvoke(new Action(() =>
            {
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = dt;
            }));
            if (int.TryParse(cboNganh.SelectedValue?.ToString(), out int currentNganh) && currentNganh == 0) return;
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboNganh.SelectedValue == null || cboNganh.SelectedValue is DataRowView) return;
            if (int.TryParse(cboNganh.SelectedValue.ToString(), out int NG_ID))
            {
                LoadHocPhanTheoNganh(NG_ID);
            }
        }
        private void cboHocPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboHocPhan.SelectedValue == null || cboHocPhan.SelectedValue is DataRowView) return;
            if (int.TryParse(cboHocPhan.SelectedValue.ToString(), out int HP_ID))
            {
                this.BeginInvoke(new Action(() =>
                {
                    string sql = $"SELECT * FROM tblHocPhan WHERE HP_ID = {HP_ID}";
                    DataTable dt = db.GetData(sql);
                    dgvUsers.DataSource = null;
                    dgvUsers.DataSource = dt;
                }));
            }
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (LoadingData) return;
            int i = e.RowIndex;
            if (i >= 0 && dgvUsers.Rows[i].Cells["HP_ID"].Value != null)
            {
                LoadingData = true;
                cboHocPhan.SelectedValue = dgvUsers.Rows[i].Cells["HP_ID"].Value;
                if (int.TryParse(cboHocPhan.SelectedValue?.ToString(), out int selectHocPhanID) && selectHocPhanID != 0)
                {
                    cboNganh.SelectedValue = dgvUsers.Rows[i].Cells["NG_ID"].Value;
                }
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["HP_SoTinChi"].Value.ToString();
                txtSoTietLyThuyet.Text = dgvUsers.Rows[i].Cells["HP_SoTietLyThuyet"].Value.ToString();
                txtSoTietThucHanh.Text = dgvUsers.Rows[i].Cells["HP_SoTietThucHanh"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["HP_MoTa"].Value.ToString();
                LoadingData = false;
            }
        }
        private void setEnable(bool enable)
        {
            cboNganh.Enabled = true;
            cboHocPhan.Enabled = true;
            txtSoTinChi.Enabled = enable;
            txtSoTietLyThuyet.Enabled = enable;
            txtSoTietThucHanh.Enabled = enable;
            txtMoTa.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnExit.Enabled = !enable;

            if (enable && AddNew)
            {
                cboHocPhan.Text = "";
                cboNganh.SelectedIndex = 0;
                txtSoTinChi.Enabled = enable;
                txtSoTietLyThuyet.Enabled = enable;
                txtSoTietThucHanh.Enabled = enable;
                txtMoTa.Enabled = enable;
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            cboHocPhan.DropDownStyle = ComboBoxStyle.DropDown;
            cboHocPhan.Text = "";
            txtSoTinChi.Text = "";
            txtSoTietLyThuyet.Text = "";
            txtSoTietThucHanh.Text = "";
            txtMoTa.Text = "";
            setEnable(true);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboHocPhan.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string hocphan = cboHocPhan.Text.Trim();
            string stc = txtSoTinChi.Text.Trim();
            string stlt = txtSoTietLyThuyet.Text.Trim();
            string stth = txtSoTietThucHanh.Text.Trim();
            string mt = txtMoTa.Text.Trim();
            string nganh = cboNganh.SelectedValue?.ToString();
            LoadingData = true;
            if (AddNew)
            {
                string sql = $"INSERT INTO tblHocPhan (HP_TenHocPhan, HP_SoTinChi, HP_SoTietLyThuyet, HP_SoTietThucHanh, HP_MoTa, NG_ID) VALUES (N'{hocphan}', N'{stc}', N'{stlt}', N'{stth}', N'{mt}', N'{nganh}')";
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboHocPhan.SelectedValue?.ToString();
                string sql = $"UPDATE tblHocPhan SET HP_TenHocPhan=N'{hocphan}', HP_SoTinChi=N'{stc}', HP_SoTietLyThuyet=N'{stlt}', HP_SoTietThucHanh=N'{stth}', HP_MoTa=N'{mt}', NG_ID=N'{nganh}' WHERE HP_ID=N'{id}'";
                db.runQuery(sql);
            }
            if (int.TryParse(nganh, out int NG_ID))
                LoadHocPhanTheoNganh(NG_ID);

            setEnable(false);
            LoadingData = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboHocPhan.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            string sql = $"DELETE FROM tblHocPhan WHERE HP_ID=N'{id}'";
            db.runQuery(sql);

            if (int.TryParse(cboHocPhan.SelectedValue.ToString(), out int NG_ID))
                LoadHocPhanTheoNganh(NG_ID);

            dgvUsers.DataSource = null;
            setEnable(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}