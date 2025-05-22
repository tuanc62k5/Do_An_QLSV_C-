using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhoa.Linq
{
    public partial class QLKhoa : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLKhoa()
        {
            InitializeComponent();
        }
        private void QLKhoa_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void LayDuLieu()
        {
            var query = from k in DataBase.tblKhoas
                        select k;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                txtKhoa.Text = dgvUsers.Rows[i].Cells["K_TenKhoa"].Value.ToString();
                txtTruongKhoa.Text = dgvUsers.Rows[i].Cells["K_TenTruongKhoa"].Value.ToString();
                txtDiaChi.Text = dgvUsers.Rows[i].Cells["K_DiaChi"].Value.ToString();
                txtDienThoai.Text = dgvUsers.Rows[i].Cells["K_DienThoai"].Value.ToString();
                txtEmail.Text = dgvUsers.Rows[i].Cells["K_Email"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            txtKhoa.Enabled = check;
            txtTruongKhoa.Enabled = check;
            txtDiaChi.Enabled = check;
            txtDienThoai.Enabled = check;
            txtEmail.Enabled = check;
            btnSave.Enabled = check;
            btnAddNew.Enabled = !check;
            btnEdit.Enabled = !check;
            btnDelete.Enabled = !check;
            btnExit.Enabled = !check;
            btnLamMoi.Enabled = !check;
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (AddNew)
            {
                tblKhoa k = new tblKhoa();
                k.K_TenKhoa = txtKhoa.Text;
                k.K_TenTruongKhoa = txtTruongKhoa.Text;
                k.K_DiaChi = txtDiaChi.Text;
                k.K_DienThoai = txtDienThoai.Text;
                k.K_Email = txtEmail.Text;
                DataBase.tblKhoas.Add(k);
                DataBase.SaveChanges();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["K_ID"].Value);
                    var query = from k in DataBase.tblKhoas
                                where (k.K_ID == id)
                                select k;
                    tblKhoa khoa = query.First();
                    khoa.K_TenKhoa = txtKhoa.Text;
                    khoa.K_TenTruongKhoa = txtTruongKhoa.Text;
                    khoa.K_DiaChi = txtDiaChi.Text;
                    khoa.K_DienThoai = txtDienThoai.Text;
                    khoa.K_Email = txtEmail.Text;
                    DataBase.SaveChanges();
                }
            }
            LayDuLieu();
            setEnable(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["K_ID"].Value);
                var query = from k in DataBase.tblKhoas
                            where (k.K_ID == id)
                            select k;
                DataBase.tblKhoas.Remove(query.First());
                DataBase.SaveChanges();
                LayDuLieu();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LayDuLieu();
            txtTimKiem.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenKhoa = txtTimKiem.Text.Trim();
            var query = from k in DataBase.tblKhoas
                        where k.K_TenKhoa.Contains(TenKhoa)
                        select k;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
