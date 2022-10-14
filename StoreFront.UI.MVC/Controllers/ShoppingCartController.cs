using Microsoft.AspNetCore.Mvc;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NuGet.Versioning;

namespace StoreFront.UI.MVC.Controllers
{
    #region Steps to Implement Session Based Shopping Cart
    /*
     * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
     * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
     * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
     * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
     *      - add using statements
     *          - using GadgetStore.DATA.EF.Models;
     *          - using Microsoft.AspNetCore.Identity;
     *          - using GadgetStore.UI.MVC.Models;
     *          - using Newtonsoft.Json;
     *      - Add props for the GadgetStoreContext && UserManager
     *      - Create a constructor for the controller - assign values to context && usermanager
     *      - Code the AddToCart() action
     *      - Code the Index() action
     *      - Code the Index View
     *          - Start with the basic table structure
     *          - Show the items that are easily accessible (like the properties from the model)
     *          - Calculate/show the lineTotal
     *          - Add the RemoveFromCart <a>
     *      - Code the RemoveFromCart() action
     *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
     *      - Add UpdateCart <form> to the Index View
     *      - Code the UpdateCart() action
     *      - Add Submit Order button to Index View
     *      - Code SubmitOrder() action
     * */

    #endregion


    public class ShoppingCartController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //TODO: Create View********************************

        public IActionResult Index()
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null)
            {
                ViewBag.Message = "There are no items in your cart";

                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                ViewBag.Message = null;

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int Id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            var sessionCart = HttpContext.Session.GetString("cart");

            if (sessionCart == null)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            Product product = _context.Products.Find(Id);

            CartItemViewModel civm = new CartItemViewModel(1, product);

            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int Id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(Id);

            if (shoppingCart.Count() == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[productId].Qty = qty;

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        #region Planning Out Order Submission
        // Create Order object -> then save to DB
        // - CustomerId (get from Identity)
        // - OrderDate (current date/time aka Datetime.Now)
        // - ShipDate (current date/time)
        // - ItemsSold (cart)
        // - OrderAmountTotal (cart)
        // Add record to _context
        // Save DB Changes

        // Create OrderProduct objects for each item in the cart -> then save to DB
        // - ProductId (Cart)
        // - OrderId (OrderObject created)       
        // - ProductPrice (Cart)
        // Add record to _context
        // Save DB changes 
        #endregion

        public async Task<IActionResult> SubmitOrder()
        {
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

                        

            Customer customer = _context.Customers.Where(c => c.UserId == userId).FirstOrDefault();

            var sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            Order o = new Order()
            {
                CustomerId = customer.CustomerId,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(3),
                ItemsSold = shoppingCart.Values.Select(x => x.Qty).Sum(),
                OrderAmoutTotal = decimal.Parse(HttpContext.Session.GetString("total"))            
            };

            _context.Orders.Add(o);

            foreach (var item in shoppingCart)
            {
                ProductOrder po = new ProductOrder()
                {
                    ProductId = item.Value.CartProduct.ProductId,
                    OrderId = o.OrderId,
                    ProductPrice = item.Value.CartProduct.ProductPrice
                };
                o.ProductOrders.Add(po);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("Index", "Orders");
        }
    }
}
