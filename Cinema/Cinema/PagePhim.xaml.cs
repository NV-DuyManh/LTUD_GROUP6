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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
namespace Cinema
{
    /// <summary>
    /// Interaction logic for PagePhim.xaml
    /// </summary>
    public partial class PagePhim : Page
    {
        // Chuỗi kết nối đến DBRapPhim.mdf
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBRapPhim.mdf;Integrated Security=True;Connect Timeout=30";

        public PagePhim()
        {
            InitializeComponent();
            LoadDuLieuPhim(); // Gọi hàm tải dữ liệu ngay khi mở trang
        }

        private void LoadDuLieuPhim()
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();

                    // Câu lệnh SQL lấy dữ liệu từ bảng phim và đổi tên cột cho đẹp
                    string query = @"SELECT 
                                        ma_phim AS [Mã Phim], 
                                        ten_phim AS [Tên Phim], 
                                        ma_the_loai AS [Mã TL], 
                                        thoi_luong AS [Thời Lượng (Phút)], 
                                        ngay_khoi_chieu AS [Khởi Chiếu], 
                                        trang_thai AS [Trạng Thái] 
                                     FROM phim
                                     ORDER BY ma_phim ASC";

                    // Dùng SqlDataAdapter để lấy dữ liệu đổ vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Đưa dữ liệu lên bảng DataGrid trên giao diện
                    dgPhim.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu phim: \n" + ex.Message, "Lỗi Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            WindowThemPhim w = new WindowThemPhim();

            if (w.ShowDialog() == true)
            {
                LoadDuLieuPhim(); // reload lại bảng
            }
        }
        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgPhim.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phim cần sửa!");
                return;
            }

            DataRowView row = (DataRowView)dgPhim.SelectedItem;

            WindowThemPhim win = new WindowThemPhim();

            // Truyền dữ liệu qua form
            win.MaPhim = Convert.ToInt32(row["Mã Phim"]);
            win.txtTenPhim.Text = row["Tên Phim"].ToString();
            win.txtMaTheLoai.Text = row["Mã TL"].ToString();
            win.txtThoiLuong.Text = row["Thời Lượng (Phút)"].ToString();
            win.dpNgayKhoiChieu.SelectedDate =
                Convert.ToDateTime(row["Khởi Chiếu"]);

            win.cbTrangThai.Text = row["Trạng Thái"].ToString();

            win.isEdit = true; // đánh dấu đang sửa

            if (win.ShowDialog() == true)
            {
                LoadDuLieuPhim();
            }
        }
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgPhim.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phim cần xóa!");
                return;
            }

            DataRowView row = (DataRowView)dgPhim.SelectedItem;
            int maPhim = Convert.ToInt32(row["Mã Phim"]);

            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa phim này?",
                "Xác nhận",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(strCon))
                    {
                        conn.Open();
                        string query = "DELETE FROM phim WHERE ma_phim = @ma";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ma", maPhim);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Xóa thành công!");
                    LoadDuLieuPhim(); // Load lại DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
