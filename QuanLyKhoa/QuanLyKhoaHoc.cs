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
        DBservices db = new DBservices();
        private bool AddNew = false;
        public QuanLyKhoaHoc()
        {
            InitializeComponent();
        }

        private void QuanLyKhoaHoc_Load(object sender, EventArgs e)
        {
            LayKhoaHoc();
            SelectedKhoaHoc();
            cboKhoaHoc.SelectedIndexChanged += cboKhoaHoc_SelectedIndexChanged;
        }
        private void LayKhoaHoc()
        {
            string sql = "SELECT * FROM tblKhoaHoc";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["KH_ID"] = 0;
            row["KH_TenKhoaHoc"] = "Tất cả Khóa học";
            dt.Rows.InsertAt(row, 0);

            cboKhoaHoc.DisplayMember = "KH_TenKhoaHoc";
            cboKhoaHoc.ValueMember = "KH_ID";
            cboKhoaHoc.DataSource = dt;
            cboKhoaHoc.SelectedIndex = 0;
        }
        private void SelectedKhoaHoc()
        {
            if (cboKhoaHoc.SelectedValue == null) return;

            if (int.TryParse(cboKhoaHoc.SelectedValue.ToString(), out int id))
            {
                string sql = string.Format(id == 0 ? "SELECT * FROM tblKhoaHoc" : "SELECT * FROM tblKhoaHoc WHERE KH_ID = {0}", id);
                dgvUsers.DataSource = db.GetData(sql);
            }
        }
        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                SelectedKhoaHoc();
            }));
        }
        private void setEnable(bool check)
        {
            cboKhoaHoc.Enabled = true;
            txtNamBatDau.Enabled = check;
            txtNamKetThuc.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0 && dgvUsers.Rows[i].Cells["KH_ID"].Value != null)
            {
                int rowKhoaHocID = Convert.ToInt32(dgvUsers.Rows[i].Cells["KH_ID"].Value);
                if (cboKhoaHoc.SelectedValue != null && int.TryParse(cboKhoaHoc.SelectedValue.ToString(), out int selectKhoaHocID) && selectKhoaHocID != 0)
                {
                    cboKhoaHoc.SelectedValue = rowKhoaHocID;
                }
                txtNamBatDau.Text = dgvUsers.Rows[i].Cells["KH_NamBatDau"].Value.ToString();
                txtNamKetThuc.Text = dgvUsers.Rows[i].Cells["KH_NamKetThuc"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboKhoaHoc.DataSource = null;
            cboKhoaHoc.Text = "";
            cboKhoaHoc.DropDownStyle = ComboBoxStyle.DropDown;
            txtNamBatDau.Text = "";
            txtNamKetThuc.Text = "";
            AddNew = true;
            setEnable(true);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string tkh = cboKhoaHoc.Text.Trim();
            string nbd = txtNamBatDau.Text.Trim();
            string nkt = txtNamKetThuc.Text.Trim();
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblKhoaHoc (KH_TenKhoaHoc, KH_NamBatDau, KH_NamKetThuc) " + "VALUES (N'{0}', N'{1}', N'{2}')", tkh, nbd, nkt);
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboKhoaHoc.SelectedValue?.ToString() ?? "";
                string sql = string.Format("UPDATE tblKhoaHoc SET KH_TenKhoaHoc=N'{1}', KH_NamBatDau=N'{2}', KH_NamKetThuc=N'{3}'" + "WHERE KH_ID='{0}'", id, tkh, nbd, nkt);
                db.runQuery(sql);
            }
            AddNew = false;
            LayKhoaHoc();
            cboKhoaHoc.SelectedIndex = 0;
            SelectedKhoaHoc();
            setEnable(false);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            LayKhoaHoc();
            setEnable(true);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboKhoaHoc.SelectedValue.ToString();
            string sql = string.Format("DELETE FROM tblKhoaHoc WHERE KH_ID=N'{0}'", id);
            db.runQuery(sql);
            LayKhoaHoc();
            cboKhoaHoc.SelectedIndex = 0;
            SelectedKhoaHoc();
            setEnable(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
