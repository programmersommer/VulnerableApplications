using System;
using System.ComponentModel.DataAnnotations;

namespace ParameterTampering.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
