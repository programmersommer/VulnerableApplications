using System;
using System.ComponentModel.DataAnnotations;

namespace ParameterTampering.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
