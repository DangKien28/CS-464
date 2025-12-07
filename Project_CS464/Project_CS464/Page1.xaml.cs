using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Project_CS464.Model;

namespace Project_CS464
{
    public partial class Page1 : Page
    {
        private SanPhamEntities db = new SanPhamEntities();

        public Page1()
        {
            InitializeComponent();
            LoadData();
            cboHang.SelectionChanged += cboHang_SelectionChanged;
        }

        // ===================== LOAD DỮ LIỆU TỪ DB ======================
        private void LoadData()
        {
            dgSanPham.ItemsSource = db.SanPhams.ToList();
        }

        // ===================== TÌM KIẾM ======================
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LocDuLieu();
        }

        // ===================== LỌC THEO HÃNG ======================
        private void cboHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LocDuLieu();
        }

        // ===================== HÀM LỌC CHUNG ======================
        private void LocDuLieu()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string hang = (cboHang.SelectedItem as ComboBoxItem)?.Content.ToString();

            var query = db.SanPhams.AsQueryable();

            // Lọc theo ô tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(sp =>
                    (sp.MaSP != null && sp.MaSP.ToLower().Contains(search)) ||
                    (sp.MauGiay != null && sp.MauGiay.ToLower().Contains(search)) ||
                    (sp.Hang != null && sp.Hang.ToLower().Contains(search))
                );
            }

            // Lọc theo hãng
            if (hang != "Tất cả hãng" && hang != null)
            {
                query = query.Where(sp => sp.Hang == hang);
            }

            dgSanPham.ItemsSource = query.ToList();
        }

        // ===================== BUTTON SỬA ======================
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var sp = (sender as Button).DataContext as SanPham;

            MessageBox.Show($"Sửa thành công");

          
        }

        // ===================== BUTTON XÓA ======================
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var sp = (sender as Button).DataContext as SanPham;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm {sp.MaSP}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                db.SanPhams.Remove(sp);
                db.SaveChanges();

                LocDuLieu(); // Cập nhật lại bảng
            }
        }

        // ===================== NHẬP HÀNG MỚI ======================
        private void btnNhapHang_Click(object sender, RoutedEventArgs e)
        {

            NhapHangMoi nh = new NhapHangMoi();
            bool? result = nh.ShowDialog(); // Mở modal window

            if (result == true) // Nếu lưu thành công
            {
                LocDuLieu(); // Cập nhật DataGrid ngay
            }
        }
    }
}
