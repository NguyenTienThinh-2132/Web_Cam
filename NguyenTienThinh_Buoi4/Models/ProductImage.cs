namespace CamZone.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; }

        //Thông tin khóa ngoại
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
