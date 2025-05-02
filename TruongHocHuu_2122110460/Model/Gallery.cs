namespace TruongHocHuu_2122110460.Model
{
    public class Gallery
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsThumbnail { get; set; }

        public Product Product { get; set; } = null!;
    }

}
