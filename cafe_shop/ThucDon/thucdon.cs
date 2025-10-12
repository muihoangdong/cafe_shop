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

namespace cafe_shop.ThucDon
{
    public partial class thucdon : UserControl
    {
        private DataTable DGV2 = new DataTable();
        public thucdon()
        {
            InitializeComponent();
            btnTim.Click += button1_Click;
            Load += thucdon_Load;
            InitDGV2();
        }

        private void InitDGV2()
        {
            if (!DGV2.Columns.Contains("MaMon"))
            {
                DGV2.Columns.Add("MaMon", typeof(int));
                DGV2.Columns.Add("TenMon", typeof(string));
                DGV2.Columns.Add("DonGia", typeof(decimal));
                DGV2.Columns.Add("LoaiMon", typeof(string));
                DGV2.Columns.Add("SoLuong", typeof(int));
                DGV2.Columns.Add("ThanhTien", typeof(decimal));
                dgv2.DataSource = DGV2;
            }
        }
        private void LoadNguyenLieu(string keyword)
        {
            string query = "SELECT MaMon, TenMon, DonGia, LoaiMon FROM ThucDon";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query += " WHERE MaMon LIKE @kw OR TenMon LIKE @kw OR DonGia LIKE @kw OR LoaiMon LIKE @kw";
                SqlParameter[] para = {
                    new SqlParameter("@kw", "%" + keyword + "%")
                };
                dgv1.DataSource = SqlHelper.ExecuteQuery(query, para);
            }
            else
            {
                dgv1.DataSource = SqlHelper.ExecuteQuery(query);
            }

            dgv1.Columns["MaMon"].HeaderText = "Mã Món";
            dgv1.Columns["TenMon"].HeaderText = "Tên Món";
            dgv1.Columns["DonGia"].HeaderText = "Giá";
            dgv1.Columns["LoaiMon"].HeaderText = "Loại";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadNguyenLieu(txtTuKhoa.Text);
        }


        //Chọn
        private void button4_Click(object sender, EventArgs e)
        {
            if (dgv1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món từ bảng bên trái!");
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!");
                return;
            }

            int maMon = Convert.ToInt32(dgv1.CurrentRow.Cells["MaMon"].Value);
            string tenMon = dgv1.CurrentRow.Cells["TenMon"].Value.ToString();
            decimal donGia = Convert.ToDecimal(dgv1.CurrentRow.Cells["DonGia"].Value);
            string loaiMon = dgv1.CurrentRow.Cells["LoaiMon"].Value.ToString();
            decimal thanhTien = donGia * soLuong;

            DGV2.Rows.Add(maMon, tenMon, donGia, loaiMon, soLuong, thanhTien);
            TinhTongTien();
        }
        private void TinhTongTien()
        {
            decimal tongTien = 0;
            int tongSL = 0;

            foreach (DataRow row in DGV2.Rows)
            {
                if (row["ThanhTien"] != DBNull.Value)
                    tongTien += Convert.ToDecimal(row["ThanhTien"]);

                if (row["SoLuong"] != DBNull.Value)
                    tongSL += Convert.ToInt32(row["SoLuong"]);
            }

            lblTongTien.Text = tongTien.ToString("N0");
            lblTongSL.Text = tongSL.ToString();
        }
        // hủy
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
                decimal tongTien = 0;
                foreach (DataRow row in DGV2.Rows)
                {
                    tongTien += Convert.ToDecimal(row["ThanhTien"]);
                }

                string insertHoaDon = @"
                    INSERT INTO HoaDon (NgayLap, TongTien)
                    OUTPUT INSERTED.MaHoaDon
                    VALUES (@ngay, @tong)";

                SqlParameter[] hoaDonParams = {
                    new SqlParameter("@ngay", DateTime.Now),
                    new SqlParameter("@tong", tongTien)
                };

                int maHD = Convert.ToInt32(SqlHelper.ExecuteScalar(insertHoaDon, hoaDonParams));

                foreach (DataRow row in DGV2.Rows)
                {
                    string insertCT = @"INSERT INTO ChiTietHoaDon (MaHoaDon, MaMon, SoLuong, DonGia)
                                        VALUES (@mahd, @mamon, @sl, @gia)";

                    SqlParameter[] para = {
                        new SqlParameter("@mahd", maHD),
                        new SqlParameter("@mamon", row["MaMon"]),
                        new SqlParameter("@sl", row["SoLuong"]),
                        new SqlParameter("@gia", row["DonGia"])
                    };

                    SqlHelper.ExecuteNonQuery(insertCT, para);
                }

                MessageBox.Show("Đã thanh toán và lưu hóa đơn!");
                DGV2.Clear();
                TinhTongTien();
            }
        }

        private void thucdon_Load(object sender, EventArgs e)
        {
            LoadNguyenLieu("");
            InitDGV2();
        }
    }
}
