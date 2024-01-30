namespace Servindustria.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PicUrl { get; set; } = string.Empty;
        public string PdfUrl { get; set; } = string.Empty;
    }
}
