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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace cafe_shop.LichTrinh
{
    public partial class lichtrinh : UserControl
    {
        public lichtrinh()
        {
            InitializeComponent();
            Load += lichtrinh_Load;
        }

        private void lichtrinh_Load(object sender, EventArgs e)
        {
            LoadCaLam();
            LoadData("");
            txtTimKiem.TextChanged += textBox1_TextChanged;
            dgvLichTrinh.CellClick += dgvLichLamViec_CellClick;
        }

        private void LoadCaLam()
        {
            comboBoxCa.Items.Clear();
            comboBoxCa.Items.AddRange(new string[] { "Sáng", "Chiều", "Tối" });
            comboBoxCa.SelectedIndex = 0;
        }
        private void LoadData(string keyword)
        {
            string query = @"SELECT LLV.MaLich, NV.MaNV, NV.TenNV, LLV.Ca, LLV.NgayLam
                     FROM LichLamViec LLV
                     JOIN NhanVien NV ON LLV.MaNV = NV.MaNV
                     WHERE NV.TenNV LIKE @kw";

            SqlParameter[] parameters = {
                new SqlParameter("@kw", "%" + keyword + "%")
            };

            DataTable dt = SqlHelper.ExecuteQuery(query, parameters);
            dgvLichTrinh.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có nhân viên nào khớp với từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearForm()
        {
            txtMaNV.Clear();
            comboBoxCa.SelectedIndex = 0;
            dtpNgayLam.Value = DateTime.Now;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtTimKiem.Text);
        }

        private void dgvLichLamViec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLichTrinh.Rows[e.RowIndex];
                txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                txtTenNV.Text = row.Cells["TenNV"].Value.ToString();
                comboBoxCa.SelectedItem = row.Cells["Ca"].Value.ToString();
                dtpNgayLam.Value = Convert.ToDateTime(row.Cells["NgayLam"].Value);
            }
        }


        // thêm
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.");
                return;
            }

            // Kiểm tra mã nhân viên có tồn tại không
            string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @ma";
            SqlParameter[] checkParams = {
        new SqlParameter("@ma", txtMaNV.Text.Trim())
    };

            int count = (int)SqlHelper.ExecuteScalar(checkQuery, checkParams);
            if (count == 0)
            {
                MessageBox.Show("Không tồn tại nhân viên này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm lịch làm việc nếu mã nhân viên tồn tại
            string query = "INSERT INTO LichLamViec (MaNV, Ca, NgayLam) VALUES (@manv, @ca, @ngay)";
            SqlParameter[] parameters = {
                new SqlParameter("@manv", txtMaNV.Text),
                new SqlParameter("@ca", comboBoxCa.SelectedItem.ToString()),
                new SqlParameter("@ngay", dtpNgayLam.Value.Date)
            };

            try
            {
                SqlHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Thêm thành công!");
                LoadData("");
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        //Xóa
        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvLichTrinh.CurrentRow == null)
            {
                MessageBox.Show("Chọn lịch để xóa!");
                return;
            }

            int maLich = Convert.ToInt32(dgvLichTrinh.CurrentRow.Cells["MaLich"].Value);
            string query = "DELETE FROM LichLamViec WHERE MaLich = @malich";
            SqlParameter[] parameters = {
                new SqlParameter("@malich", maLich)
            };

            SqlHelper.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Xóa thành công!");
            LoadData("");
            ClearForm();
        }

        //sửa
        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvLichTrinh.CurrentRow == null)
            {
                MessageBox.Show("Chọn lịch để sửa!");
                return;
            }

            if (txtMaNV.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.");
                return;
            }

            // Kiểm tra mã nhân viên có tồn tại không
            string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @ma";
            SqlParameter[] checkParams = {
                new SqlParameter("@ma", txtMaNV.Text.Trim())
            };

            int count = (int)SqlHelper.ExecuteScalar(checkQuery, checkParams);
            if (count == 0)
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thực hiện cập nhật nếu nhân viên tồn tại
            int maLich = Convert.ToInt32(dgvLichTrinh.CurrentRow.Cells["MaLich"].Value);
            string query = "UPDATE LichLamViec SET MaNV = @manv, Ca = @ca, NgayLam = @ngay WHERE MaLich = @malich";
            SqlParameter[] parameters = {
                new SqlParameter("@manv", txtMaNV.Text),
                new SqlParameter("@ca", comboBoxCa.SelectedItem.ToString()),
                new SqlParameter("@ngay", dtpNgayLam.Value.Date),
                new SqlParameter("@malich", maLich)
            };

            try
            {
                SqlHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Sửa thành công!");
                LoadData("");
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
