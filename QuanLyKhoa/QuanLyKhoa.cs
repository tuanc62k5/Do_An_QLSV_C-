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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyKhoa
{
    public partial class QuanLyKhoa : Form
    {
        DBservices db = new DBservices();
        private bool AddNew = false;

        public QuanLyKhoa()
        {
            InitializeComponent();
        }

        private void QuanLyKhoa_Load(object sender, EventArgs e)
        {
            LayKhoa();
            SelectedKhoa();
            cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
        }
        private void LayKhoa()
        {
            string sql = "SELECT * FROM tblKhoa";
            DataTable dt = db.GetData(sql);

            DataRow row = dt.NewRow();
            row["K_ID"] = 0;
            row["K_TenKhoa"] = "Tất cả khoa";
            dt.Rows.InsertAt(row, 0);

            cboKhoa.DisplayMember = "K_TenKhoa";
            cboKhoa.ValueMember = "K_ID";
            cboKhoa.DataSource = dt;
            cboKhoa.SelectedIndex = 0;
        }
        private void SelectedKhoa()
        {
            if (cboKhoa.SelectedValue == null) return;
            
            if (int.TryParse(cboKhoa.SelectedValue.ToString(), out int id))
            {
                string sql = string.Format(id == 0 ? "SELECT * FROM tblKhoa" : "SELECT * FROM tblKhoa WHERE K_ID = {0}", id);
                dgvUsers.DataSource = db.GetData(sql);
            }
        }
        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                SelectedKhoa();
            }));
        }
        private void setEnable(bool check)
        {
            cboKhoa.Enabled = true;
            txtTenTruongKhoa.Enabled = check;
            txtDiaChi.Enabled = check;
            txtDienThoai.Enabled = check;
            txtEmail.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnSave.Enabled = check;
            btnExit.Enabled = !check;
        }

        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0 && dgvUsers.Rows[i].Cells["K_ID"].Value != null)
            {
                int rowKhoaID = Convert.ToInt32(dgvUsers.Rows[i].Cells["K_ID"].Value);
                if (cboKhoa.SelectedValue != null && int.TryParse(cboKhoa.SelectedValue.ToString(), out int selectKhoaID) && selectKhoaID != 0)
                {
                    cboKhoa.SelectedValue = rowKhoaID;
                }
                txtTenTruongKhoa.Text = dgvUsers.Rows[i].Cells["K_TenTruongKhoa"].Value.ToString();
                txtDiaChi.Text = dgvUsers.Rows[i].Cells["K_DiaChi"].Value.ToString();
                txtDienThoai.Text = dgvUsers.Rows[i].Cells["K_DienThoai"].Value.ToString();
                txtEmail.Text = dgvUsers.Rows[i].Cells["K_email"].Value.ToString();
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboKhoa.DataSource = null;
            cboKhoa.Text = "";
            cboKhoa.DropDownStyle = ComboBoxStyle.DropDown;
            txtTenTruongKhoa.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtEmail.Text = "";
            AddNew = true;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string tk = cboKhoa.Text.Trim();
            string ttk = txtTenTruongKhoa.Text.Trim();
            string dc = txtDiaChi.Text.Trim();
            string dt = txtDienThoai.Text.Trim();
            string em = txtEmail.Text.Trim();
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblKhoa (K_TenKhoa, K_TenTruongKhoa, K_DiaChi, K_DienThoai, K_Email) " + "VALUES (N'{0}', N'{1}', N'{2}', '{3}', '{4}')", tk, ttk, dc, dt, em);
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string id = cboKhoa.SelectedValue?.ToString() ?? "";
                string sql = string.Format("UPDATE tblKhoa SET K_TenKhoa=N'{1}', K_TenTruongKhoa=N'{2}', K_DiaChi=N'{3}', K_DienThoai=N'{4}', K_Email=N'{5}'" + "WHERE K_ID='{0}'", id, tk, ttk, dc, dt, em);
                db.runQuery(sql);
            }
            AddNew = false;
            LayKhoa();
            cboKhoa.SelectedIndex = 0;
            SelectedKhoa();
            setEnable(false);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            LayKhoa();
            setEnable(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = cboKhoa.SelectedValue.ToString();
            string sql = string.Format("DELETE FROM tblKhoa WHERE K_ID=N'{0}'", id);
            db.runQuery(sql);
            LayKhoa();
            cboKhoa.SelectedIndex = 0;
            SelectedKhoa();
            setEnable(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void quảnLíSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLySinhVien f = new QuanLySinhVien();
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

        private void quảnLíHọcPhầnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLyHocPhan f = new QuanLyHocPhan();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
