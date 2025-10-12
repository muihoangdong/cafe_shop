using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cafe_shop.KhoHang
{
    public partial class khohang : UserControl
    {
        public khohang()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            NhapKho nhapKhoControl = new NhapKho();
            nhapKhoControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(nhapKhoControl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            NhapKhoNguyenLieu nhapKhoControl = new NhapKhoNguyenLieu();
            nhapKhoControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(nhapKhoControl);
        }

        private void khohang_Load(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            NhapKho nhapKhoControl = new NhapKho();
            nhapKhoControl.Dock = DockStyle.Fill;
            panel2.Controls.Add(nhapKhoControl);
        }
    }
}
