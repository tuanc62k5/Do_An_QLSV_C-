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
    public partial class QLLopHanhChinh : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLLopHanhChinh()
        {
            InitializeComponent();
        }
        private void QLLopHanhChinh_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }
        private void LayDuLieu()
        {
            var query = from lp in DataBase.tblLopHanhChinhs
                        select lp;
            dgvUsers.DataSource = query.ToList();
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i > 0)
            {
                cboNganh.Text = dgvUsers.Rows[i].Cells["NG_TenNganh"].Value.ToString();
                txtLop.Text = dgvUsers.Rows[i].Cells["LP_TenLop"].Value.ToString();
                txtChuNhiem.Text = dgvUsers.Rows[i].Cells["LP_TenChuNhiem"].Value.ToString();
            }
        }
        private void setEnable(bool check)
        {
            txtTimKiem.Enabled = check;
            cboNganh.Enabled = check;
            txtLop.Enabled = check;
            txtChuNhiem.Enabled = check;
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
                tblLopHanhChinh lp = new tblLopHanhChinh();
                var nganh = DataBase.tblNganhs.FirstOrDefault(ng => ng.NG_TenNganh == cboNganh.Text);
                if (nganh != null)
                {
                    lp.NG_ID = nganh.NG_ID;
                    lp.LP_TenLop = txtLop.Text.Trim();
                    lp.LP_TenChuNhiem = txtChuNhiem.Text.Trim();
                    DataBase.tblLopHanhChinhs.Add(lp);
                    DataBase.SaveChanges();
                }
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int LP_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                    var nganh = DataBase.tblNganhs.FirstOrDefault(ng => ng.NG_TenNganh == cboNganh.Text);
                    var lop = DataBase.tblLopHanhChinhs.FirstOrDefault(lp => lp.LP_ID == LP_ID);
                    if (lop != null)
                    {
                        lop.NG_ID = nganh.NG_ID;
                        lop.LP_TenLop = txtLop.Text.Trim();
                        lop.LP_TenChuNhiem = txtChuNhiem.Text.Trim();
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
                int LP_ID = Convert.ToInt32(dgvUsers.CurrentRow.Cells["id"].Value);
                var query = from xoa in DataBase.View_LopHanhChinh
                            where (xoa.id == LP_ID)
                            select xoa;
                DataBase.View_LopHanhChinh.Remove(query.First());
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
            string TenLop = txtTimKiem.Text.Trim();
            var query = from tim in DataBase.View_LopHanhChinh
                        where tim.LP_TenLop.Contains(TenLop)
                        select tim;
            dgvUsers.DataSource = query.ToList();
        }
    }
}
