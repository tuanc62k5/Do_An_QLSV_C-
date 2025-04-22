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
    public partial class test : Form
    {
        DBservices db = new DBservices();
        public test()
        {
            InitializeComponent();
        }

        private void test_Load(object sender, EventArgs e)
        {
            LayKhoa();
        }
        private void LayKhoa()
        {
            string sql = "SELECT * FROM tblKhoa";
            cboKhoa.DisplayMember = "K_TenKhoa";
            cboKhoa.ValueMember = "K_ID";
            cboKhoa.DataSource = db.GetData(sql);
        }

        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedValue != null)
            {
                string id = cboKhoa.SelectedValue.ToString();
                string sql = string.Format("SELECT * FROM tblNganh WHERE (K_ID={0})", id);
                cboNganh.DisplayMember = "NG_TenNganh";
                cboNganh.ValueMember = "NG_ID";
                cboNganh.DataSource = db.GetData(sql);
            }
            else
            {
                cboNganh.DataSource = null;
            }
        }

        private void cboNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNganh.SelectedValue == null || cboNganh.SelectedValue is DataRowView) return;
            string id = cboNganh.SelectedValue.ToString();
            string sql = $"SELECT * FROM tblLopHanhChinh WHERE NG_ID= {id}";
            cboLop.DisplayMember = "LP_TenLop";
            cboLop.ValueMember = "LP_ID";
            cboLop.DataSource = db.GetData(sql);
        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboLop.SelectedValue == null || cboLop.SelectedValue is DataRowView) return;
            string id = cboLop.SelectedValue.ToString();
            string sql = $"SELECT * FROM tblSinhVien WHERE LP_ID= {id}";
            dgvUsers.DataSource = db.GetData(sql);
        }
    }
}
