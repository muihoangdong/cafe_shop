using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        private void dangky_Load(object sender, EventArgs e)
        {
            //combobox chọn năm sinh
            comboBox1.Items.Add("Chọn năm sinh"); // Dòng placeholder

            for (int year = 1950; year <= DateTime.Now.Year; year++)
            {
                comboBox1.Items.Add(year.ToString());
            }

            comboBox1.SelectedIndex = 0; // Hiển thị mặc định là dòng đầu

            //combobox chọn chức vụ
            comboBox2.Items.Add("Chọn chức vụ"); // Placeholder
            comboBox2.Items.Add("Quản lý");
            comboBox2.Items.Add("Nhân viên");
            comboBox2.Items.Add("Thu ngân");
            comboBox2.Items.Add("Pha chế");

            comboBox2.SelectedIndex = 0; // Mặc định hiển thị "Chọn chức vụ"
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string email = textBox1.Text.Trim();
            string tenNV = textBox2.Text.Trim();
            string tenDN = textBox3.Text.Trim();
            string namSinh = comboBox1.SelectedItem?.ToString() ?? "";
            string chucVu = comboBox2.SelectedItem?.ToString() ?? "";

            // Kiểm tra dữ liệu chưa nhập hoặc chọn
            if (email == "Nhập email đăng ký" || string.IsNullOrWhiteSpace(email) ||
                tenNV == "Nhập tên nhân viên" || string.IsNullOrWhiteSpace(tenNV) ||
                tenDN == "Nhập mã nhân viên" || string.IsNullOrWhiteSpace(tenDN) ||
                comboBox1.SelectedIndex == 0 || comboBox2.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Đăng ký thành công. Nhấn OK để đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                // Mở form đăng nhập (ví dụ: Form 'dangnhap')
                this.Hide();
                dangnhap dn = new dangnhap();
                this.Close();
            }
        }
    }
}
