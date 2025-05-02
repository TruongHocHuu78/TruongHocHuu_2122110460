namespace TruongHocHuu_2122110460.Dto
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public IFormFile File { get; set; }
    }

}
