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
    public partial class QuanLyLopHocPhan : Form
    {
        bool AddNew = false;
        public QuanLyLopHocPhan()
        {
            InitializeComponent();
        }

        private void QuanLyLopHocPhan_Load(object sender, EventArgs e)
        {
            loadgriddata();
            loadMaHocPhan();
            loadNamHoc();
            cboMaLopHP.DropDownStyle = ComboBoxStyle.DropDown;
            cboMaHocPhan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaNamHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            timeNgayBatDau.Format = DateTimePickerFormat.Custom;
            timeNgayBatDau.CustomFormat = "dd/MM/yyyy";
            timeNgayKetThuc.Format = DateTimePickerFormat.Custom;
            timeNgayKetThuc.CustomFormat = "dd/MM/yyyy";
        }
        private void loadMaHocPhan()
        {
            DBservices db = new DBservices();
            string sql = "SELECT MaHocPhan, TenHocPhan FROM tblHocPhan";
            cboMaHocPhan.DataSource = db.GetData(sql);
            cboMaHocPhan.DisplayMember = "TenHocPhan";
            cboMaHocPhan.ValueMember = "MaHocPhan";
        }
        private void loadNamHoc()
        {
            DBservices db = new DBservices();
            string sql = "SELECT MaNamHoc, TenNamHoc FROM tblNamHoc";
            cboMaNamHoc.DataSource = db.GetData(sql);
            cboMaNamHoc.DisplayMember = "TenNamHoc";
            cboMaNamHoc.ValueMember = "MaNamHoc";
        }
        private void loadgriddata()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblLopHocPhan";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }
        private void setEnable(bool check)
        {
            cboMaLopHP.Enabled = AddNew;
            txtTenLopHP.Enabled = check;
            cboMaHocPhan.Enabled = check;
            txtTenGV.Enabled = check;
            cboMaNamHoc.Enabled = check;
            txtHocKy.Enabled = check;
            txtGioiHanSV.Enabled = check;
            timeNgayBatDau.Enabled = check;
            timeNgayKetThuc.Enabled = check;
            txtPhongHoc.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }

        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cboMaLopHP.Text = dgvUsers.Rows[i].Cells["MaLopHP"].Value.ToString();
                txtTenLopHP.Text = dgvUsers.Rows[i].Cells["TenLopHP"].Value.ToString();
                cboMaHocPhan.Text = dgvUsers.Rows[i].Cells["MaHocPhan"].Value.ToString();
                txtTenGV.Text = dgvUsers.Rows[i].Cells["TenGiangVien"].Value.ToString();
                cboMaNamHoc.Text = dgvUsers.Rows[i].Cells["MaNamHoc"].Value.ToString();
                txtHocKy.Text = dgvUsers.Rows[i].Cells["HocKy"].Value.ToString();
                txtGioiHanSV.Text = dgvUsers.Rows[i].Cells["GioiHanSV"].Value.ToString();
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["NgayBatDau"].Value.ToString(), out DateTime nbd))
                {
                    timeNgayBatDau.Value = nbd;
                }
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["NgayKetThuc"].Value.ToString(), out DateTime nkt))
                {
                    timeNgayKetThuc.Value = nkt;
                }
                txtPhongHoc.Text = dgvUsers.Rows[i].Cells["PhongHoc"].Value.ToString();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboMaLopHP.Text= "";
            txtTenLopHP.Text = "";
            cboMaHocPhan.Text = "";
            txtTenGV.Text = "";
            cboMaNamHoc.Text = "";
            txtHocKy.Text = "";
            txtGioiHanSV.Text = "";
            timeNgayBatDau.Text = "";
            timeNgayKetThuc.Text = "";
            txtPhongHoc.Text = "";
            AddNew = true;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string mlhp = cboMaLopHP.Text;
            string tlhp = txtTenLopHP.Text;
            string mhp = cboMaHocPhan.SelectedValue.ToString();
            string tgv = txtTenGV.Text;
            string mnh = cboMaNamHoc.SelectedValue.ToString();
            string hk = txtHocKy.Text;
            string ghsv = txtGioiHanSV.Text;
            string nbd = timeNgayBatDau.Value.ToString("yyyy-MM-dd");
            string nkt = timeNgayKetThuc.Value.ToString("yyyy-MM-dd");
            string ph = txtPhongHoc.Text;

            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblLopHocPhan (MaLopHP, TenLopHP, MaHocPhan, TenGiangVien, MaNamHoc, HocKy, GioiHanSV, NgayBatDau, NgayKetThuc, PhongHoc) VALUES " + "(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}', N'{5}', N'{6}', N'{7}', N'{8}', N'{9}')", mlhp, tlhp, mhp, tgv, mnh, hk, ghsv, nbd, nkt, ph);
                db.runQuery(sql);
                AddNew = false;
                loadgriddata();
            }
            else
            {
                string sql = string.Format("UPDATE tblLopHocPhan SET TenLopHP=N'{1}', MaHocPhan=N'{2}', TenGiangVien=N'{3}', MaNamHoc=N'{4}', HocKy=N'{5}', GioiHanSV=N'{6}', NgayBatDau=N'{7}', NgayKetThuc=N'{8}', PhongHoc=N'{9}' WHERE MaLopHP=N'{0}'", mlhp, tlhp, mhp, tgv, mnh, hk, ghsv, nbd, nkt, ph);
                db.runQuery(sql);
                loadgriddata();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string MaLopHP = cboMaLopHP.Text;
            string sql = string.Format("DELETE FROM tblLopHocPhan WHERE MaLopHP=N'{0}'", MaLopHP);
            DBservices db = new DBservices();
            db.runQuery(sql);
            loadgriddata();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quảnLíLớpHọcPhầnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyLopHocPhan f = new QuanLyLopHocPhan();
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

        private void quảnLíHọcPhầnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            QuanLyHocPhan f = new QuanLyHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }
    }
}
