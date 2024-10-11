using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
