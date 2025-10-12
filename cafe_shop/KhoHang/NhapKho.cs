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
    public partial class NhapKho : UserControl
    {
        private DataTable bangTam = new DataTable();
        public NhapKho()
        {
            InitializeComponent();
            Load += NhapKho_Load;
            txtTim.TextChanged += txtTim_TextChanged;
        }

        private void NhapKho_Load(object sender, EventArgs e)
        {
            comboNhaCungCap.Items.AddRange(new string[] { "90S Coffee", "Trung Nguyên", "Vinacafe", "Mekong Coffee", "G7" });
            comboNhaCungCap.SelectedIndex = 0;
            LoadNguyenLieu("");
            InitBangTam();
        }
        private void InitBangTam()
        {
            if (bangTam.Columns.Count == 0)
            {
                bangTam.Columns.Add("MaNguyenLieu", typeof(string));
                bangTam.Columns.Add("GiaNhap", typeof(int));
                bangTam.Columns.Add("SoLuong", typeof(int));
                bangTam.Columns.Add("ThanhTien", typeof(int));
                dgv2.DataSource = bangTam;
            }
        }
        private void LoadNguyenLieu(string keyword)
        {
            string query = "SELECT MaNguyenLieu, TenNguyenLieu, XuatXu FROM NguyenLieu";

            // Nếu có tìm kiếm theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query += " WHERE TenNguyenLieu LIKE @kw OR XuatXu LIKE @kw";
                SqlParameter[] para = {
            new SqlParameter("@kw", "%" + keyword + "%")
        };
                dgv1.DataSource = SqlHelper.ExecuteQuery(query, para);
            }
            else
            {
                dgv1.DataSource = SqlHelper.ExecuteQuery(query);
            }

            dgv1.Columns["MaNguyenLieu"].HeaderText = "Mã NL";
            dgv1.Columns["TenNguyenLieu"].HeaderText = "Tên Nguyên Liệu";
            dgv1.Columns["XuatXu"].HeaderText = "Xuất Xứ";
        }

        //chọn
        private void button4_Click(object sender, EventArgs e)
        {
            if (dgv1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu từ bảng bên trái!");
                return;
            }
            string donGiaText = txtDonGia.Text.Trim();
            string soLuongText = txtSoLuong.Text.Trim();

            if (!decimal.TryParse(donGiaText, out decimal gia) || !int.TryParse(soLuongText, out int sl))
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng đơn giá và số lượng!");
                return;
            }

            string maNL = dgv1.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            decimal thanhTien = gia * sl;

            bangTam.Rows.Add(maNL, gia, sl, thanhTien);
            dgv2.DataSource = bangTam;

            TinhTongTien();
        }
        private void TinhTongTien()
        {
            decimal tongTien = 0;
            int tongSL = 0;

            foreach (DataRow row in bangTam.Rows)
            {
                if (row["ThanhTien"] != DBNull.Value)
                    tongTien += Convert.ToDecimal(row["ThanhTien"]);

                if (row["SoLuong"] != DBNull.Value)
                    tongSL += Convert.ToInt32(row["SoLuong"]);
            }

            lblTongTien.Text = tongTien.ToString("N0");
            lblTongSL.Text = tongSL.ToString();
        }


        //hủy
        private void button5_Click(object sender, EventArgs e)
        {
            if (dgv2.CurrentRow != null)
            {
                dgv2.Rows.Remove(dgv2.CurrentRow);
                TinhTongTien();
            }
        }


        //thanh toán
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn thanh toán không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                foreach (DataRow row in bangTam.Rows)
                {
                    string insertQuery = @"INSERT INTO NhapKho (MaNguyenLieu, GiaNhap, SoLuong)
                                   VALUES (@ma, @gia, @sl)";

                    SqlParameter[] parameters = {
                new SqlParameter("@ma", row["MaNguyenLieu"]),
                new SqlParameter("@gia", row["GiaNhap"]),
                new SqlParameter("@sl", row["SoLuong"]),
            };

                    SqlHelper.ExecuteNonQuery(insertQuery, parameters);
                }

                MessageBox.Show("Đã thanh toán thành công!");
                bangTam.Clear();
                TinhTongTien();
            }
        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            LoadNguyenLieu(txtTim.Text);
        }
    }
}
