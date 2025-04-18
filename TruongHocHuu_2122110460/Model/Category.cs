namespace TruongHocHuu_2122110460.Model
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property (danh sách sản phẩm thuộc Category này)
        public List<Product> Products { get; set; }
    }
}
