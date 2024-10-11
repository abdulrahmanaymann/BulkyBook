using Stripe.Checkout;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ShoppingCartViewModel CartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImageRepository.GetAll();

            foreach (var cart in CartViewModel.ShoppingCartList)
            {
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(CartViewModel);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            // Set the ApplicationUser for the OrderHeader
            CartViewModel.OrderHeader.ApplicationUser =
                _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            CartViewModel.OrderHeader.Name = CartViewModel.OrderHeader.ApplicationUser.Name;
            CartViewModel.OrderHeader.PhoneNumber = CartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            CartViewModel.OrderHeader.StreetAddress = CartViewModel.OrderHeader.ApplicationUser.StreetAddress;
            CartViewModel.OrderHeader.City = CartViewModel.OrderHeader.ApplicationUser.City;
            CartViewModel.OrderHeader.State = CartViewModel.OrderHeader.ApplicationUser.State;
            CartViewModel.OrderHeader.PostalCode = CartViewModel.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in CartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(CartViewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel.ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            CartViewModel.OrderHeader.OrderDate = DateTime.Now;
            CartViewModel.OrderHeader.ApplicationUserId = userId;

            // Set the ApplicationUser for the OrderHeader
            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id == userId);

            foreach (var cart in CartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                CartViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                // It is Customer Account
                CartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                CartViewModel.OrderHeader.OrderStatus = SD.StatusPending;
            }

            else
            {
                // It is Company Account
                CartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                CartViewModel.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitOfWork.OrderHeaderRepository.Add(CartViewModel.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in CartViewModel.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = CartViewModel.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };

                _unitOfWork.OrderDetailRepository.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                // It is Customer Account
                // Add Stripe Logic Here
                var domain = "https://localhost:7193/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={CartViewModel.OrderHeader.Id}",
                    CancelUrl = domain + "Customer/Cart/Index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in CartViewModel.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title,
                            }
                        },
                        Quantity = item.Count
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeaderRepository.UpdateStripePaymentId(CartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);

                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return StatusCode(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = CartViewModel.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader order = _unitOfWork.OrderHeaderRepository
                .Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if (order.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(order.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaderRepository
                        .UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);

                    _unitOfWork.OrderHeaderRepository
                        .UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);

                    _unitOfWork.Save();
                }

                HttpContext.Session.Clear();
            }

            _emailSender.SendEmailAsync(order.ApplicationUser.Email, "New Order - Bulky Book",
                $"<p>Order has been submitted successfully - {order.Id}</p>");

            List<ShoppingCart> carts = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCartRepository.RemoveRange(carts);
            _unitOfWork.Save();

            return View(id);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.Id == cartId, tracked: true);

            if (cartFromDb.Count <= 1)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
               _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            }

            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.Id == cartId, tracked: true);

            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // Helper method to get the price based on the quantity
        private double GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            if (cart.Count <= 50)
            {
                return cart.Product.Price;
            }

            else if (cart.Count > 50 && cart.Count <= 100)
            {
                return cart.Product.Price50;
            }

            return cart.Product.Price100;
        }
    }
}
