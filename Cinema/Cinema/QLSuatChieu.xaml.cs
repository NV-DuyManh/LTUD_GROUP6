using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cinema
{
    public partial class QLSuatChieu : Page
    {
        DBRapPhimEntities db = new DBRapPhimEntities();

        public QLSuatChieu()
        {
            InitializeComponent();
            LoadDuLieuBieuMau();
            ThucHienLoc();
        }

        private void LoadDuLieuBieuMau()
        {
            try
            {
                cmb_Phim.ItemsSource = db.phim.ToList();
                cmb_Phim.DisplayMemberPath = "ten_phim";
                cmb_Phim.SelectedValuePath = "ma_phim";

                cmb_Phong.ItemsSource = db.phongchieu.ToList();
                cmb_Phong.DisplayMemberPath = "ten_phong";
                cmb_Phong.SelectedValuePath = "ma_phong";

                var danhSachLoc = db.phongchieu.ToList();
                phongchieu tatCa = new phongchieu();
                tatCa.ma_phong = 0;
                tatCa.ten_phong = "Tất cả phòng";
                danhSachLoc.Insert(0, tatCa);

                lst_LocPhong.ItemsSource = danhSachLoc;
                lst_LocPhong.DisplayMemberPath = "ten_phong";
                lst_LocPhong.SelectedValuePath = "ma_phong";

                lst_LocPhong.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- HÀM LỌC CHỐNG LỖI 100% ---
        private void ThucHienLoc()
        {
            // CHỐT CHẶN BẢO VỆ: Nếu giao diện chưa vẽ xong thì không cho chạy hàm lọc
            if (txt_tim_kiem == null || lst_LocPhong == null || dtg_suat_chieu == null) return;

            try
            {
                var query = db.lichchieu.AsQueryable();

                if (lst_LocPhong.SelectedValue != null)
                {
                    int maPhongChon = (int)lst_LocPhong.SelectedValue;
                    if (maPhongChon > 0)
                    {
                        query = query.Where(x => x.ma_phong == maPhongChon);
                    }
                }

                string tuKhoa = txt_tim_kiem.Text.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(tuKhoa) && tuKhoa != "🔍 tìm kiếm tên phim...")
                {
                    // Lọc an toàn: Kiểm tra x.phim != null để tránh lỗi do rác database
                    query = query.Where(x => x.phim != null && x.phim.ten_phim.ToLower().Contains(tuKhoa));
                }

                var ketQua = query.ToList();
                dtg_suat_chieu.ItemsSource = ketQua;
                txt_ket_qua.Text = $"Đang hiển thị {ketQua.Count} suất chiếu.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc dữ liệu: " + ex.Message);
            }
        }

        private void lst_LocPhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThucHienLoc();
        }

        private void txt_tim_kiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            ThucHienLoc();
        }

        private void txt_tim_kiem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt_tim_kiem.Text == "🔍 Tìm kiếm tên phim...")
            {
                txt_tim_kiem.Text = "";
                txt_tim_kiem.Foreground = Brushes.Black;
            }
        }

        private void txt_tim_kiem_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_tim_kiem.Text))
            {
                txt_tim_kiem.Text = "🔍 Tìm kiếm tên phim...";
                txt_tim_kiem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#95A5A6"));
                ThucHienLoc();
            }
        }

        private void btn_LuuNhanh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmb_Phim.SelectedValue == null || cmb_Phong.SelectedValue == null || dp_NgayChieu.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ Phim, Phòng và Ngày chiếu!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                lichchieu lcMoi = new lichchieu();
                lcMoi.ma_phim = (int)cmb_Phim.SelectedValue;
                lcMoi.ma_phong = (int)cmb_Phong.SelectedValue;
                lcMoi.ngay_chieu = dp_NgayChieu.SelectedDate.Value;
                lcMoi.gio_bat_dau = TimeSpan.Parse(txt_GioChieu.Text);
                lcMoi.gia_ve_co_ban = decimal.Parse(txt_GiaVe.Text);
                lcMoi.nguoi_lap_lich = 1;

                db.lichchieu.Add(lcMoi);
                db.SaveChanges();

                MessageBox.Show("Tạo suất chiếu mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                ThucHienLoc();
                pnl_NhapLieu.Visibility = Visibility.Collapsed;
            }
            catch (FormatException)
            {
                MessageBox.Show("Giờ chiếu (VD: 19:30) hoặc Giá vé nhập sai định dạng!", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu vào DB: " + ex.Message, "Lỗi");
            }
        }

        private void btn_xoa_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            lichchieu suatChieuCanXoa = btn.DataContext as lichchieu;

            if (suatChieuCanXoa != null)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa suất chiếu phim '{suatChieuCanXoa.phim.ten_phim}' lúc {suatChieuCanXoa.gio_bat_dau} không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.lichchieu.Remove(suatChieuCanXoa);
                        db.SaveChanges();
                        ThucHienLoc();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                    }
                }
            }
        }

        private void btn_hien_form_them_Click(object sender, RoutedEventArgs e)
        {
            pnl_NhapLieu.Visibility = Visibility.Visible;
        }

        private void btn_HuyNhap_Click(object sender, RoutedEventArgs e)
        {
            pnl_NhapLieu.Visibility = Visibility.Collapsed;
        }

        private void FastEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_LuuNhanh_Click(sender, e);
            }
        }
    }
}