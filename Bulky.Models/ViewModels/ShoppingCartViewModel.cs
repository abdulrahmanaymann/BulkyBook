using BulkyBook.Models.Models;

namespace BulkyBook.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
