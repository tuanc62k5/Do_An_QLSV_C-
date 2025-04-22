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
    public partial class QuanLySinhVien : Form
    {
        bool AddNew = false;
        public QuanLySinhVien()
        {
            InitializeComponent();
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
        private void loadgriddata()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblSinhVien";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void QuanLySinhVien_Load(object sender, EventArgs e)
        {
            loadgriddata();
            timeNgaySinh.Format = DateTimePickerFormat.Custom;
            timeNgaySinh.CustomFormat = "dd/MM/yyyy";
            DBservices db = new DBservices();
            cboMaKhoaHoc.DataSource = db.GetData("SELECT KH_ID, KH_TenKhoaHoc FROM tblKhoaHoc");
            cboMaKhoaHoc.DisplayMember = "KH_TenKhoaHoc";
            cboMaKhoaHoc.ValueMember = "KH_ID";
            cboMaKhoaHoc.SelectedIndex = -1;

            cboMaNganh.DataSource = db.GetData("SELECT NG_ID, NG_TenNganh FROM tblNganh");
            cboMaNganh.DisplayMember = "NG_TenNganh";
            cboMaNganh.ValueMember = "NG_ID";
            cboMaNganh.SelectedIndex = -1;

            cboMaLop.DataSource = db.GetData("SELECT LP_ID, LP_TenLop FROM tblLopHanhChinh");
            cboMaLop.DisplayMember = "LP_TenLop";
            cboMaLop.ValueMember = "LP_ID";
            cboMaLop.SelectedIndex = -1;

            cboMaKhoa.DataSource = db.GetData("SELECT K_ID, K_TenKhoa FROM tblKhoa");
            cboMaKhoa.DisplayMember = "K_TenKhoa";
            cboMaKhoa.ValueMember = "K_ID";
            cboMaKhoa.SelectedIndex = -1;

            cboMaKhoaHoc.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaNganh.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaLop.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaKhoa.DropDownStyle = ComboBoxStyle.DropDown;

        }
        private void setEnable(bool check)
        {
            txtMaSV.Enabled = AddNew;
            txtTenSV.Enabled = check;
            txtSDT.Enabled = check;
            txtDiaChi.Enabled = check;
            timeNgaySinh.Enabled = check;
            txtGioiTinh.Enabled = check;
            txtEmail.Enabled = check;
            cboMaKhoaHoc.Enabled = check;
            cboMaNganh.Enabled = check;
            cboMaLop.Enabled = check;
            cboMaKhoa.Enabled = check;
            txtTenChucVu.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }
        private void dgvUsers_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtMaSV.Text = dgvUsers.Rows[i].Cells["SV_MaSV"].Value.ToString();
                txtTenSV.Text = dgvUsers.Rows[i].Cells["SV_HoVaTen"].Value.ToString();
                txtSDT.Text = dgvUsers.Rows[i].Cells["SV_DienThoai"].Value.ToString();
                txtDiaChi.Text = dgvUsers.Rows[i].Cells["SV_DiaChi"].Value.ToString();
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["SV_NgaySinh"].Value.ToString(), out DateTime ns))
                {
                    timeNgaySinh.Value = ns;
                }
                txtGioiTinh.Text = dgvUsers.Rows[i].Cells["SV_GioiTinh"].Value.ToString();
                txtEmail.Text = dgvUsers.Rows[i].Cells["SV_Email"].Value.ToString();
                cboMaLop.SelectedValue = dgvUsers.Rows[i].Cells["LP_ID"].Value.ToString();
                cboMaKhoaHoc.SelectedValue = dgvUsers.Rows[i].Cells["KH_ID"].Value.ToString();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            txtMaSV.Text = "";
            txtTenSV.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            timeNgaySinh.Text = "";
            txtGioiTinh.Text = "";
            txtEmail.Text = "";
            cboMaLop.Text = "";
            cboMaKhoaHoc.Text = "";
            AddNew = true;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string msv = txtMaSV.Text;
            string tsv = txtTenSV.Text;
            string sdt = txtSDT.Text;
            string dc = txtDiaChi.Text;
            string ns = timeNgaySinh.Value.ToString("yyyy-MM-dd");
            string gt = txtGioiTinh.Text;
            string em = txtEmail.Text;
            string ml = cboMaLop.SelectedValue?.ToString() ?? "";
            string mkh = cboMaKhoaHoc.SelectedValue?.ToString() ?? "";
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblSinhVien (SV_MaSV, SV_TenSV, SV_DienThoai, SV_DiaChi, SV_NgaySinh, SV_GioiTinh, SV_Email, LP_ID, KH_ID) VALUES " + "(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', N'{8}')", msv, tsv, sdt, dc, ns, gt, em, ml, mkh);
                db.runQuery(sql);
                AddNew = false;
                loadgriddata();
            }
            else
            {
                string sql = string.Format("UPDATE tblSinhVien SET SV_TenSV=N'{1}', SV_DienThoai=N'{2}', SV_DiaChi=N'{3}', SV_NgaySinh=N'{4}', SV_GioiTinh=N'{5}', SV_Email=N'{6}', LP_ID=N'{7}', KH_ID=N'{8}' WHERE SV_MaSV=N'{0}'", msv, tsv, sdt, dc, ns, gt, em, ml, mkh);
                db.runQuery(sql);
                loadgriddata();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string MaSV = txtMaSV.Text;
            string sql = string.Format("DELETE FROM tblSinhVien WHERE SV_MaSV=N'{0}'", MaSV);
            DBservices db = new DBservices();
            db.runQuery(sql);
            loadgriddata();
        }

        private void quảnLíHọcPhầnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            QuanLyHocPhan f = new QuanLyHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíLớpHọcPhầnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyLopHocPhan f = new QuanLyLopHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
        private void quảnLíKhóaHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyKhoaHoc f = new QuanLyKhoaHoc();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test f = new test();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
