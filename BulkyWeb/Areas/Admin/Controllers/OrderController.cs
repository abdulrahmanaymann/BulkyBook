namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderViewModel orderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository
                .Get(u => u.Id == id, includeProperties: "ApplicationUser"),

                OrderDetails = _unitOfWork.OrderDetailRepository
                .GetAll(u => u.OrderHeaderId == id, includeProperties: "Product")
            };

            return View(orderVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orders = _unitOfWork.OrderHeaderRepository
                .GetAll(includeProperties: "ApplicationUser")
                .ToList();

            switch (status)
            {
                case "pending":
                    orders = orders.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orders = orders.Where(o => o.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orders = orders.Where(o => o.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orders = orders.Where(o => o.PaymentStatus == SD.PaymentStatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orders });
        }

        #endregion
    }
}
