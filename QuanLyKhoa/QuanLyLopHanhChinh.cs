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
    public partial class QuanLyLopHanhChinh : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;
        private bool LoadingData = false;
        public QuanLyLopHanhChinh()
        {
            InitializeComponent();
        }

        private void QuanLyLopHanhChinh_Load(object sender, EventArgs e)
        {
            loadNganh();
            setEnable(false);
        }
        private void loadNganh()
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
            LoadLopTheoNganh(0);
        }
        private void LoadLopTheoNganh(int NG_ID)
        {
            if (LoadingData) return;
            string sql = NG_ID == 0 ? "SELECT * FROM tblLopHanhChinh" : $"SELECT * FROM tblLopHanhChinh WHERE NG_ID = {NG_ID}";
            DataTable dt = db.GetData(sql);

            cboLop.SelectedIndexChanged -= cboLop_SelectedIndexChanged;

            cboLop.DataSource = dt;
            cboLop.DisplayMember = "LP_TenLop";
            cboLop.ValueMember = "LP_ID";

            cboLop.SelectedIndexChanged += cboLop_SelectedIndexChanged;

            this.BeginInvoke(new Action(() =>
            {
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = dt;
            }));
            if (int.TryParse(cboNganh.SelectedValue?.ToString(), out int selectNganh) && selectNganh == 0) return;
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboNganh.SelectedValue == null || cboNganh.SelectedValue is DataRowView) return;
            if (int.TryParse(cboNganh.SelectedValue.ToString(), out int NG_ID))
            {
                LoadLopTheoNganh(NG_ID);
            }
        }
        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboLop.SelectedValue == null || cboLop.SelectedValue is DataRowView) return;
            if (int.TryParse(cboLop.SelectedValue.ToString(), out int LP_ID))
            {
                this.BeginInvoke(new Action(() =>
                {
                    string sql = $"SELECT * FROM tblLopHanhChinh WHERE LP_ID = {LP_ID}";
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
            if (i >= 0 && dgvUsers.Rows[i].Cells["LP_ID"].Value != null)
            {
                LoadingData = true;
                cboLop.SelectedValue = dgvUsers.Rows[i].Cells["LP_ID"].Value;
                if (int.TryParse(cboNganh.SelectedValue?.ToString(), out int selectNganhID) && selectNganhID != 0)
                {
                    cboLop.SelectedValue = dgvUsers.Rows[i].Cells["NG_ID"].Value;
                }
                txtChuNhiem.Text = dgvUsers.Rows[i].Cells["LP_TenChuNhiem"].Value.ToString();
                LoadingData = false;
            }
        }
        private void setEnable(bool enable)
        {
            cboNganh.Enabled = true;
            cboLop.Enabled = true;
            txtChuNhiem.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnExit.Enabled = !enable;

            if (enable && AddNew)
            {
                cboLop.Text = "";
                cboNganh.SelectedIndex = 0;
                txtChuNhiem.Text = "";
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            cboLop.DropDownStyle = ComboBoxStyle.DropDown;
            cboLop.Text = "";
            txtChuNhiem.Text = "";
            setEnable(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string lop = cboLop.Text.Trim();
            string cn = txtChuNhiem.Text.Trim();
            string nganh = cboNganh.SelectedValue?.ToString();
            LoadingData = true;
            if (AddNew)
            {
                string sql = $"INSERT INTO tblLopHanhChinh (LP_TenLop, LP_TenChuNhiem, NG_ID) VALUES (N'{lop}', N'{cn}', N'{nganh}')";
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboLop.SelectedValue?.ToString();
                string sql = $"UPDATE tblLopHanhChinh SET LP_TenLop=N'{lop}', LP_TenChuNhiem=N'{cn}', NG_ID=N'{nganh}' WHERE LP_ID=N'{id}'";
                db.runQuery(sql);
            }
            if (int.TryParse(nganh, out int NG_ID))
                LoadLopTheoNganh(NG_ID);

            setEnable(false);
            LoadingData = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboLop.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            string sql = $"DELETE FROM tblLopHanhChinh WHERE LP_ID=N'{id}'";
            db.runQuery(sql);

            if (int.TryParse(cboLop.SelectedValue.ToString(), out int LP_ID))
                LoadLopTheoNganh(LP_ID);

            dgvUsers.DataSource = null;
            setEnable(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
