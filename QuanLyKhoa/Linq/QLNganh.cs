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
    public partial class QLNganh : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLNganh()
        {
            InitializeComponent();
        }
        private void QLNganh_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void LayDuLieu()
        {
            var query = from ng in DataBase.tblNganhs
                        select ng;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                cboKhoa.Text = dgvUsers.Rows[i].Cells["K_TenKhoa"].Value.ToString();
                txtNganh.Text = dgvUsers.Rows[i].Cells["NG_TenNganh"].Value.ToString();
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["NG_SoTinChi"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["NG_MoTa"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            cboKhoa.Enabled = check;
            txtNganh.Enabled = check;
            txtSoTinChi.Enabled = check;
            txtMoTa.Enabled = check;
            btnLuu.Enabled = check;
            btnThem.Enabled = !check;
            btnSua.Enabled = !check;
            btnXoa.Enabled = !check;
            btnThoat.Enabled = !check;
            btnLamMoi.Enabled = !check;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            AddNew = true;
            setEnable(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            AddNew = false;
            setEnable(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (AddNew)
            {
                tblNganh ng = new tblNganh();
                var khoa = DataBase.tblKhoas.FirstOrDefault(k => k.K_TenKhoa == cboKhoa.Text);
                if (khoa != null)
                {
                    ng.K_ID = khoa.K_ID;
                    ng.NG_TenNganh = txtNganh.Text.Trim();
                    ng.NG_SoTinChi = txtSoTinChi.Text.Trim();
                    ng.NG_MoTa = txtMoTa.Text.Trim();
                    DataBase.tblNganhs.Add(ng);
                    DataBase.SaveChanges();
                }
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int NG_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                    var khoa = DataBase.tblKhoas.FirstOrDefault(k => k.K_TenKhoa == cboKhoa.Text);
                    var nganh = DataBase.tblNganhs.FirstOrDefault(ng => ng.NG_ID == NG_ID);
                    if (nganh != null)
                    {
                        nganh.K_ID = khoa.K_ID;
                        nganh.NG_TenNganh = txtNganh.Text.Trim();
                        nganh.NG_SoTinChi = txtSoTinChi.Text.Trim();
                        nganh.NG_MoTa = txtMoTa.Text.Trim();
                    }
                }
            }
            DataBase.SaveChanges();
            LayDuLieu();
            setEnable(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int NG_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                var query = from xoa in DataBase.View_Nganh
                            where (xoa.id == NG_ID)
                            select xoa;
                DataBase.View_Nganh.Remove(query.First());
                DataBase.SaveChanges();
                LayDuLieu();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LayDuLieu();
            txtTimKiem.Clear();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string TenNganh = txtTimKiem.Text.Trim();
            var query = from tim in DataBase.View_Nganh
                        where tim.NG_TenNganh.Contains(TenNganh)
                        select tim;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
