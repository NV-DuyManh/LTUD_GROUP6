using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cinema
{
    public partial class QLTaiKhoan : Page
    {
        DBRapPhimEntities db = new DBRapPhimEntities();

        public QLTaiKhoan()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            db = new DBRapPhimEntities();
            dgNguoiDung.ItemsSource = db.nguoidung.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbContext = new DBRapPhimEntities())
                {
                    var moi = new nguoidung
                    {
                        tai_khoan = txtTaiKhoan.Text,
                        mat_khau = txtMatKhau.Text,
                        ho_ten = txtHoTen.Text,
                        chuc_vu = (cbChucVu.SelectedItem as ComboBoxItem)?.Content.ToString()
                    };
                    dbContext.nguoidung.Add(moi);
                    dbContext.SaveChanges();
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.InnerException?.Message ?? ex.Message;
                MessageBox.Show("Lỗi chi tiết: " + message);
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgNguoiDung.SelectedItem as nguoidung;
            if (selected != null)
            {
                var user = db.nguoidung.Find(selected.ma_nguoi_dung);
                user.tai_khoan = txtTaiKhoan.Text;
                user.mat_khau = txtMatKhau.Text;
                user.ho_ten = txtHoTen.Text;
                user.chuc_vu = (cbChucVu.SelectedItem as ComboBoxItem)?.Content.ToString();
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Cập nhật thành công!");
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgNguoiDung.SelectedItem as nguoidung;
            if (selected == null) return;

            if (MessageBox.Show($"Xóa tài khoản {selected.tai_khoan}?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    var userInDb = db.nguoidung.Find(selected.ma_nguoi_dung);
                    db.nguoidung.Remove(userInDb);
                    db.SaveChanges();
                    LoadData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dgNguoiDung_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgNguoiDung.SelectedItem as nguoidung;
            if (selected != null)
            {
                txtTaiKhoan.Text = selected.tai_khoan;
                txtHoTen.Text = selected.ho_ten;
                txtMatKhau.Text = selected.mat_khau;
                cbChucVu.Text = selected.chuc_vu;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = txtSearch.Text.ToLower();
            dgNguoiDung.ItemsSource = db.nguoidung.Where(u => u.ho_ten.ToLower().Contains(search) || u.tai_khoan.ToLower().Contains(search)).ToList();
        }

        void ClearFields()
        {
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            cbChucVu.SelectedIndex = -1;
            txtSearch.Clear();
        }

        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            LoadData();
        }
    }
}