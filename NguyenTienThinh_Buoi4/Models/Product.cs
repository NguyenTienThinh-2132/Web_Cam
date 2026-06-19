using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamZone.Models
{
    public class Product
    {
        //ID sản phẩm, khóa chính
        public int Id { get; set; }
        //Tên sản phẩm: bắt buộc phải có <==> not null
        //Chiều dài tối đa là 100 kí tự.
        //Kiểu dữ liệu là string <==> nvarchar
        [Required, StringLength(100)]
        public string Name { get; set; }
        //Thuộc tính giá, nằm trong ngưỡng 0,01 => 10000
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        //Thuộc tính tính này cho phép NULL
        public string? ImageUrl { get; set; }
        //Thông tin để biết khóa ngoại
        //ProductImage sẽ có khóa ngoại đi qua đây
        public List<ProductImage>? Images { get; set; }

        [NotMapped]
        public List<string> ImageUrls => Images?
            .Where(i => !string.IsNullOrWhiteSpace(i.Url))
            .Select(i => i.Url)
            .ToList() ?? new List<string>();

        //Thông tin khoa ngoại
        //CategoryID là khóa ngoại của bảng Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
