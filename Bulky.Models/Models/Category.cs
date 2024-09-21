using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order for category must be between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
