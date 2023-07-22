using System.ComponentModel.DataAnnotations;

namespace WorkshopApp.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string? Name { get; set; }

        public int Quantity { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
