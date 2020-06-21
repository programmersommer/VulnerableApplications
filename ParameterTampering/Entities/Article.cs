﻿
using System.ComponentModel.DataAnnotations;

namespace ParameterTampering.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
