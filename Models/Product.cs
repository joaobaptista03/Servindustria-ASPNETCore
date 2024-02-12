namespace Servindustria.Models {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
    }
}