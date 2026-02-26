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
    /// Interaction logic for DangKi.xaml
    /// </summary>
    public partial class DangKi : Window
    {

        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBRapPhim.mdf;Integrated Security=True;";

        public DangKi()
        {
            InitializeComponent();
        }

        private void BtnConfirmRegister_Click(object sender, RoutedEventArgs e)
        {
            string tk = txtRegTaiKhoan.Text.Trim();
            string ht = txtRegHoTen.Text.Trim();
            string mk = txtRegMatKhau.Password;
            string nlmk = txtConfirmMatKhau.Password;

            if (string.IsNullOrEmpty(tk) || string.IsNullOrEmpty(mk))
            {
                MessageBox.Show("Tài khoản và mật khẩu không được để trống!");
                return;
            }

            if (mk != nlmk)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(strCon))
                {
                    sqlCon.Open();

                    string cmdId = "SELECT ISNULL(MAX(ma_nguoi_dung), 0) + 1 FROM nguoidung";
                    SqlCommand cmdGetId = new SqlCommand(cmdId, sqlCon);
                    int nextId = (int)cmdGetId.ExecuteScalar();

                    string query = "INSERT INTO nguoidung (ma_nguoi_dung, tai_khoan, mat_khau, ho_ten, chuc_vu) " +
                                   "VALUES (@id, @tk, @mk, @ht, 'Staff')";

                    using (SqlCommand cmd = new SqlCommand(query, sqlCon))
                    {
                        cmd.Parameters.AddWithValue("@id", nextId);
                        cmd.Parameters.AddWithValue("@tk", tk);
                        cmd.Parameters.AddWithValue("@mk", mk);
                        cmd.Parameters.AddWithValue("@ht", ht);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập ngay.");
                        this.Close(); // Đóng cửa sổ đăng ký
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: Tài khoản có thể đã tồn tại! \n" + ex.Message);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
