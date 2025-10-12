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

namespace cafe_shop.KhoHang
{
    public partial class NhapKhoNguyenLieu : UserControl
    {
        public NhapKhoNguyenLieu()
        {
            InitializeComponent();
        }

        private void NhapKhoNguyenLieu_Load(object sender, EventArgs e)
        {
            LoadNguyenLieu();
        }
        private void LoadNguyenLieu()
        {
            string query = "SELECT TenNguyenLieu, XuatXu, SoLuongTon FROM NguyenLieu";
            dgvNguyenLieu.DataSource = SqlHelper.ExecuteQuery(query);
        }

        private void dgvNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNguyenLieu.Rows[e.RowIndex];
                lblTenNL.Text = row.Cells["TenNguyenLieu"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuongTon"].Value.ToString();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (lblTenNL.Text == "")
            {
                MessageBox.Show("Chọn nguyên liệu cần cập nhật!");
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuongMoi))
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            string query = "UPDATE NguyenLieu SET SoLuongTon = @sl WHERE TenNguyenLieu = @ten";
            SqlParameter[] parameters = {
                new SqlParameter("@sl", soLuongMoi),
                new SqlParameter("@ten", lblTenNL.Text.Trim())
            };

            try
            {
                SqlHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Cập nhật thành công!");
                LoadNguyenLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
