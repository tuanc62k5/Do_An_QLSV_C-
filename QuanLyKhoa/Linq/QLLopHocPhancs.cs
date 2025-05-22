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
    public partial class QLLopHocPhancs : Form
    {
        QLSVEntities DataBase = new QLSVEntities();
        private bool AddNew = false;
        public QLLopHocPhancs()
        {
            InitializeComponent();
        }
        private void QLLopHocPhancs_Load(object sender, EventArgs e)
        {
            LoadCombobox();
            LayDuLieu();
            setEnable(false);
        }
        private void LoadCombobox()
        {
            cboHocPhan.DataSource = DataBase.tblHocPhans.ToList();
            cboHocPhan.DisplayMember = "HP_TenHocPhan";
            cboHocPhan.ValueMember = "HP_ID";

            cboNamHoc.DataSource = DataBase.tblNamHocs.ToList();
            cboNamHoc.DisplayMember = "NH_TenNamHoc";
            cboNamHoc.ValueMember = "NH_ID";

            cboHocKy.DataSource = DataBase.tblHocKies.ToList();
            cboHocKy.DisplayMember = "HK_TenHocKy";
            cboHocKy.ValueMember = "HK_ID";
        }
        private void LayDuLieu()
        {
            var query = from lhp in DataBase.tblLopHocPhans
                        join hp in DataBase.tblHocPhans on lhp.HP_ID equals hp.HP_ID
                        join nh in DataBase.tblNamHocs on lhp.NH_ID equals nh.NH_ID
                        join hk in DataBase.tblHocKies on lhp.HK_ID equals hk.HK_ID
                        select new
                        {
                            LHP_ID = lhp.LHP_ID,
                            HP_ID = hp.HP_ID,
                            NH_ID = nh.NH_ID,
                            HK_ID = hk.HK_ID,
                            TenHocPhan = hp.HP_TenHocPhan,
                            TenLopHP = lhp.LHP_TenLopHP,
                            GiangVien = lhp.LHP_GiangVien,
                            NamHoc = nh.NH_TenNamHoc,
                            HocKy = hk.HK_TenHocKy,
                            PhongHoc = lhp.LHP_PhongHoc,
                            NgayBatDau = lhp.LHP_NgayBatDau,
                            NgayKetThuc = lhp.LHP_NgayKetThuc
                        };

            dgvUsers.DataSource = query.ToList();
            dgvUsers.Columns["HP_ID"].Visible = false;
            dgvUsers.Columns["NH_ID"].Visible = false;
            dgvUsers.Columns["HK_ID"].Visible = false;
        }
        private void setEnable (bool enable)
        {
            cboHocPhan.Enabled = true;
            txtLopHP.Enabled = enable;
            txtGiangVien.Enabled = enable;
            cboNamHoc.Enabled = enable;
            cboHocKy.Enabled = enable;
            txtPhongHoc.Enabled = enable;
            timeNgayBatDau.Enabled = enable;
            timeNgayKetThuc.Enabled = enable;
            btnSave.Enabled = enable;
            btnAddNew.Enabled = !enable;
            btnEdit.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnExit.Enabled = !enable;
        }
        private void dgvUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cboHocPhan.SelectedValue = dgvUsers.Rows[i].Cells["HP_ID"].Value;
                cboNamHoc.SelectedValue = dgvUsers.Rows[i].Cells["NH_ID"].Value;
                cboHocKy.SelectedValue = dgvUsers.Rows[i].Cells["HK_ID"].Value;

                txtLopHP.Text = dgvUsers.Rows[i].Cells["TenLopHP"].Value?.ToString() ?? "";
                txtGiangVien.Text = dgvUsers.Rows[i].Cells["GiangVien"].Value?.ToString() ?? "";
                txtPhongHoc.Text = dgvUsers.Rows[i].Cells["PhongHoc"].Value?.ToString() ?? "";
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["NgayBatDau"].Value?.ToString(), out DateTime ngayBD))
                    timeNgayBatDau.Value = ngayBD;
                if (DateTime.TryParse(dgvUsers.Rows[i].Cells["NgayKetThuc"].Value?.ToString(), out DateTime ngayKT))
                    timeNgayKetThuc.Value = ngayKT;
            }
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
            int hpID = Convert.ToInt32(cboHocPhan.SelectedValue);
            int nhID = Convert.ToInt32(cboNamHoc.SelectedValue);
            int hkID = Convert.ToInt32(cboHocKy.SelectedValue);

            if (AddNew)
            {
                var newLHP = new tblLopHocPhan
                {
                    HP_ID = hpID,
                    NH_ID = nhID,
                    HK_ID = hkID,
                    LHP_TenLopHP = txtLopHP.Text.Trim(),
                    LHP_GiangVien = txtGiangVien.Text.Trim(),
                    LHP_PhongHoc = txtPhongHoc.Text.Trim(),
                    LHP_NgayBatDau = timeNgayBatDau.Value,
                    LHP_NgayKetThuc = timeNgayKetThuc.Value
                };

                DataBase.tblLopHocPhans.Add(newLHP);
            }
            else
            {
                if (dgvUsers.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["LHP_ID"].Value);
                    var lhp = DataBase.tblLopHocPhans.FirstOrDefault(p => p.LHP_ID == id);

                    if (lhp != null)
                    {
                        lhp.HP_ID = hpID;
                        lhp.NH_ID = nhID;
                        lhp.HK_ID = hkID;
                        lhp.LHP_TenLopHP = txtLopHP.Text.Trim();
                        lhp.LHP_GiangVien = txtGiangVien.Text.Trim();
                        lhp.LHP_PhongHoc = txtPhongHoc.Text.Trim();
                        lhp.LHP_NgayBatDau = timeNgayBatDau.Value;
                        lhp.LHP_NgayKetThuc = timeNgayKetThuc.Value;
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
                int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["LHP_ID"].Value);
                var lhp = DataBase.tblLopHocPhans.FirstOrDefault(x => x.LHP_ID == id);
                if (lhp != null)
                {
                    var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa lớp học phần này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        DataBase.tblLopHocPhans.Remove(lhp);
                        DataBase.SaveChanges();
                        LayDuLieu();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
