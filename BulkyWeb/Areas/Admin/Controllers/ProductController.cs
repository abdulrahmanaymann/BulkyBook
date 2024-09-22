namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository
                .GetAll(includeProperties: "Category")
                .ToList();
            return View(products);
        }

        public IActionResult UpSert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll()
            .Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ProductViewModel productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // Create
                return View(productVM);
            }

            // Update
            productVM.Product = _unitOfWork.ProductRepository.Get(p => p.Id == id);
            return View(productVM);

        }

        [HttpPost]
        public IActionResult UpSert(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["Success"] = "Product added successfully";

                return RedirectToAction("Index");
            }

            productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll()
            .Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Product> products = _unitOfWork.ProductRepository
                .GetAll(includeProperties: "Category")
                .ToList();

            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.ProductRepository.Get(p => p.Id == id);

            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath =
                           Path.Combine(_hostEnvironment.WebRootPath,
                           product.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
