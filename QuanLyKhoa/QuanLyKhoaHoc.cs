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
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-KFK2IU3B\\TUANANH2K5;Initial Catalog=QLSV;Integrated Security=True");
        bool AddNew = false;
        public QuanLyKhoaHoc()
        {
            InitializeComponent();
        }

        private void QuanLyKhoaHoc_Load(object sender, EventArgs e)
        {
            loadgriddata();
            loadKhoaHoc();
        }
        private void loadgriddata()
        {
            DBservices db = new DBservices();
            string sql = "SELECT * FROM tblKhoaHoc";
            dgvUsers.DataSource = db.GetData(sql);
            setEnable(false);
        }
        private void loadKhoaHoc()
        {
            string sql = "SELECT KH_ID, KH_TenKhoaHoc FROM tblKhoaHoc";
            DBservices db = new DBservices();
            DataTable dt = db.GetData(sql);
            cboMaKhoaHoc.DataSource = dt;
            cboMaKhoaHoc.DisplayMember = "KH_TenKhoaHoc";
            cboMaKhoaHoc.ValueMember = "KH_ID";
        }
        private void setEnable(bool check)
        {
            cboMaKhoaHoc.Enabled = AddNew;
            txtTenKhoaHoc.Enabled = check;
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
            if (i >= 0)
            {
                string makhoahoc = dgvUsers.Rows[i].Cells["KH_ID"].Value.ToString();
                cboMaKhoaHoc.SelectedValue = makhoahoc;
                txtTenKhoaHoc.Text = dgvUsers.Rows[i].Cells["KH_TenKhoaHoc"].Value.ToString();
                txtNamBatDau.Text = dgvUsers.Rows[i].Cells["KH_NamBatDau"].Value.ToString();
                txtNamKetThuc.Text = dgvUsers.Rows[i].Cells["KH_NamKetThuc"].Value.ToString();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            cboMaKhoaHoc.DataSource = null;
            cboMaKhoaHoc.Text = "";
            txtTenKhoaHoc.Text = "";
            txtNamBatDau.Text = "";
            txtNamKetThuc.Text = "";
            cboMaKhoaHoc.DropDownStyle = ComboBoxStyle.DropDown; // cho phép nhập mã khoá học bằng tay
            AddNew = true;
            setEnable(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            cboMaKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            loadKhoaHoc();
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string mkh = AddNew ? cboMaKhoaHoc.Text : cboMaKhoaHoc.SelectedValue.ToString();
            string tkh = txtTenKhoaHoc.Text;
            string nbd = txtNamBatDau.Text;
            string nkt = txtNamKetThuc.Text;
            DBservices db = new DBservices();
            if (AddNew)
            {
                string sql = string.Format("INSERT INTO tblKhoaHoc (KH_ID, KH_TenKhoaHoc, KH_NamBatDau, KH_NamKetThuc) VALUES " + "(N'{0}', N'{1}', N'{2}', N'{3}')", mkh, tkh, nbd, nkt);
                db.runQuery(sql);
                AddNew = false;
            }
            else
            {
                string sql = string.Format("UPDATE tblKhoaHoc SET KH_TenKhoaHoc=N'{1}', KH_NamBatDau=N'{2}', KH_NamKetThuc=N'{3}', WHERE KH_ID=N'{0}'", mkh, tkh, nbd, nkt);
                db.runQuery(sql);
            }
            loadgriddata();
            loadKhoaHoc();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string KH_ID = cboMaKhoaHoc.Text;
            string sql = string.Format("DELETE FROM tblKhoa WHERE KH_ID=N'{0}'", KH_ID);
            DBservices db = new DBservices();
            db.runQuery(sql);
            loadgriddata();
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
    }
}
