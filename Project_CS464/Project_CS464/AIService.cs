using Microsoft.ML;
using System;
using System.IO;
using System.Linq;

namespace Project_CS464
{
    public class AiService
    {
        private static string _modelPath = Path.Combine(Environment.CurrentDirectory, "ShoeSalesModel.zip");
        private static string _dataPath = Path.Combine(Environment.CurrentDirectory, "sales_data.csv");
        private MLContext _mlContext;

        public AiService()
        {
            _mlContext = new MLContext();
        }


        public void TrainModel()
        {
            if (!File.Exists(_dataPath)) return; 


            IDataView dataView = _mlContext.Data.LoadFromTextFile<ShoeData>(
                _dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "LoaiGiayEncoded", inputColumnName: "nameof(ShoeSalesData.LoaiGiay)")
                .Append(_mlContext.Transforms.Concatenate("Features", "LoaiGiayEncoded", "Tuan"))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "SoLuongBan", maximumNumberOfIterations: 100));

            // Bắt đầu huấn luyện
            var model = pipeline.Fit(dataView);

            // Lưu mô hình vào file .zip
            _mlContext.Model.Save(model, dataView.Schema, _modelPath);
        }

        //Dùng mô hình đã lưu để dự báo
        public float PredictSales(string loaiGiay, int tuanToi)
        {
            if (!File.Exists(_modelPath)) return 0;

            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ShoeData, ShoeSalesPrediction>(loadedModel);

            // Tạo dữ liệu đầu vào
            var input = new ShoeData
            {
                LoaiGiay = loaiGiay,
                Tuan = tuanToi
            };

            // Dự đoán
            var prediction = predictionEngine.Predict(input);
            return prediction.SoLuongDuKien;
        }
    }
}