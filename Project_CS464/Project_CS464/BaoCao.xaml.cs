using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Project_CS464.Model;
using Project_CS464;

namespace Project_CS464
{
    public partial class QLyNv : Page
    {
        // Khởi tạo kết nối Database
        // Lưu ý: Đảm bảo UserDBEntities đã được Update Model để chứa 3 bảng: SanPham, DonHang, CTDonHang
        private QLBHEntities db = new QLBHEntities();

        public QLyNv()
        {
            InitializeComponent();
            // Mặc định chọn ngày hiện tại
            datePicker.SelectedDate = DateTime.Now;
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datePicker.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày cần xem báo cáo!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime selectedDate = datePicker.SelectedDate.Value;
                int mode = cbMode.SelectedIndex; // 0: Ngày, 1: Tháng, 2: Năm

                LoadReportData(selectedDate, mode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính toán: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadReportData(DateTime date, int mode)
        {
            var querySanPhams = db.SanPhams.AsQueryable();
            var queryDonHangs = db.DonHangs.AsQueryable();

            // Áp dụng bộ lọc ngày/tháng/năm
            switch (mode)
            {
                case 0: // Theo ngày
                    querySanPhams = querySanPhams.Where(x => x.NgayNhap.HasValue && x.NgayNhap.Value.Year == date.Year
                                                          && x.NgayNhap.Value.Month == date.Month
                                                          && x.NgayNhap.Value.Day == date.Day);

                    queryDonHangs = queryDonHangs.Where(x => x.ngaydat.HasValue && x.ngaydat.Value.Year == date.Year
                                                          && x.ngaydat.Value.Month == date.Month
                                                          && x.ngaydat.Value.Day == date.Day);
                    break;

                case 1: // Theo tháng
                    querySanPhams = querySanPhams.Where(x => x.NgayNhap.HasValue && x.NgayNhap.Value.Year == date.Year
                                                          && x.NgayNhap.Value.Month == date.Month);

                    queryDonHangs = queryDonHangs.Where(x => x.ngaydat.HasValue && x.ngaydat.Value.Year == date.Year
                                                          && x.ngaydat.Value.Month == date.Month);
                    break;

                case 2: // Theo năm
                    querySanPhams = querySanPhams.Where(x => x.NgayNhap.HasValue && x.NgayNhap.Value.Year == date.Year);

                    queryDonHangs = queryDonHangs.Where(x => x.ngaydat.HasValue && x.ngaydat.Value.Year == date.Year);
                    break;
            }

            // --- BƯỚC 2: TÍNH TOÁN CÁC CHỈ SỐ ---

            // 1. Tổng nhập: Đếm số bản ghi trong bảng SanPham theo ngày nhập
            int tongNhap = querySanPhams.Count();
            txtTongNhap.Text = tongNhap.ToString("N0"); // Format số (ví dụ: 1,200)

            // 2. Số đơn: Đếm số bản ghi trong bảng DonHang
            int soDon = queryDonHangs.Count();
            txtSoDon.Text = soDon.ToString("N0");

            // --- XỬ LÝ DỮ LIỆU BÁN HÀNG (JOIN BẢNG) ---
            // Để tính Tổng Bán, Lợi Nhuận, Top SP -> Phải nối DonHang (đã lọc ngày) với CTDonHang và SanPham

            var listBanHang = from dh in queryDonHangs
                              join ctdh in db.CTDonHangs on dh.madon equals ctdh.madon
                              join sp in db.SanPhams on ctdh.masp equals sp.MaSP
                              select new
                              {
                                  MaSP = ctdh.masp,
                                  TenSP = sp.MauGiay, // Hoặc sp.Hang, tùy bạn muốn hiển thị gì
                                  SoLuong = ctdh.soluong,
                                  Gia = sp.Gia
                              };

            // Lấy dữ liệu về RAM để tính toán (tránh lỗi Entity Framework khi tính toán phức tạp)
            var dataSales = listBanHang.ToList();

            if (dataSales.Count > 0)
            {
                // 3. Tổng bán: Tổng số lượng sản phẩm bán ra
                // (Vì MaSP là string nên không nhân được, ở đây tôi tính tổng số lượng)
                int totalSoldQty = dataSales.Sum(x => x.SoLuong.GetValueOrDefault(0));
                txtTongBan.Text = totalSoldQty.ToString("N0");

                // 4. Lợi nhuận (Doanh thu): Tổng (Số lượng * Giá)
                decimal totalRevenue = dataSales.Sum(x => (decimal)x.SoLuong.GetValueOrDefault(0) * x.Gia.GetValueOrDefault(0));
                txtLoiNhuan.Text = totalRevenue.ToString("N0") + " đ";

                // 5. Top sản phẩm: Sản phẩm có tổng số lượng bán nhiều nhất
                var topProduct = dataSales.GroupBy(x => x.MaSP)
                                          .Select(g => new
                                          {
                                              MaSP = g.Key,
                                              TongSL = g.Sum(x => x.SoLuong.GetValueOrDefault(0)),
                                              TenSP = g.FirstOrDefault().TenSP
                                          })
                                          .OrderByDescending(x => x.TongSL)
                                          .FirstOrDefault();

                if (topProduct != null)
                {
                    txtTopSP.Text = $"{topProduct.MaSP} - {topProduct.TenSP} \n(SL: {topProduct.TongSL})";
                }
                else
                {
                    txtTopSP.Text = "Không xác định";
                }
            }
            else
            {
                // Nếu không có đơn hàng nào trong khoảng thời gian này
                txtTongBan.Text = "0";
                txtLoiNhuan.Text = "0 đ";
                txtTopSP.Text = "---";
            }
        }
    }
}