using System.ComponentModel.DataAnnotations;

namespace ParameterTampering.Dto
{
    public class ArticleDto
    {
        [Key]
        public int Id { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter valid double number")]
        public decimal Price { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9 _\-().!?:;\\\/]*$", ErrorMessage = "You are using some unallowed sign or character")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
        public int Quantity { get; set; }
    }
}
