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
        private string currentAction = "";
        private int selectedSinhVienID = -1;
        private string QuyenHan;
        private int SVID;
        public QuanLySinhVien(string QuyenHan, int SVID)
        {
            this.QuyenHan = QuyenHan;
            this.SVID = SVID;
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
        private void LoadSinhVien(string filterCondition)
        {
            string query = $@"SELECT SV.SV_ID, SV.SV_MaSV, SV.SV_HoVaTen, SV.SV_NgaySinh, SV.SV_GioiTinh, SV.SV_DiaChi, SV.SV_Email, SV.SV_DienThoai, CV.CV_TenChucVu
                            FROM tblSinhVien_ChucVu SVCV
                            LEFT JOIN tblSinhVien SV ON SV.SV_ID = SVCV.SV_ID
                            LEFT JOIN tblChucVu CV on CV.CV_ID = SVCV.CV_ID
        {filterCondition}";

            DataTable dt = db.GetData(query);
            dgvUsers.DataSource = dt;
        }
        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboKhoa.SelectedItem;
                int K_ID = Convert.ToInt32(selectedRow["K_ID"]);
                LoadNganh(K_ID);
                string filter = $@"WHERE SV.LP_ID IN (SELECT LP_ID FROM tblLopHanhChinh WHERE NG_ID IN (
                                SELECT NG_ID FROM tblNganh WHERE K_ID = {K_ID}))";
                LoadSinhVien(filter);
            }
        }
        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNganh.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboNganh.SelectedItem;
                int NG_ID = Convert.ToInt32(selectedRow["NG_ID"]);
                LoadLopHanhChinh(NG_ID);
                string filter = $@"WHERE SV.LP_ID IN (SELECT LP_ID FROM tblLopHanhChinh WHERE NG_ID = {NG_ID})";
                LoadSinhVien(filter);
            }
        }
        private void cboLopHanhChinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLopHanhChinh.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cboLopHanhChinh.SelectedItem;
                int LP_ID = Convert.ToInt32(selectedRow["LP_ID"]);
                string filter = $"WHERE SV.LP_ID = {LP_ID}";
                LoadSinhVien(filter);
            }
        }
        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            currentAction = "add";
            selectedSinhVienID = -1;
            
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtSinhVien.Text.Trim();
            DateTime ngaySinh = timeNgaySinh.Value;
            string gioiTinh = txtGioiTinh.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string email = txtEmail.Text.Trim();
            string dienThoai = txtDienThoai.Text.Trim();
            int lp_id = Convert.ToInt32(cboLopHanhChinh.SelectedValue);
            int kh_id = Convert.ToInt32(cboKhoaHoc.SelectedValue);

            if (currentAction == "add")
            {
                string query = $@"INSERT INTO tblSinhVien (SV_MaSV, SV_HoVaTen, SV_NgaySinh, SV_GioiTinh, SV_DiaChi, SV_Email, SV_DienThoai, LP_ID, KH_ID)
                        VALUES (N'{maSV}', N'{hoTen}', '{ngaySinh:yyyy-MM-dd}', N'{gioiTinh}', N'{diaChi}', N'{email}', N'{dienThoai}', {lp_id}, {kh_id})";
                db.runQuery(query);
                MessageBox.Show("Thêm sinh viên thành công!");
            }
            else if (currentAction == "edit" && selectedSinhVienID != -1)
            {
                string query = $@"UPDATE tblSinhVien SET 
                        SV_MaSV = N'{maSV}', SV_HoVaTen = N'{hoTen}', SV_NgaySinh = '{ngaySinh:yyyy-MM-dd}', 
                        SV_GioiTinh = N'{gioiTinh}', SV_DiaChi = N'{diaChi}', SV_Email = N'{email}', 
                        SV_DienThoai = N'{dienThoai}', LP_ID = {lp_id}, KH_ID = {kh_id}
                        WHERE SV_ID = {selectedSinhVienID}";
                db.runQuery(query);
                MessageBox.Show("Cập nhật sinh viên thành công!");
            }

            LoadSinhVien("");
            currentAction = "";
            
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                selectedSinhVienID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["SV_ID"].Value);
                txtMaSV.Text = dgvUsers.CurrentRow.Cells["SV_MaSV"].Value.ToString();
                txtSinhVien.Text = dgvUsers.CurrentRow.Cells["SV_HoVaTen"].Value.ToString();
                timeNgaySinh.Value = Convert.ToDateTime(dgvUsers.CurrentRow.Cells["SV_NgaySinh"].Value);
                txtGioiTinh.Text = dgvUsers.CurrentRow.Cells["SV_GioiTinh"].Value.ToString();
                txtDiaChi.Text = dgvUsers.CurrentRow.Cells["SV_DiaChi"].Value.ToString();
                txtEmail.Text = dgvUsers.CurrentRow.Cells["SV_Email"].Value.ToString();
                txtDienThoai.Text = dgvUsers.CurrentRow.Cells["SV_DienThoai"].Value.ToString();
                cboKhoaHoc.SelectedValue = HienThiKH_ID(selectedSinhVienID);
                currentAction = "edit";
            }
        }
        private int HienThiKH_ID(int SVID)
        {
            string query = $"SELECT KH_ID FROM tblSinhVien WHERE SV_ID = {SVID}";
            DataTable dt = db.GetData(query);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["KH_ID"]);
            }
            return -1;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int sv_id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["SV_ID"].Value);
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string query = $"DELETE FROM tblSinhVien WHERE SV_ID = {sv_id}";
                    db.runQuery(query);
                    MessageBox.Show("Xóa sinh viên thành công!");
                    LoadSinhVien("");
                }
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
        private void thôngTinChiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiaoDien f = new GiaoDien();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiaoDien f = new GiaoDien();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void quảnLíĐiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyDiem f = new QuanLyDiem();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

    }
}
