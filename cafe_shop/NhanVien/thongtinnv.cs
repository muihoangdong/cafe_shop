using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cafe_shop.NhanVien
{
    public partial class thongtinnv : UserControl
    {
        private string connectionString = @"Data Source=LAPTOP-A0S7TQ9Q\SQLEXPRESS01;Initial Catalog=QuanLyCaPhe;Integrated Security=True";
        public thongtinnv()
        {
            InitializeComponent();
            Load += thongtinnv_Load;
        }

        private void LoadData(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT MaNV, TenNV, Email, NamSinh,
                CASE WHEN GioiTinh = 1 THEN N'Nam' ELSE N'Nữ' END AS GioiTinh
                FROM NhanVien
                WHERE MaNV LIKE @ma";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@ma", "%" + maNV + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNhanVien.DataSource = dt;
            }
        }
        public class NhanVien
        {
            public string MaNV { get; set; }
            public string TenNV { get; set; }
            public string Email { get; set; }
            public string NamSinh { get; set; }
            public string GioiTinh { get; set; }
        }

        //thêm
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "" || txtTenNV.Text == "" || txtEmail.Text == "" || (!radNam.Checked && !radNu.Checked))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra mã nhân viên đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @ma";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ma", txtMaNV.Text.Trim());

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại!");
                        return;
                    }

                    string q = "INSERT INTO NhanVien (MaNV, TenNV, Email, NamSinh, GioiTinh, MatKhau) VALUES (@ma,@ten,@email,@nam,@gt,'123')";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@ma", txtMaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@ten", txtTenNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@nam", dateNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gt", radNam.Checked ? 1 : 0);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm thành công!");
                        LoadData(txtMaNVTim.Text);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập đúng thông tin!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message);
                }
            }
        }

        //xóa
        private void button3_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Chọn nhân viên để xóa!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string q = "DELETE FROM NhanVien WHERE MaNV=@ma";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@ma", txtMaNV.Text.Trim());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!");
                LoadData(txtMaNVTim.Text);
                ClearForm();
            }
        }

        //sửa
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra xem mã nhân viên có tồn tại không
                    string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @ma";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ma", txtMaNV.Text.Trim());

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Vui lòng nhập đúng thông tin! Không tìm thấy mã nhân viên.");
                        return;
                    }

                    string q = "UPDATE NhanVien SET TenNV=@ten, Email=@email, NamSinh=@nam, GioiTinh=@gt WHERE MaNV=@ma";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@ma", txtMaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@ten", txtTenNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@nam", dateNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gt", radNam.Checked ? 1 : 0);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Sửa thành công!");
                        LoadData(txtMaNVTim.Text);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập đúng thông tin!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }
        }

        private void thongtinnv_Load(object sender, EventArgs e)
        {
            LoadData(""); 
            txtMaNVTim.TextChanged += textBox1_TextChanged;
            dgvNhanVien.CellClick += dgvNhanVien_CellClick;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtMaNVTim.Text);
        }

        private void ClearForm()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtEmail.Text = "";
            dateNgaySinh.Value = new DateTime(2000, 1, 1);
            radNam.Checked = radNu.Checked = false;
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow dr = ((DataTable)dgvNhanVien.DataSource).Rows[e.RowIndex];
                txtMaNV.Text = dr["MaNV"].ToString();
                txtTenNV.Text = dr["TenNV"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                dateNgaySinh.Value = Convert.ToDateTime(dr["NamSinh"]);
                bool gt = dr["GioiTinh"].ToString() == "Nam";
                radNam.Checked = gt;
                radNu.Checked = !gt;
            }
        }
    }
}
