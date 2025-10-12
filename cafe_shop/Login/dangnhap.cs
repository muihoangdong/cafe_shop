using cafe_shop.Login;
using cafe_shop.TrangChu;
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

namespace cafe_shop
{
    public partial class dangnhap : Form
    {
        public dangnhap()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "Nhập mã nhân viên")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == "Nhập mật khẩu")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.UseSystemPasswordChar = true;
            }
        }

        
        private void dangky_btn_MouseClick(object sender, MouseEventArgs e)
        {
            dangky dk = new dangky();
            this.Hide();
            dk.ShowDialog();
            this.Show();
        }

        private void XuLyDangNhap()
        {
            string maNV = textBox1.Text.Trim();
            string matKhau = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            using (SqlConnection conn = SqlHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @maNV AND MatKhau = @matKhau";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maNV", maNV);
                    cmd.Parameters.AddWithValue("@matKhau", matKhau);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        this.Hide();
                        trangchu tc = new trangchu();
                        tc.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sai mã nhân viên hoặc mật khẩu!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dangnhap_btn_MouseClick(object sender, MouseEventArgs e)
        {
            XuLyDangNhap();
        }

        private void quenmatkhau_btn_MouseClick(object sender, MouseEventArgs e)
        {
            quenmatkhau qmk = new quenmatkhau();
            this.Hide();
            qmk.ShowDialog();
            this.Show();
        }

        private void thoat_btn_MouseClick(object sender, MouseEventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn thoát không?",
        "Thông báo",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dangnhap_btn_Enter(object sender, EventArgs e)
        {
            XuLyDangNhap();
        }
    }
}
