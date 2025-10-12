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

namespace cafe_shop.HoaDon
{
    public partial class hoadon : UserControl
    {
        public hoadon()
        {
            InitializeComponent();
            Load += hoadon_Load;
        }

        private void hoadon_Load(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }
        private void LoadDanhSachHoaDon()
        {
            string query = "SELECT MaHoaDon, NgayLap, TongTien FROM HoaDon ORDER BY MaHoaDon DESC";
            dgvHoaDon.DataSource = SqlHelper.ExecuteQuery(query);

            dgvHoaDon.Columns["MaHoaDon"].HeaderText = "Mã hóa đơn";
            dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn!");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["MaHoaDon"].Value);
            LoadChiTietHoaDon(maHD);
        }
        private void LoadChiTietHoaDon(int maHoaDon)
        {
            string query = @"
            SELECT 
                TD.TenMon,
                CTHD.DonGia,
                CTHD.SoLuong,
                (CTHD.DonGia * CTHD.SoLuong) AS ThanhTien
            FROM ChiTietHoaDon CTHD
            JOIN ThucDon TD ON CTHD.MaMon = TD.MaMon
            WHERE CTHD.MaHoaDon = @mahd";

            SqlParameter[] para = {
            new SqlParameter("@mahd", maHoaDon)
        };

            dgvChiTiet.DataSource = SqlHelper.ExecuteQuery(query, para);

            dgvChiTiet.Columns["TenMon"].HeaderText = "Tên món";
            dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }
    }
}
