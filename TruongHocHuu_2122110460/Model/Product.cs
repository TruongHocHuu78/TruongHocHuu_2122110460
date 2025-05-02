namespace TruongHocHuu_2122110460.Model
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        private DateTime createdAt { get; set; }
        private DateTime updatedAt { get; set; }
        // Foreign key
        public long CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }
        public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();

    }
}
