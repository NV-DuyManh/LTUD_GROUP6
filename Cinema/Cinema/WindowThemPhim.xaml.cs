using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
namespace Cinema
{
    /// <summary>
    /// Interaction logic for WindowThemPhim.xaml
    /// </summary>
    public partial class WindowThemPhim : Window
    {
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBRapPhim.mdf;Integrated Security=True;Connect Timeout=30";
        public bool isEdit = false;
        public int MaPhim;
        public WindowThemPhim()
        {
            InitializeComponent();
        }
        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();

                    if (!isEdit)
                    {
                        // THÊM MỚI
                        string getMaxIdQuery = "SELECT ISNULL(MAX(ma_phim),0) + 1 FROM phim";
                        SqlCommand getMaxCmd = new SqlCommand(getMaxIdQuery, conn);
                        int newMaPhim = (int)getMaxCmd.ExecuteScalar();

                        string query = @"INSERT INTO phim
                                 (ma_phim, ten_phim, ma_the_loai, thoi_luong, ngay_khoi_chieu, trang_thai)
                                 VALUES
                                 (@ma, @ten, @matl, @thoiluong, @ngay, @trangthai)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ma", newMaPhim);
                        cmd.Parameters.AddWithValue("@ten", txtTenPhim.Text);
                        cmd.Parameters.AddWithValue("@matl", int.Parse(txtMaTheLoai.Text));
                        cmd.Parameters.AddWithValue("@thoiluong", int.Parse(txtThoiLuong.Text));
                        cmd.Parameters.AddWithValue("@ngay", dpNgayKhoiChieu.SelectedDate);
                        cmd.Parameters.AddWithValue("@trangthai", cbTrangThai.Text);

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // CẬP NHẬT
                        string query = @"UPDATE phim
                                 SET ten_phim = @ten,
                                     ma_the_loai = @matl,
                                     thoi_luong = @thoiluong,
                                     ngay_khoi_chieu = @ngay,
                                     trang_thai = @trangthai
                                 WHERE ma_phim = @ma";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ma", MaPhim);
                        cmd.Parameters.AddWithValue("@ten", txtTenPhim.Text);
                        cmd.Parameters.AddWithValue("@matl", int.Parse(txtMaTheLoai.Text));
                        cmd.Parameters.AddWithValue("@thoiluong", int.Parse(txtThoiLuong.Text));
                        cmd.Parameters.AddWithValue("@ngay", dpNgayKhoiChieu.SelectedDate);
                        cmd.Parameters.AddWithValue("@trangthai", cbTrangThai.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
