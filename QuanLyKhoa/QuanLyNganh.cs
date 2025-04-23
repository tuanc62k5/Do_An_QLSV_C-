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
        private bool LoadingData = false;
        public QuanLyNganh()
        {
            InitializeComponent();
        }
        private void QuanLyNganh_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            setEnable(false);
        }
        private void LoadKhoa()
        {
            LoadingData = true;
            string sql = "SELECT * FROM tblKhoa";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["K_ID"] = 0;
            row["K_TenKhoa"] = "Tất cả khoa";
            dt.Rows.InsertAt(row, 0);

            cboKhoa.DataSource = dt;
            cboKhoa.DisplayMember = "K_TenKhoa";
            cboKhoa.ValueMember = "K_ID";

            if (cboKhoa.SelectedIndex == -1)
            {
                cboKhoa.SelectedIndex = 0;
            }
            LoadingData = false;
            LoadNganhTheoKhoa(0);
        }
        private void LoadNganhTheoKhoa(int K_ID)
        {
            if (LoadingData) return;
            string sql = K_ID == 0 ? "SELECT * FROM tblNganh" : $"SELECT * FROM tblNganh WHERE K_ID = {K_ID}";
            DataTable dt = db.GetData(sql);

            cboNganh.SelectedIndexChanged -= cboNganh_SelectedIndexChanged;

            cboNganh.DataSource = dt;
            cboNganh.DisplayMember = "NG_TenNganh";
            cboNganh.ValueMember = "NG_ID";

            cboNganh.SelectedIndexChanged += cboNganh_SelectedIndexChanged;

            this.BeginInvoke(new Action(() =>
            {
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = dt;
            }));
            if (int.TryParse(cboKhoa.SelectedValue?.ToString(), out int currentKhoa) && currentKhoa == 0) return;
        }
        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboKhoa.SelectedValue == null || cboKhoa.SelectedValue is DataRowView) return;
            if (int.TryParse(cboKhoa.SelectedValue.ToString(), out int K_ID))
            {
                LoadNganhTheoKhoa(K_ID);
            }
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboNganh.SelectedValue == null || cboNganh.SelectedValue is DataRowView) return;
            if (int.TryParse(cboNganh.SelectedValue.ToString(), out int NG_ID))
            {
                this.BeginInvoke(new Action(() =>
                {
                    string sql = $"SELECT * FROM tblNganh WHERE NG_ID = {NG_ID}";
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
            if (i >= 0 && dgvUsers.Rows[i].Cells["NG_ID"].Value != null)
            {
                LoadingData = true;
                cboNganh.SelectedValue = dgvUsers.Rows[i].Cells["NG_ID"].Value;
                if (int.TryParse(cboKhoa.SelectedValue?.ToString(), out int selectKhoaID) && selectKhoaID != 0)
                {
                    cboKhoa.SelectedValue = dgvUsers.Rows[i].Cells["K_ID"].Value;
                }
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["NG_SoTinChi"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NG_MoTa"].Value.ToString();
                LoadingData = false;
            }
        }
        private void setEnable(bool enable)
        {
            cboKhoa.Enabled = true;
            cboNganh.Enabled = true;
            txtSoTinChi.Enabled = enable;
            txtMoTa.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnExit.Enabled = !enable;

            if (enable && AddNew)
            {
                cboNganh.Text = "";
                cboKhoa.SelectedIndex = 0;
                txtSoTinChi.Text = "";
                txtMoTa.Text = "";
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            cboNganh.DropDownStyle = ComboBoxStyle.DropDown;
            cboNganh.Text = "";
            txtSoTinChi.Text = "";
            txtMoTa.Text = "";
            setEnable(true);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboNganh.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string nganh = cboNganh.Text.Trim();
            string stc = txtSoTinChi.Text.Trim();
            string mt = txtMoTa.Text.Trim();
            string khoa = cboKhoa.SelectedValue?.ToString();
            LoadingData = true;
            if (AddNew)
            {
                string sql = $"INSERT INTO tblNganh (NG_TenNganh, NG_SoTinChi, NG_MoTa, K_ID) VALUES (N'{nganh}', N'{stc}', N'{mt}', N'{khoa}')";
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboNganh.SelectedValue?.ToString();
                string sql = $"UPDATE tblNganh SET NG_TenNganh=N'{nganh}', NG_SoTinChi=N'{stc}', NG_MoTa=N'{mt}', K_ID=N'{khoa}' WHERE NG_ID=N'{id}'";
                db.runQuery(sql);
            }
            if (int.TryParse(khoa, out int K_ID))
                LoadNganhTheoKhoa(K_ID);

            setEnable(false);
            LoadingData = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboNganh.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            string sql = $"DELETE FROM tblNganh WHERE NG_ID=N'{id}'";
            db.runQuery(sql);

            if (int.TryParse(cboKhoa.SelectedValue.ToString(), out int K_ID))
                LoadNganhTheoKhoa(K_ID);

            dgvUsers.DataSource = null;
            setEnable(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
