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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace cafe_shop.Login
{
    public partial class dangky : Form
    {
        public dangky()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void quaylai_btn_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            dangnhap dn = new dangnhap();
            this.Close();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "Nhập email đăng ký")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == "Nhập tên nhân viên")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }


        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox3.Text == "Nhập mã nhân viên")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        public bool CheckMaNV(string AC) // check maatj khaaur vaf taif khaorn
        {
            return Regex.IsMatch(AC, "^[0-9]{3}$");
        }
        public bool CheckEmail(string EM)
        {
            return Regex.IsMatch(EM, @"^[a-zA-Z0-9._]{3,20}@gmail.com(.vn|)$");
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string email = textBox1.Text.Trim();
            string tenNV = textBox2.Text.Trim();
            string maNV = textBox3.Text.Trim();
            DateTime ngaySinh = dateTimePicker1.Value;

            if (!CheckMaNV(maNV)) { MessageBox.Show("Vui lòng nhập mã nhân viên 3 ký tự"); return; }
            if (!CheckEmail(email)) { MessageBox.Show("Vui lòng nhập đúng định dạng email"); return; }

            int gioiTinh = radioButton1.Checked ? 1 : 0;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(tenNV) || string.IsNullOrWhiteSpace(maNV) || (!radioButton1.Checked && !radioButton2.Checked))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            using (SqlConnection conn = SqlHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO NhanVien (MaNV, TenNV, Email, MatKhau, NamSinh, GioiTinh) " +
                                   "VALUES (@ma, @ten, @email, @matkhau, @ngay, @gioiTinh)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ma", maNV);
                    cmd.Parameters.AddWithValue("@ten", tenNV);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@matkhau", "12345");
                    cmd.Parameters.AddWithValue("@ngay", ngaySinh);
                    cmd.Parameters.AddWithValue("@gioiTinh", gioiTinh);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đăng ký thành công! Mật khẩu mặc định là: 12345");

                    this.Hide();
                    new dangnhap().ShowDialog();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
