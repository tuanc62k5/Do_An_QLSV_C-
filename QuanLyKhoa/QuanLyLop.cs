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
    public partial class QuanLyLop : Form
    {
        bool AddNew = false;
        public QuanLyLop()
        {
            InitializeComponent();
        }

        private void QuanLyLop_Load(object sender, EventArgs e)
        {
            loadgriddata();
            loadNganh();
            loadKhoaHoc();
            loadLop();
            cboMaLop.SelectedIndexChanged += cboMaLop_SelectedIndexChanged;
            cboMaNganh.SelectedIndexChanged += cboMaNganh_SelectedIndexChanged;
            cboMaKhoaHoc.SelectedIndexChanged += cboMaKhoaHoc_SelectedIndexChanged;
        }
        private void loadNganh()
        {
            DBservices db = new DBservices();
            DataTable dt = db.GetData("SELECT MaNganh, TenNganh from tblNganh");
            cboMaNganh.DataSource = dt;
            cboMaNganh.DisplayMember = "TenNganh";
            cboMaNganh.ValueMember = "MaNganh";
        }
        private void loadKhoaHoc()
        {
            DBservices db = new DBservices();
            DataTable dt = db.GetData("SELECT MaKhoaHoc, TenKhoaHoc FROM tblKhoaHoc");
            cboMaKhoaHoc.DataSource = dt;
            cboMaKhoaHoc.DisplayMember = "TenKhoaHoc";
            cboMaKhoaHoc.ValueMember = "MaKhoaHoc";
        }
        private void loadLop()
        {
            DBservices db = new DBservices();
            DataTable dt = db.GetData("SELECT MaLop, TenLop FROM tblLop");
            cboMaLop.DataSource = dt;
            cboMaLop.DisplayMember = "TenLop";
            cboMaLop.ValueMember = "MaLop";
            cboMaLop.DropDownStyle = ComboBoxStyle.DropDown;
        }
        private void cboMaLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaLop.SelectedItem is DataRowView drv)
            {
                txtTenLop.Text = drv["TenLop"].ToString();
            }
        }
        private void cboMaNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void cboMaKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void setEnable(bool check)
        {
            if (AddNew)
            {
                cboMaLop.Enabled = true;
            }
            else
            {
                cboMaLop.Enabled = false;
            }
            txtTenLop.Enabled = check;
            cboMaNganh.Enabled = check;
            cboMaKhoaHoc.Enabled = check;
            txtTenChuNhiem.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }
        private void loadgriddata()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblLop";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }

        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           int i = e.RowIndex;
           if (i >= 0)
           {
                cboMaLop.SelectedValue = dgvUsers.Rows[i].Cells["MaLop"].Value.ToString();
                txtTenLop.Text = dgvUsers.Rows[i].Cells["TenLop"].Value.ToString();
                cboMaNganh.SelectedValue = dgvUsers.Rows[i].Cells["MaNganh"].Value.ToString();
                cboMaKhoaHoc.SelectedValue = dgvUsers.Rows[i].Cells["MaKhoaHoc"].Value.ToString();
                txtTenChuNhiem.Text = dgvUsers.Rows[i].Cells["TenChuNhiem"].Value.ToString();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboMaLop.DataSource = null;
            cboMaLop.Text = "";
            txtTenLop.Text = "";
            cboMaNganh.Text = "";
            cboMaKhoaHoc.Text = "";
            txtTenChuNhiem.Text = "";
            //cho phép nhập tay:
            cboMaLop.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaKhoaHoc.DropDownStyle = ComboBoxStyle.DropDown;
            AddNew = true;
            setEnable(true);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboMaLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            loadLop();
            loadNganh();
            loadKhoaHoc();
            setEnable(true);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string MaLop = cboMaLop.SelectedValue?.ToString() ?? "";
            string sql = string.Format("DELETE FROM tblKhoa WHERE MaLop=N'{0}'", MaLop);
            DBservices db = new DBservices();
            db.runQuery(sql);
            loadgriddata();
            loadLop();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string ml = cboMaLop.Text.Trim();
            string tl = txtTenLop.Text.Trim();
            string mn = cboMaNganh.SelectedValue?.ToString() ?? "";
            string mkh = cboMaKhoaHoc.SelectedValue?.ToString() ?? "";
            string tcn = txtTenChuNhiem.Text.Trim();
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblLop (MaLop, TenLop, MaNganh, MaKhoaHoc, TenChuNhiem) VALUES " + "(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}')", ml, tl, mn, mkh, tcn);
                db.runQuery(sql);
                AddNew = false;
                loadgriddata();
                loadLop();
            }
            else
            {
                string sql = string.Format("UPDATE tblLop SET TenLop=N'{1}', MaNganh=N'{2}', MaKhoaHoc=N'{3}', TenChuNhiem=N'{4}' WHERE MaLop=N'{0}'", ml, tl, mn, mkh, tcn);
                db.runQuery(sql);
                loadgriddata();
                loadLop();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void quảnLíKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyKhoa f = new QuanLyKhoa();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíNgànhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyNganh f = new QuanLyNganh();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíHọcPhầnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyHocPhan f = new QuanLyHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyHocPhan f = new QuanLyHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
