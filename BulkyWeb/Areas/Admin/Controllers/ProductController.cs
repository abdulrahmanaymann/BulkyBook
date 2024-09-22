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
            List<Product> products = _unitOfWork.ProductRepository.GetAll().ToList();
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
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                _unitOfWork.ProductRepository.Add(productVM.Product);
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

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.ProductRepository.Get(c => c.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            TempData["Success"] = "Product Updated successfully";
            return RedirectToAction("Index");
        }
    }
}
