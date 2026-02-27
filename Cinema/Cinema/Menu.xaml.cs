using System;
using System.Windows;
using System.Windows.Controls;

namespace Cinema
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void BtnPhim_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị PagePhim vào trong cái khung MainFrame
            MainFrame.Content = new PagePhim();
        }

        private void BtnSanPham_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý sản phẩm đang được xây dựng!", "Thông báo");
        }
        // Quản lý suất chiếu
        private void BtnSuatChieu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new QLSuatChieu();
        }


        private void BtnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý tài khoản đang được xây dựng!", "Thông báo");
        }
    }
}