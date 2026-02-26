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

        // Quản lý phim
        private void BtnPhim_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý phim đang được xây dựng!", "Thông báo");
        }

        // Quản lý sản phẩm
        private void BtnSanPham_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý sản phẩm đang được xây dựng!", "Thông báo");
        }

        // Quản lý suất chiếu
        private void BtnSuatChieu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý suất chiếu đang được xây dựng!", "Thông báo");
        }

        // ✅ THÊM CÁI NÀY
        private void BtnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng Quản lý tài khoản đang được xây dựng!", "Thông báo");
        }
    }
}