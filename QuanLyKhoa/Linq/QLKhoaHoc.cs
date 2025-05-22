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
    public partial class QLKhoaHoc : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLKhoaHoc()
        {
            InitializeComponent();
        }
        private void QLKhoaHoc_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void LayDuLieu()
        {
            var query = from kh in DataBase.tblKhoaHocs
                        select kh;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                txtKhoaHoc.Text = dgvUsers.Rows[i].Cells["KH_TenKhoaHoc"].Value.ToString();
                txtNamBatDau.Text = dgvUsers.Rows[i].Cells["KH_NamNBatDau"].Value.ToString();
                txtNamKetThuc.Text = dgvUsers.Rows[i].Cells["KH_NamKetThuc"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            txtKhoaHoc.Enabled = check;
            txtNamBatDau.Enabled = check;
            txtNamKetThuc.Enabled = check;
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
                tblKhoaHoc kh = new tblKhoaHoc();
                kh.KH_TenKhoaHoc = txtKhoaHoc.Text;
                kh.KH_NamBatDau = int.Parse(txtNamBatDau.Text);
                kh.KH_NamKetThuc = int.Parse(txtNamKetThuc.Text);
                DataBase.tblKhoaHocs.Add(kh);
                DataBase.SaveChanges();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["KH_ID"].Value);
                    var query = from kh in DataBase.tblKhoaHocs
                                where (kh.KH_ID == id)
                                select kh;
                    tblKhoaHoc khoahoc = query.First();
                    khoahoc.KH_TenKhoaHoc = txtKhoaHoc.Text;
                    khoahoc.KH_NamBatDau = int.Parse(txtNamBatDau.Text);
                    khoahoc.KH_NamKetThuc = int.Parse(txtNamKetThuc.Text);
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
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["KH_ID"].Value);
                var query = from kh in DataBase.tblKhoaHocs
                            where (kh.KH_ID == id)
                            select kh;
                DataBase.tblKhoaHocs.Remove(query.First());
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
            string TenKhoaHoc = txtTimKiem.Text.Trim();
            var query = from kh in DataBase.tblKhoaHocs
                        where kh.KH_TenKhoaHoc.Contains(TenKhoaHoc)
                        select kh;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
