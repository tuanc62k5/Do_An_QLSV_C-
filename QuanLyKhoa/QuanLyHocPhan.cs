using System;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    public partial class QuanLyHocPhan : Form
    {
        bool AddNew = false;
        public QuanLyHocPhan()
        {
            InitializeComponent();
        }

        private void QuanLyHocPhan_Load(object sender, EventArgs e)
        {
            loadgriddata();
            loadNganh();
            cboMaHocPhan.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void loadNganh()
        {
            DBservices db = new DBservices();
            string sql = "SELECT MaNganh, TenNganh FROM tblNganh";
            cboMaNganh.DataSource = db.GetData(sql);
            cboMaNganh.DisplayMember = "TenNganh";
            cboMaNganh.ValueMember = "MaNganh";
        }
        private void setEnable(bool check)
        {
            if (AddNew)
            {
                cboMaHocPhan.Enabled = true;
            }
            else
            {
                cboMaHocPhan.Enabled = false;
            }
            txtTenHocPhan.Enabled = check;
            cboMaNganh.Enabled = check;
            txtSoTinChi.Enabled = check;
            txtSoTietLyThuyet.Enabled = check;
            txtSoTietThucHanh.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
            if (!check)
            {
                cboMaHocPhan.DropDownStyle = ComboBoxStyle.DropDownList;
                cboMaNganh.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
        private void loadgriddata()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblHocPhan";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cboMaHocPhan.Text = dgvUsers.Rows[i].Cells["MaHocPhan"].Value.ToString();
                txtTenHocPhan.Text = dgvUsers.Rows[i].Cells["TenHocPhan"].Value.ToString();
                cboMaNganh.Text = dgvUsers.Rows[i].Cells["MaNganh"].Value.ToString();
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["SoTinChi"].Value.ToString();
                txtSoTietLyThuyet.Text = dgvUsers.Rows[i].Cells["SoTietLyThuyet"].Value.ToString();
                txtSoTietThucHanh.Text = dgvUsers.Rows[i].Cells["SoTietThucHanh"].Value.ToString();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string mhp = cboMaHocPhan.Text;
            string thp = txtTenHocPhan.Text;
            string mn = cboMaNganh.SelectedValue?.ToString() ?? "";
            string stc = txtSoTinChi.Text;
            string stlt = txtSoTietLyThuyet.Text;
            string stth = txtSoTietThucHanh.Text;
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblHocPhan (MaHocPhan, TenHocPhan, MaNganh, SoTinChi, SoTietLyThuyet, SoTietThucHanh) VALUES " + "(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}')", mhp, thp, mn, stc, stlt, stth);
                db.runQuery(sql);
                AddNew = false;
                loadgriddata();
            }
            else
            {
                string sql = string.Format("UPDATE tblHocPhan SET TenHocPhan=N'{1}', MaNganh=N'{2}', SoTinChi=N'{3}', SoTietLyThuyet=N'{4}', SoTietThucHanh=N'{5}' WHERE MaHocPhan=N'{0}'", mhp, thp, mn, stc, stlt, stth);
                db.runQuery(sql);
                loadgriddata();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
            cboMaHocPhan.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string MaHocPhan = cboMaHocPhan.Text;
            string sql = string.Format("DELETE FROM tblHocPhan WHERE MaHocPhan=N'{0}'", MaHocPhan);
            DBservices db = new DBservices();
            db.runQuery(sql);
            loadgriddata();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboMaHocPhan.Text = "";
            txtTenHocPhan.Text = "";
            cboMaNganh.Text = "";
            txtSoTinChi.Text = "";
            txtSoTietLyThuyet.Text = "";
            txtSoTietThucHanh.Text = "";
            AddNew = true;
            setEnable(true);
            cboMaHocPhan.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDown;
        }

        private void quảnLíSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLySinhVien f = new QuanLySinhVien();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
        private void quảnLíKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyKhoa f = new QuanLyKhoa();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyLop f = new QuanLyLop();
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
    }

}