using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Project_CS464
{
    public class ShoeData
    {
        [LoadColumn(0)]
        public string LoaiGiay { get; set; }

        [LoadColumn(1)]
        public float Tuan { get; set; }

        [LoadColumn(2)]
        public float SoLuongBan { get; set; }
    }


    public class ShoeSalesPrediction
    {
        [ColumnName("Score")]
        public float SoLuongDuKien { get; set; }
    }
}