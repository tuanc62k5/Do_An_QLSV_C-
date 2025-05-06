using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhoa
{
    public partial class QuanLySinhVien : Form
    {
        DBservices db = new DBservices();
        public QuanLySinhVien()
        {
            InitializeComponent();
            LoadKhoa();
            LoadKhoaHoc();
        }
        private void QuanLySinhVien_Load(object sender, EventArgs e)
        {
            dgvUsers.AutoGenerateColumns = true;
        }
        private void LoadKhoa()
        {
            string query = "SELECT * FROM tblKhoa";
            DataTable dt = db.GetData(query);
            cboKhoa.DataSource = dt;
            cboKhoa.DisplayMember = "K_TenKhoa";
            cboKhoa.ValueMember = "K_ID";
            cboKhoa.SelectedIndex = -1;
        }
        private void LoadNganh(int K_ID)
        {
            string query = $"SELECT * FROM tblNganh WHERE K_ID = '{K_ID}'";
            DataTable dt = db.GetData(query);
            cboNganh.DataSource = dt;
            cboNganh.DisplayMember = "NG_TenNganh";
            cboNganh.ValueMember = "NG_ID";
            cboNganh.SelectedIndex = -1;
        }
        private void LoadLopHanhChinh(int NG_ID)
        {
            string query = $"SELECT * FROM tblLopHanhChinh WHERE NG_ID = '{NG_ID}'";
            DataTable dt = db.GetData(query);
            cboLopHanhChinh.DataSource = dt;
            cboLopHanhChinh.DisplayMember = "LP_TenLop";
            cboLopHanhChinh.ValueMember = "LP_ID";
            cboLopHanhChinh.SelectedIndex = -1;
        }
        private void LoadKhoaHoc()
        {
            string query = "SELECT * FROM tblKhoaHoc";
            DataTable dt = db.GetData(query);
            cboKhoaHoc.DataSource = dt;
            cboKhoaHoc.DisplayMember = "KH_TenKhoaHoc";
            cboKhoaHoc.ValueMember = "KH_ID";
            cboKhoaHoc.SelectedIndex = -1;
        }
        private void LoadSinhVien(string query)
        {
            DataTable dt = db.GetData(query);
            dgvUsers.DataSource = dt;
        }
        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedItem != null)
            {
                // Lấy đối tượng DataRowView từ SelectedItem
                DataRowView selectedRow = (DataRowView)cboKhoa.SelectedItem;

                // Lấy giá trị của K_ID từ DataRowView
                int K_ID = Convert.ToInt32(selectedRow["K_ID"]);

                LoadNganh(K_ID);

                string query = $@"SELECT * FROM tblSinhVien WHERE LP_ID IN (
                            SELECT LP_ID FROM tblLopHanhChinh WHERE NG_ID IN (
                                SELECT NG_ID FROM tblNganh WHERE K_ID = {K_ID}
                            )
                        )";
                LoadSinhVien(query);
            }
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNganh.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboNganh.SelectedItem;
                int NG_ID = Convert.ToInt32(selectedRow["NG_ID"]);
                LoadLopHanhChinh(NG_ID);

                string query = $@"SELECT * FROM tblSinhVien WHERE LP_ID IN (
                            SELECT LP_ID FROM tblLopHanhChinh WHERE NG_ID = {NG_ID}
                        )";
                LoadSinhVien(query);
            }
        }
        private void cboLopHanhChinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLopHanhChinh.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboLopHanhChinh.SelectedItem;
                int LP_ID = Convert.ToInt32(selectedRow["LP_ID"]);
                string query = $"SELECT * FROM tblSinhVien WHERE LP_ID = {LP_ID}";
                LoadSinhVien(query);
            }
        }
        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoaHoc.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboKhoaHoc.SelectedItem;
                int KH_ID = Convert.ToInt32(selectedRow["KH_ID"]);
                string query = $"SELECT * FROM tblSinhVien WHERE KH_ID = {KH_ID}";
                LoadSinhVien(query);
            }
        }
    
       
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearForm()
        {
            txtMaSV.Clear();
            txtGioiTinh.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            timeNgaySinh.Value = DateTime.Now;
            cboSinhVien.SelectedIndex = -1;
            cboChucVu.SelectedIndex = -1;
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
        private void quảnLíLớpHànhChínhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyLopHanhChinh f = new QuanLyLopHanhChinh();
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
        private void quảnLíNămHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyNamHoc f = new QuanLyNamHoc();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }


    }
}
