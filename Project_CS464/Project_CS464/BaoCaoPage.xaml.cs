using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Project_CS464;

namespace Project_CS464
{
    public partial class BaoCaoPage : Page
    {
        QLBHEntities db = new QLBHEntities();

        public BaoCaoPage()
        {
            InitializeComponent();
            
            dpTuNgay.SelectedDate = DateTime.Today.AddDays(-30);
            dpDenNgay.SelectedDate = DateTime.Today;
        }

        private void btnXemBaoCao_Click(object sender, RoutedEventArgs e)
        {
            db = new QLBHEntities();
            LoadBaoCao();
        }

        private void LoadBaoCao()
        {
                DateTime? tuNgay = dpTuNgay.SelectedDate;
                DateTime? denNgay = dpDenNgay.SelectedDate;

                if (tuNgay == null || denNgay == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ ngày tháng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                DateTime denNgayCuoi = denNgay.Value.AddDays(1).AddSeconds(-1);

                var query = from dh in db.DonHangs
                            join ct in db.CTDonHangs on dh.madon equals ct.madon
                            join sp in db.SanPhams on ct.masp equals sp.MaSP
                            where dh.ngaydat >= tuNgay && dh.ngaydat <= denNgayCuoi
                            select new
                            {
                                MaDH = dh.madon,
                                NgayDat = dh.ngaydat,

                                // Tên = Hãng + Mẫu)
                                TenSP = (sp.Hang == null ? "" : sp.Hang) + " " + (sp.MauGiay == null ? "" : sp.MauGiay),
                                SoLuong = ct.soluong == null ? 0 : ct.soluong,
                                DonGia = sp.Gia == null ? 0 : sp.Gia,
                                ThanhTien = (ct.soluong == null ? 0 : ct.soluong) * (sp.Gia == null ? 0 : sp.Gia)
                            };

                var ketQua = query.ToList();
                
                dgBaoCao.ItemsSource = ketQua;
                
                if (ketQua.Count > 0)
                {
                    decimal tongTien = 0;
                    foreach (var item in ketQua)
                    {
                        tongTien += Convert.ToDecimal(item.ThanhTien);
                    }

                    txtTongDoanhThu.Text = string.Format("{0:N0} đ", tongTien);
                }
                else
                {
                    txtTongDoanhThu.Text = "0 đ";
                    MessageBox.Show("Không tìm thấy dữ liệu đơn hàng nào trong khoảng thời gian này!", "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
                }
        }            
    }
}