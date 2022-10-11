using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }
        public Product CartProduct { get; set; }

        public CartItemViewModel() { }

        public CartItemViewModel(int qty, Product Product)
        {
            Qty = qty;
            CartProduct = Product;
        }
    }
}
