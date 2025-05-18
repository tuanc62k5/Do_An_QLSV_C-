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
    public partial class QuanLyDiem : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;
        private bool LoadingData = false;
        public QuanLyDiem()
        {
            InitializeComponent();
        }

        private void QuanLyDiem_Load(object sender, EventArgs e)
        {
            LoadLopHP();
            setEnable(false);
        }
        private void LoadLopHP()
        {
            LoadingData = true;
            string sql = "SELECT * FROM tblLopHocPhan";
            DataTable dt = db.GetData(sql);

            cboLopHP.DataSource = dt;
            cboLopHP.DisplayMember = "LHP_TenLopHP";
            cboLopHP.ValueMember = "LHP_ID";

            if (cboLopHP.Items.Count > 0)
            {
                cboLopHP.SelectedIndex = 0;
            }

            LoadingData = false;

            if (int.TryParse(cboLopHP.SelectedValue?.ToString(), out int lhpID))
            {
                LoadSinhVienTheoLopHP(lhpID);
            }
        }
        private void LoadSinhVienTheoLopHP(int LHP_ID)
        {
            string sql = $@"
                SELECT sv.SV_ID, sv.SV_HoVaTen, 
                       d.DIEM_ChuyenCan, d.DIEM_GiuaKy, d.DIEM_CuoiKy, d.DIEM_TongKet
                FROM tblSinhVien sv
                INNER JOIN tblDiem d ON sv.SV_ID = d.SV_ID
                WHERE d.LHP_ID = {LHP_ID}";

            DataTable dt = db.GetData(sql);

            cboSinhVien.SelectedIndexChanged -= cboSinhVien_SelectedIndexChanged;
            cboSinhVien.DataSource = dt;
            cboSinhVien.DisplayMember = "SV_HoVaTen";
            cboSinhVien.ValueMember = "SV_ID";
            cboSinhVien.SelectedIndexChanged += cboSinhVien_SelectedIndexChanged;

            dgvUsers.DataSource = dt;
        }

        private void cboLopHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboLopHP.SelectedValue == null || cboLopHP.SelectedValue is DataRowView)
                return;

            if (int.TryParse(cboLopHP.SelectedValue.ToString(), out int lhpID))
            {
                LoadSinhVienTheoLopHP(lhpID);
            }
        }
        private void cboSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingData || cboSinhVien.SelectedValue == null || cboSinhVien.SelectedValue is DataRowView)
                return;

            string svID = cboSinhVien.SelectedValue.ToString();
            if (!int.TryParse(cboLopHP.SelectedValue?.ToString(), out int lhpID)) return;

            string sql = $@"
                SELECT sv.SV_ID, sv.SV_HoVaTen, 
                       d.DIEM_ChuyenCan, d.DIEM_GiuaKy, d.DIEM_CuoiKy, d.DIEM_TongKet
                FROM tblSinhVien sv
                INNER JOIN tblDiem d ON sv.SV_ID = d.SV_ID
                WHERE d.LHP_ID = {lhpID} AND sv.SV_ID = {svID}";

            DataTable dt = db.GetData(sql);
            dgvUsers.DataSource = dt;
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (LoadingData || e.RowIndex < 0) return;

            int i = e.RowIndex;
            DataGridViewRow row = dgvUsers.Rows[i];

            if (row.Cells["SV_ID"].Value != null)
            {
                LoadingData = true;

                cboSinhVien.SelectedValue = row.Cells["SV_ID"].Value;
                txtDiemChuyenCan.Text = row.Cells["DIEM_ChuyenCan"].Value?.ToString() ?? "";
                txtDiemGiuaKy.Text = row.Cells["DIEM_GiuaKy"].Value?.ToString() ?? "";
                txtDiemCuoiKy.Text = row.Cells["DIEM_CuoiKy"].Value?.ToString() ?? "";
                txtDiemTongKet.Text = row.Cells["DIEM_TongKet"].Value?.ToString() ?? "";

                LoadingData = false;
            }
        }
        private void setEnable(bool enable)
        {
            cboLopHP.Enabled = !enable;
            cboSinhVien.Enabled = true;
            txtDiemChuyenCan.Enabled = enable;
            txtDiemGiuaKy.Enabled = enable;
            txtDiemCuoiKy.Enabled = enable;
            txtDiemTongKet.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnExit.Enabled = !enable;

            if (enable && AddNew)
            {
                cboSinhVien.Text = "";
                txtDiemChuyenCan.Clear();
                txtDiemGiuaKy.Clear();
                txtDiemCuoiKy.Clear();
                txtDiemTongKet.Clear();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            cboSinhVien.DropDownStyle = ComboBoxStyle.DropDown;
            cboSinhVien.Text = "";
            txtDiemChuyenCan.Clear();
            txtDiemGiuaKy.Clear();
            txtDiemCuoiKy.Clear();
            txtDiemTongKet.Clear();
            setEnable(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboSinhVien.DropDownStyle = ComboBoxStyle.DropDownList;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string svID = cboSinhVien.SelectedValue?.ToString();
            string cc = txtDiemChuyenCan.Text.Trim();
            string gk = txtDiemGiuaKy.Text.Trim();
            string ck = txtDiemCuoiKy.Text.Trim();
            string tk = txtDiemTongKet.Text.Trim();
            string lopID = cboLopHP.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(svID) || string.IsNullOrEmpty(lopID)) return;

            string checkSql = $"SELECT COUNT(*) AS Total FROM tblDiem WHERE SV_ID = '{svID}' AND LHP_ID = '{lopID}'";
            DataTable dt = db.GetData(checkSql);

            int count = (dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0]["Total"]) : 0;

            string sql;
            if (count == 0)
            {
                sql = $"INSERT INTO tblDiem (SV_ID, LHP_ID, DIEM_ChuyenCan, DIEM_GiuaKy, DIEM_CuoiKy, DIEM_TongKet) " +
                      $"VALUES ('{svID}', '{lopID}', N'{cc}', N'{gk}', N'{ck}', N'{tk}')";
            }
            else
            {
                sql = $"UPDATE tblDiem SET DIEM_ChuyenCan=N'{cc}', DIEM_GiuaKy=N'{gk}', DIEM_CuoiKy=N'{ck}', DIEM_TongKet=N'{tk}' " +
                      $"WHERE SV_ID='{svID}' AND LHP_ID = '{lopID}'";
            }

            db.runQuery(sql);
            LoadSinhVienTheoLopHP(int.Parse(lopID));
            setEnable(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string svID = cboSinhVien.SelectedValue?.ToString();
            string lopID = cboLopHP.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(svID) || string.IsNullOrEmpty(lopID)) return;

            string sql = $"DELETE FROM tblDiem WHERE SV_ID='{svID}' AND LHP_ID='{lopID}'";
            db.runQuery(sql);

            LoadSinhVienTheoLopHP(int.Parse(lopID));
            dgvUsers.DataSource = null;
            setEnable(false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
