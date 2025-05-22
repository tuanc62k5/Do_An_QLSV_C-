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
    public partial class QLNamHoc : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLNamHoc()
        {
            InitializeComponent();
        }
        private void QLNamHoc_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void LayDuLieu()
        {
            var query = from nh in DataBase.tblNamHocs
                        select nh;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                txtNamHoc.Text = dgvUsers.Rows[i].Cells["NH_TenNamHoc"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NH_MoTa"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            txtNamHoc.Enabled = check;
            txtMoTa.Enabled = check;
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
                tblNamHoc nh = new tblNamHoc();
                nh.NH_TenNamHoc = txtNamHoc.Text;
                nh.NH_MoTa = txtMoTa.Text;
                DataBase.tblNamHocs.Add(nh);
                DataBase.SaveChanges();
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NH_ID"].Value);
                    var query = from nh in DataBase.tblNamHocs
                                where (nh.NH_ID == id)
                                select nh;
                    tblNamHoc namhoc = query.First();
                    namhoc.NH_TenNamHoc = txtNamHoc.Text;
                    namhoc.NH_MoTa = txtMoTa.Text;
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
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["NH_ID"].Value);
                var query = from nh in DataBase.tblNamHocs
                            where (nh.NH_ID == id)
                            select nh;
                DataBase.tblNamHocs.Remove(query.First());
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
            string TenNamHoc = txtTimKiem.Text.Trim();
            var query = from nh in DataBase.tblNamHocs
                        where nh.NH_TenNamHoc.Contains(TenNamHoc)
                        select nh;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
