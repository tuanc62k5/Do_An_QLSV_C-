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
        DBservices db = new DBservices();
        private bool AddNew = false;
        private bool LoadingData = false;
        public QuanLyLopHocPhan()
        {
            InitializeComponent();
        }

        private void QuanLyLopHocPhan_Load(object sender, EventArgs e)
        {
            LoadHocPhan();
            LoadNamHoc();
            LoadHocKy();
            setEnable(false);

            timeNgayBatDau.Format = DateTimePickerFormat.Custom;
            timeNgayBatDau.CustomFormat = "dd/MM/yyyy";
            timeNgayKetThuc.Format = DateTimePickerFormat.Custom;
            timeNgayKetThuc.CustomFormat = "dd/MM/yyyy";
        }
        private void LoadHocPhan()
        {
            LoadingData = true;
            string sql = "SELECT * FROM tblHocPhan";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["HP_ID"] = 0;
            row["HP_TenHocPhan"] = "Tất cả Học phần";
            dt.Rows.InsertAt(row, 0);

            cboHocPhan.DataSource = dt;
            cboHocPhan.DisplayMember = "HP_TenHocPhan";
            cboHocPhan.ValueMember = "HP_ID";

            if (cboHocPhan.SelectedIndex == -1)
            {
                cboHocPhan.SelectedIndex = 0;
            }
            LoadingData = false;
            LoadLopHPTheoHocPhan(0);
        }
        private void LoadLopHPTheoHocPhan(int HP_ID)
        {
            if (LoadingData) return;
            string sql = HP_ID == 0 ? "SELECT * FROM tblLopHocPhan" : $"SELECT * FROM tblLopHocPhan WHERE HP_ID = {HP_ID}";
            DataTable dt = db.GetData(sql);

            cboLopHocPhan.SelectedIndexChanged -= cboLopHocPhan_SelectedIndexChanged;

            cboLopHocPhan.DataSource = dt;
            cboLopHocPhan.DisplayMember = "LHP_TenLopHP";
            cboLopHocPhan.ValueMember = "LHP_ID";

            cboLopHocPhan.SelectedIndexChanged += cboLopHocPhan_SelectedIndexChanged;

            this.BeginInvoke(new Action(() =>
            {
                dgvUsers.DataSource = null;
                dgvUsers.DataSource = dt;
            }));
            if (int.TryParse(cboHocPhan.SelectedValue?.ToString(), out int currentHocPhan) && currentHocPhan == 0) return;
        }
        private void LoadNamHoc()
        {
            string sql = "SELECT * FROM tblNamHoc";
            DataTable dt = db.GetData(sql);
            cboNamHoc.DataSource = dt;
            cboNamHoc.DisplayMember = "NH_TenNamHoc";
            cboNamHoc.ValueMember = "NH_ID";
        }
        private void LoadHocKy()
        {
            string sql = "SELECT * FROM tblHocKy";
            DataTable dt = db.GetData(sql);

            cboHocKy.DataSource = dt;
            cboHocKy.DisplayMember = "HK_TenHocKy";
            cboHocKy.ValueMember = "HK_ID"; 
        }
        private void cboHocPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboHocPhan.SelectedValue == null || cboHocPhan.SelectedValue is DataRowView) return;
            if (int.TryParse(cboHocPhan.SelectedValue.ToString(), out int HP_ID))
            {
                LoadLopHPTheoHocPhan(HP_ID);
            }
        }

        private void cboLopHocPhan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboLopHocPhan.SelectedValue == null || cboLopHocPhan.SelectedValue is DataRowView) return;
            if (int.TryParse(cboLopHocPhan.SelectedValue.ToString(), out int LHP_ID))
            {
                this.BeginInvoke(new Action(() =>
                {
                    string sql = $"SELECT * FROM tblLopHocPhan WHERE LHP_ID = {LHP_ID}";
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
            if (i >= 0 && dgvUsers.Rows[i].Cells["LHP_ID"].Value != null)
            {
                LoadingData = true;
                cboLopHocPhan.SelectedValue = dgvUsers.Rows[i].Cells["LHP_ID"].Value;
                if (int.TryParse(cboLopHocPhan.SelectedValue?.ToString(), out int selectLopHPID) && selectLopHPID != 0)
                {
                    cboHocPhan.SelectedValue = dgvUsers.Rows[i].Cells["HP_ID"].Value;
                }
                txtTenGV.Text = dgvUsers.Rows[i].Cells["LHP_GiangVien"].Value.ToString();
                cboNamHoc.SelectedValue = dgvUsers.Rows[i].Cells["NH_ID"].Value.ToString();
                cboHocKy.SelectedValue = dgvUsers.Rows[i].Cells["HK_ID"].Value.ToString();
                txtPhongHoc.Text = dgvUsers.Rows[i].Cells["LHP_PhongHoc"].Value.ToString();
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["LHP_NgayBatDau"].Value.ToString(), out DateTime nbd))
                {
                    timeNgayBatDau.Value = nbd;
                }
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["LHP_NgayKetThuc"].Value.ToString(), out DateTime nkt))
                {
                    timeNgayKetThuc.Value = nkt;
                }
                LoadingData = false;
            }
        }
        private void setEnable(bool enable)
        {
            cboHocPhan.Enabled = true;
            cboLopHocPhan.Enabled = true;
            txtTenGV.Enabled = enable;
            cboNamHoc.Enabled = enable;
            cboHocKy.Enabled = enable;
            txtPhongHoc.Enabled = enable;
            timeNgayBatDau.Enabled = enable;
            timeNgayKetThuc.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnExit.Enabled = !enable;
            if (enable && AddNew)
            {
                cboLopHocPhan.Text = "";
                cboHocPhan.SelectedIndex = 0;
                txtTenGV.Enabled = enable;
                cboNamHoc.SelectedIndex = 0;
                cboHocKy.SelectedIndex = 0;
                txtPhongHoc.Enabled = enable;
            }
        }


        private void btnAddNew_Click(object sender, EventArgs e)
        {
            LoadingData = true;
            cboLopHocPhan.DropDownStyle = ComboBoxStyle.DropDown;
            cboNamHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHocKy.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string tenLop = cboLopHocPhan.Text.Trim();
            string gv = txtTenGV.Text.Trim();
            string phong = txtPhongHoc.Text.Trim();
            string hp = cboHocPhan.SelectedValue?.ToString();
            string nh = cboNamHoc.SelectedValue?.ToString();
            string hk = cboHocKy.SelectedValue?.ToString();
            string nbd = timeNgayBatDau.Value.ToString("yyyy-MM-dd");
            string nkt = timeNgayKetThuc.Value.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(tenLop)) return;

            if (LoadingData)
            {
                string sql = $@"
                    INSERT INTO tblLopHocPhan (LHP_TenLopHP, LHP_GiangVien, LHP_PhongHoc, LHP_NgayBatDau, LHP_NgayKetThuc, HP_ID, NH_ID, HK_ID)
                    VALUES (N'{tenLop}', N'{gv}', N'{phong}', '{nbd}', '{nkt}', {hp}, {nh}, {hk})";
                db.runQuery(sql);
                LoadingData = false;
            }
            else
            {
                string id = cboLopHocPhan.SelectedValue?.ToString();
                string sql = $@"
                    UPDATE tblLopHocPhan 
                    SET LHP_TenLopHP=N'{tenLop}', LHP_GiangVien=N'{gv}', LHP_PhongHoc=N'{phong}',
                        LHP_NgayBatDau='{nbd}', LHP_NgayKetThuc='{nkt}',
                        HP_ID={hp}, NH_ID={nh}, HK_ID={hk}
                    WHERE LHP_ID={id}";
                db.runQuery(sql);
            }

            if (int.TryParse(hp, out int HP_ID))
                LoadLopHPTheoHocPhan(HP_ID);

            setEnable(false);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            LoadingData = false;
            cboLopHocPhan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNamHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHocKy.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboLopHocPhan.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(id)) return;

            db.runQuery($"DELETE FROM tblDiem WHERE LHP_ID = {id}");
            db.runQuery($"DELETE FROM tblLopHocPhan WHERE LHP_ID = {id}");

            if (int.TryParse(cboHocPhan.SelectedValue.ToString(), out int HP_ID))
                LoadLopHPTheoHocPhan(HP_ID);

            dgvUsers.DataSource = null;
            setEnable(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
