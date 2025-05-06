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
        DBservices db = new DBservices();
        private bool AddNew = false;
        public QuanLyNamHoc()
        {
            InitializeComponent();
        }

        private void QuanLyNamHoc_Load(object sender, EventArgs e)
        {
            LayNamHoc();
            SelectedNamHoc();
            cboNamHoc.SelectedIndexChanged += cboNamHoc_SelectedIndexChanged;
        }
        private void LayNamHoc()
        {
            string sql = "SELECT * FROM tblNamHoc";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["NH_ID"] = 0;
            row["NH_TenNamHoc"] = "Tất cả Năm Học";
            dt.Rows.InsertAt(row, 0);

            cboNamHoc.DisplayMember = "NH_TenNamHoc";
            cboNamHoc.ValueMember = "NH_ID";
            cboNamHoc.DataSource = dt;
            cboNamHoc.SelectedIndex = 0;
        }
        private void SelectedNamHoc()
        {
            if (cboNamHoc.SelectedValue == null) return;

            if (int.TryParse(cboNamHoc.SelectedValue.ToString(), out int id))
            {
                string sql = string.Format(id == 0 ? "SELECT * FROM tblNamHoc" : "SELECT * FROM tblNamHoc WHERE NH_ID = {0}", id);
                dgvUsers.DataSource = db.GetData(sql);
            }
        }
        private void cboNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                SelectedNamHoc();
            }));
        }
        private void setEnable(bool check)
        {
            cboNamHoc.Enabled = true;
            txtMoTa.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0 && dgvUsers.Rows[i].Cells["NH_ID"].Value != null)
            {
                int rowNamHocID = Convert.ToInt32(dgvUsers.Rows[i].Cells["NH_ID"].Value);
                if (cboNamHoc.SelectedValue != null && int.TryParse(cboNamHoc.SelectedValue.ToString(), out int selectNamHocID) && selectNamHocID != 0)
                {
                    cboNamHoc.SelectedValue = rowNamHocID;
                }
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NH_MoTa"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboNamHoc.DataSource = null;
            cboNamHoc.Text = "";
            cboNamHoc.DropDownStyle = ComboBoxStyle.DropDown;
            txtMoTa.Text = "";
            AddNew = true;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nh = cboNamHoc.Text.Trim();
            string mt = txtMoTa.Text.Trim();
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblNamHoc (NH_TenNamHoc, NH_MoTa) " + "VALUES (N'{0}', N'{1}')", nh, mt);
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboNamHoc.SelectedValue?.ToString() ?? "";
                string sql = string.Format("UPDATE tblNamHoc SET NH_TenNamHoc=N'{1}', NH_MoTa=N'{2}'" + "WHERE NH_ID='{0}'", id, nh, mt);
                db.runQuery(sql);
            }
            AddNew = false;
            LayNamHoc();
            cboNamHoc.SelectedIndex = 0;
            SelectedNamHoc();
            setEnable(false);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboNamHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            LayNamHoc();
            setEnable(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboNamHoc.SelectedValue.ToString();
            string sql = string.Format("DELETE FROM tblNamHoc WHERE NH_ID=N'{0}'", id);
            db.runQuery(sql);
            LayNamHoc();
            cboNamHoc.SelectedIndex = 0;
            SelectedNamHoc();
            setEnable(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
