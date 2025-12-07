using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls; // Thư viện chứa Page
using Project_CS464;

namespace Project_CS464
{
    public partial class AIPage : Page
    {
        private AiService _aiService;

        // Giả lập danh sách giày trong kho
        private List<string> _danhSachGiay = new List<string> { "Nike Air", "Adidas Ultra", "Puma Running", "Biti's Hunter", "Converse" };

        public AIPage()
        {
            InitializeComponent();
            _aiService = new AiService();
        }

        private void btnTrain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Gọi hàm train từ Service đã viết ở bước trước
                _aiService.TrainModel();
                MessageBox.Show("Huấn luyện hoàn tất! AI đã sẵn sàng dự báo.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi huấn luyện: {ex.Message}");
            }
        }

        private void btnRecommend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tuanToi = GetNextWeekNumber();
                var ketQua = new Dictionary<string, float>();

                foreach (var giay in _danhSachGiay)
                {
                    float duBao = _aiService.PredictSales(giay, tuanToi);
                    ketQua.Add(giay, duBao);
                }

                // Sắp xếp và hiển thị
                var deXuat = ketQua.OrderByDescending(x => x.Value).ToList();

                lstSuggestions.Items.Clear();
                lstSuggestions.Items.Add($"=== DỰ BÁO TUẦN THỨ {tuanToi} ===");

                foreach (var item in deXuat)
                {
                    string danhGia = item.Value > 8 ? "🔥 HOT - Nhập nhiều" : (item.Value > 4 ? "✅ Ổn - Nhập vừa" : "⚠️ Chậm - Cân nhắc");
                    lstSuggestions.Items.Add($"{item.Key.PadRight(15)} | Dự kiến bán: {item.Value:F1} | {danhGia}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi dự báo (Hãy huấn luyện trước): {ex.Message}");
            }
        }

        private int GetNextWeekNumber()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(DateTime.Now, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek) + 1;
        }
    }
}