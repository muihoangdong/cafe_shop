using cafe_shop.LichTrinh;
using cafe_shop.NhanVien;
using cafe_shop.KhoHang;
using cafe_shop.Login;
using cafe_shop.TrangChu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cafe_shop.HoaDon;
using cafe_shop.ThucDon;

namespace cafe_shop.TrangChu
{
    public partial class trangchu : Form
    {
        private Size originalFormSize;
        private Dictionary<Control, Rectangle> originalControlBounds = new Dictionary<Control, Rectangle>();
        private Dictionary<Control, float> originalFontSizes = new Dictionary<Control, float>();
        public trangchu()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.Resize += Form1_Resize;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            originalFormSize = this.Size;
            SaveOriginalBounds(this);

            ShowHomeScreen();
        }

        public void loadAll()
        {
            originalFormSize = this.Size;
            SaveOriginalBounds(this);
        }

        private void SaveOriginalBounds(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (!originalControlBounds.ContainsKey(c))
                {
                    originalControlBounds[c] = c.Bounds;
                    originalFontSizes[c] = c.Font.Size;
                }

                if (c.HasChildren)
                    SaveOriginalBounds(c);
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            float xRatio = (float)this.Width / originalFormSize.Width;
            float yRatio = (float)this.Height / originalFormSize.Height;

            ResizeControls(this, xRatio, yRatio);
            ResizeControls(panel1, xRatio, yRatio); // 👈 cần thiết
        }

        private void ResizeControls(Control parent, float xRatio, float yRatio)
        {
            foreach (Control c in parent.Controls)
            {
                if (originalControlBounds.ContainsKey(c))
                {
                    Rectangle original = originalControlBounds[c];
                    c.Left = (int)(original.Left * xRatio);
                    c.Top = (int)(original.Top * yRatio);
                    c.Width = (int)(original.Width * xRatio);
                    c.Height = (int)(original.Height * yRatio);

                    // Resize font đúng cách từ font gốc đã lưu
                    if (originalFontSizes.ContainsKey(c))
                    {
                        float originalFontSize = originalFontSizes[c];
                        c.Font = new Font(c.Font.FontFamily, originalFontSize * Math.Min(xRatio, yRatio), c.Font.Style);
                    }
                }

                // 🔁 Gọi đệ quy cho control con
                if (c.HasChildren)
                {
                    ResizeControls(c, xRatio, yRatio);
                }
            }
        }

        private void LoadUserControl(UserControl uc)
        {
            panel1.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc);

            SaveOriginalBounds(uc); // lưu bounds
            ResizeControls(uc,
                (float)this.Width / originalFormSize.Width,
                (float)this.Height / originalFormSize.Height); // resize ngay
        }

        private void ShowHomeScreen()
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(pictureBox1);
            pictureBox1.Dock = DockStyle.Fill;
        }
        private void trangchu_btn_MouseClick(object sender, MouseEventArgs e)
        {
            ShowHomeScreen();

        }


        private void nhanvien_btn_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Controls.Clear();
            thongtinnv nv = new thongtinnv();
            nv.Dock = DockStyle.Fill;
            panel1.Controls.Add(nv);

            SaveOriginalBounds(nv);
        }

        private void khohang_btn_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Controls.Clear();
            khohang kh = new khohang();
            kh.Dock = DockStyle.Fill;
            panel1.Controls.Add(kh);

            SaveOriginalBounds(kh);
        }
        private void lichtrinh_btn_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Controls.Clear();
            lichtrinh lt = new lichtrinh();
            lt.Dock = DockStyle.Fill;
            panel1.Controls.Add(lt);

            SaveOriginalBounds(lt);
        }

        private void hoadon_btn_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Controls.Clear();
            hoadon hd = new hoadon();
            hd.Dock = DockStyle.Fill;
            panel1.Controls.Add(hd);

            SaveOriginalBounds(hd);
        }

        private void thucdon_btn_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.Controls.Clear();
            thucdon td = new thucdon();
            td.Dock = DockStyle.Fill;
            panel1.Controls.Add(td);

            SaveOriginalBounds(td);
        }

        private void thoatra_btn_MouseClick(object sender, MouseEventArgs e)
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
    }
}
