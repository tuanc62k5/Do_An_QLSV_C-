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
    public partial class QLHocPhan : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLHocPhan()
        {
            InitializeComponent();
        }
        private void QLHocPhan_Load(object sender, EventArgs e)
        {
            LoadNganh();
            LayDuLieu();
        }
        private void LoadNganh()
        {
            var nganh = from ng in DataBase.tblNganhs
                        select ng.NG_TenNganh;
            cboNganh.DataSource = nganh.ToList();
        }
        private void LayDuLieu()
        {
            var query = from hp in DataBase.tblHocPhans
                        select hp;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                cboNganh.Text = dgvUsers.Rows[i].Cells["NG_TenNganh"].Value.ToString();
                txtHocPhan.Text = dgvUsers.Rows[i].Cells["HP_TenHocPhan"].Value.ToString();
                txtSoTinChi.Text = dgvUsers.Rows[i].Cells["HP_SoTinChi"].Value.ToString();
                txtSoTietLyThuyet.Text = dgvUsers.Rows[i].Cells["HP_SoTietLyThuyet"].Value.ToString();
                txtSoTietThucHanh.Text = dgvUsers.Rows[i].Cells["HP_SoTietThucHanh"].Value.ToString();
                txtMoTa.Text = dgvUsers.Rows[i].Cells["HP_MoTa"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            cboNganh.Enabled = check;
            txtHocPhan.Enabled = check;
            txtSoTinChi.Enabled = check;
            txtSoTietLyThuyet.Enabled = check;
            txtSoTietThucHanh.Enabled = check;
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
                tblHocPhan hp = new tblHocPhan();
                var nganh = DataBase.tblNganhs.FirstOrDefault(ng => ng.NG_TenNganh == cboNganh.Text);
                if (nganh != null)
                {
                    hp.NG_ID = nganh.NG_ID;
                    hp.HP_TenHocPhan = txtHocPhan.Text.Trim();
                    hp.HP_SoTinChi = int.Parse(txtSoTinChi.Text.Trim());
                    hp.HP_SoTietLyThuyet = int.Parse(txtSoTietLyThuyet.Text.Trim());
                    hp.HP_SoTietThucHanh = int.Parse(txtSoTietThucHanh.Text.Trim());
                    hp.HP_MoTa = txtMoTa.Text.Trim();
                    DataBase.tblHocPhans.Add(hp);
                    DataBase.SaveChanges();
                }
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int HP_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                    var nganh = DataBase.tblNganhs.FirstOrDefault(ng => ng.NG_TenNganh == cboNganh.Text);
                    var hocphan = DataBase.tblHocPhans.FirstOrDefault(hp => hp.HP_ID == HP_ID);
                    if (hocphan != null && nganh != null)
                    {
                        hocphan.NG_ID = nganh.NG_ID;
                        hocphan.HP_TenHocPhan = txtHocPhan.Text.Trim();
                        hocphan.HP_SoTinChi = int.Parse(txtSoTinChi.Text.Trim());
                        hocphan.HP_SoTietLyThuyet = int.Parse(txtSoTietLyThuyet.Text.Trim());
                        hocphan.HP_SoTietThucHanh = int.Parse(txtSoTietThucHanh.Text.Trim());
                        hocphan.HP_MoTa = txtMoTa.Text.Trim();
                    }
                }
            }
            DataBase.SaveChanges();
            LayDuLieu();
            setEnable(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                int HP_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                var query = from xoa in DataBase.tblHocPhans
                            where xoa.HP_ID == HP_ID
                            select xoa;
                DataBase.tblHocPhans.Remove(query.First());
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
            string keyword = txtTimKiem.Text.Trim();
            var query = from hp in DataBase.View_HocPhan
                        where hp.HP_TenHocPhan.Contains(keyword)
                        select hp;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
