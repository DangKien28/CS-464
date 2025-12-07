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
using Project_CS464;

namespace Project_CS464
{
    // ĐỔI: Kế thừa từ Page thay vì Window
    public partial class QLyNVPage : Page
    {
        DB1Entities db = new DB1Entities();
        private NHANVIEN nvDangChon;

        // ĐỔI: Constructor tên theo Class
        public QLyNVPage()
        {
            InitializeComponent();
            loadData();
        }

        // === HELPER METHODS ===

        /// <summary>
        /// Lấy giới tính từ RadioButton
        /// </summary>
        private string LayGioiTinh()
        {
            if (rdb_nam.IsChecked == true) return "Nam";
            if (rdb_nu.IsChecked == true) return "Nữ";
            if (rdb_khac.IsChecked == true) return "Khác";
            return "";
        }

        /// <summary>
        /// Lấy ngày sinh từ DatePicker
        /// </summary>
        private DateTime? LayNgaySinh()
        {
            return dp_NgaySinh.SelectedDate;
        }

        /// <summary>
        /// Hiển thị danh sách nhân viên
        /// </summary>
        private void HienThiDanhSach()
        {
            DG_NhanVien.ItemsSource = null;
            DG_NhanVien.ItemsSource = db.NHANVIENs.ToList();
        }

        /// <summary>
        /// Xóa trắng các ô nhập liệu
        /// </summary>
        private void ClearInputs()
        {
            txt_MaNV.Clear();
            txt_HoTen.Clear();
            txt_DiaChi.Clear();
            rdb_nam.IsChecked = false;
            rdb_nu.IsChecked = false;
            rdb_khac.IsChecked = false;
            dp_NgaySinh.SelectedDate = null;
            nvDangChon = null;
        }

        /// <summary>
        /// Kiểm tra dữ liệu nhập vào có hợp lệ không
        /// </summary>
        private bool KiemTraDuLieuHopLe(string maNV, string hoTen)
        {
            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txt_MaNV.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ tên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txt_HoTen.Focus();
                return false;
            }

            return true;
        }

        // === LOAD DATA ===

        private void loadData()
        {
            try
            {
                HienThiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // === BUTTON EVENTS ===

        private void Button_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                db = new DB1Entities();
                loadData();
                ClearInputs();
                MessageBox.Show("Tải dữ liệu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Them(object sender, RoutedEventArgs e)
        {
            try
            {
                string maNV = txt_MaNV.Text.Trim();
                string hoTen = txt_HoTen.Text.Trim();

                // Kiểm tra dữ liệu hợp lệ
                if (!KiemTraDuLieuHopLe(maNV, hoTen))
                    return;

                // Kiểm tra MaNV trùng
                if (db.NHANVIENs.Any(x => x.MaNV == maNV))
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tạo nhân viên mới
                NHANVIEN nvMoi = new NHANVIEN
                {
                    MaNV = maNV,
                    TenNV = hoTen,
                    DiaChi = txt_DiaChi.Text,
                    GioiTinh = LayGioiTinh(),
                    NgaySinh = LayNgaySinh()
                };

                db.NHANVIENs.Add(nvMoi);
                db.SaveChanges();

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearInputs();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Sua(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nvDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string maNVMoi = txt_MaNV.Text.Trim();
                string hoTen = txt_HoTen.Text.Trim();

                // Kiểm tra dữ liệu hợp lệ
                if (!KiemTraDuLieuHopLe(maNVMoi, hoTen))
                    return;

                // Kiểm tra MaNV trùng (nếu thay đổi mã)
                if (maNVMoi != nvDangChon.MaNV && db.NHANVIENs.Any(nv => nv.MaNV == maNVMoi))
                {
                    MessageBox.Show("Mã nhân viên mới đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Cập nhật thông tin
                nvDangChon.MaNV = maNVMoi;
                nvDangChon.TenNV = hoTen;
                nvDangChon.DiaChi = txt_DiaChi.Text;
                nvDangChon.GioiTinh = LayGioiTinh();
                nvDangChon.NgaySinh = LayNgaySinh();

                db.SaveChanges();
                MessageBox.Show("Sửa nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearInputs();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa nhân viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_xoa(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nvDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa nhân viên " + nvDangChon.TenNV
                    + "?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.NHANVIENs.Remove(nvDangChon);
                    db.SaveChanges();
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearInputs();
                    loadData();
                    nvDangChon = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa nhân viên: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Tim(object sender, RoutedEventArgs e)
        {
            try
            {
                string maNVTim = txt_MaNV.Text.Trim();

                if (string.IsNullOrEmpty(maNVTim))
                {
                    MessageBox.Show("Vui lòng nhập Mã nhân viên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_MaNV.Focus();
                    return;
                }

                db = new DB1Entities();

                // Tìm kiếm
                var ketQua = db.NHANVIENs.Where(nv => nv.MaNV.Contains(maNVTim)).ToList();

                if (ketQua.Any())
                {
                    DG_NhanVien.ItemsSource = ketQua;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào có mã: " + maNVTim, "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DG_NhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DG_NhanVien.SelectedItem != null)
                {
                    nvDangChon = (NHANVIEN)DG_NhanVien.SelectedItem;

                    txt_MaNV.Text = nvDangChon.MaNV;
                    txt_HoTen.Text = nvDangChon.TenNV;
                    txt_DiaChi.Text = nvDangChon.DiaChi;

                    // Hiển thị giới tính
                    if (nvDangChon.GioiTinh == "Nam")
                        rdb_nam.IsChecked = true;
                    else if (nvDangChon.GioiTinh == "Nữ")
                        rdb_nu.IsChecked = true;
                    else
                        rdb_khac.IsChecked = true;

                    // Hiển thị ngày sinh
                    dp_NgaySinh.SelectedDate = nvDangChon.NgaySinh;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}