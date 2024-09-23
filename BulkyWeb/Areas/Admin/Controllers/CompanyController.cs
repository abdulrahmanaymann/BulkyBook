namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.CompanyRepository.GetAll().ToList();

            return View(companies);
        }

        public IActionResult UpSert(int? id)
        {
            if (id == null || id == 0)
            {
                // Create
                return View(new Company());
            }

            // Update
            Company company = _unitOfWork.CompanyRepository.Get(p => p.Id == id);
            return View(company);

        }

        [HttpPost]
        public IActionResult UpSert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.CompanyRepository.Add(company);
                }
                else
                {
                    _unitOfWork.CompanyRepository.Update(company);
                }

                _unitOfWork.Save();
                TempData["Success"] = "Company added successfully";

                return RedirectToAction("Index");
            }

            return View(company);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Company> companies = _unitOfWork.CompanyRepository.GetAll().ToList();

            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.CompanyRepository.Get(p => p.Id == id);

            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.CompanyRepository.Remove(company);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
