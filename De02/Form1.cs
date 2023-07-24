using De02.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace De02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public ListViewItem listItem = new ListViewItem();

        private void reset_textbox()
        {
            txtMaSP.Text = "";
            txtTenSP.Text = "";
        }
        private void DATA_FOR_CMB()
        {
            List<LoaiSP> a = new QLSanpham().LoaiSPs.Select(s=>s).ToList();
            cboLoaiSP.DataSource = a;
            cboLoaiSP.DisplayMember = "TenLoai"; 
            cboLoaiSP.ValueMember = "MaLoai";
        }
        private string Ten_SP(string s) 
        {
            QLSanpham con = new QLSanpham();
             return s = con.LoaiSPs.FirstOrDefault(a =>a.MaLoai==s).TenLoai.ToString();
        }
        private void DATA()
        {
            lvSanpham.Items.Clear();
            QLSanpham con = new QLSanpham();
            var list = con.Sanphams.Select(s=>s).ToList();
            foreach( var s in list )
            {
                listItem = new ListViewItem(s.MaSP);
                listItem.SubItems.Add(s.TenSP);
                listItem.SubItems.Add(s.Ngaynhap.ToString().Substring(0, s.Ngaynhap.ToString().IndexOf(' ')));
                listItem.SubItems.Add(Ten_SP(s.MaLoai));
                lvSanpham.Items.Add(listItem);
            }
            reset_textbox();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DATA();
            DATA_FOR_CMB();
        }

        private void lvSanpham_ItemActivate(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                ListViewItem a = lvSanpham.SelectedItems[0];

                txtMaSP.Text = a.SubItems[0].Text;
                txtTenSP.Text = a.SubItems[1].Text;
                dtNgaynhap.Text = a.SubItems[2].Text;
                cboLoaiSP.Text = a.SubItems[3].Text;
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                string tmp = ((cboLoaiSP.SelectedItem as LoaiSP).MaLoai.ToString()).Trim();
                QLSanpham con = new QLSanpham();
                if (!con.Sanphams.Any(x => x.MaSP == txtMaSP.Text))
                {
                    Sanpham sp = new Sanpham()
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        Ngaynhap = DateTime.Parse(dtNgaynhap.Text),
                        MaLoai = tmp
                    };
                    //
                    con.Sanphams.Add(sp);
                    MessageBox.Show("Thêm thành công!");
                    con.SaveChanges();
                    DATA();
                }
                else
                {
                    MessageBox.Show("Mã sản phãm đã tồn tại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                
                QLSanpham con = new QLSanpham();
                Sanpham update = con.Sanphams.FirstOrDefault(p => p.MaSP == txtMaSP.Text);
                if (update != null)
                {
                    update.TenSP= txtTenSP.Text;
                    update.Ngaynhap = DateTime.Parse(dtNgaynhap.Text);
                    string tmp = ((cboLoaiSP.SelectedItem as LoaiSP).MaLoai.ToString()).Trim();
                    update.MaLoai = tmp;
                    //
                    MessageBox.Show("Sửa thành công!");
                    con.SaveChanges();
                    DATA();

                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin sản phẩm!");
                }
                DATA();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string message, title;
            MessageBoxButtons buttons;
            DialogResult result;
            try
            {
                QLSanpham con = new QLSanpham();
                Sanpham update = con.Sanphams.FirstOrDefault(p => p.MaSP == txtMaSP.Text);
                if (update != null)
                {
                    message = "Bạn có muốn xóa sản phẩm này không";
                    title = "Cảnh báo";
                    buttons = MessageBoxButtons.YesNo;
                    result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        con.Sanphams.Remove(update);
                        con.SaveChanges();
                        DATA();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin nhân viên!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }
}
