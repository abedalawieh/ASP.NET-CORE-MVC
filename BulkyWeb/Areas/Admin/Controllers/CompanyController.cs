using AutoMapper;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CONST_Roles.Admin)]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyController(IUnitOfWork unitOfWork, ILogger<CompanyController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        #region ACTIONS

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upsert(int id)
        {
            CompanyVM companyVM = new();
            if (id == 0)
                return View(companyVM);
            companyVM = _mapper.Map<CompanyVM>(_unitOfWork.CompanyRepo.Get(c => c.Id == id, includeProperties: nameof(Company.Address)));
            if (companyVM == null)
            {
                TempData["errorMessage"] = $"Resource not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(companyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(CompanyVM companyVM)
        {
            try
            {
                if (!ModelState.IsValid) return View(companyVM);

                string? messageAction;
                var company = _mapper.Map<Company>(companyVM); 
                if (company.Id == 0)
                {
                    messageAction = "create";
                    _unitOfWork.CompanyRepo.Add(company);
                }
                else
                {
                    messageAction = "update";
                    _unitOfWork.CompanyRepo.update(company);
                }
                _unitOfWork.Save();

                TempData["successMessage"] = $"Company {company.Name} {messageAction} successfully";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
                _logger.LogError(0, ex, "Erro no processo de UPSERT de Company");
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region API CALLS

        [HttpGet]
        [Route("API/[area]/[controller]/GetAll")]
        public ActionResult GetAllApi()
        {
            var companies = _unitOfWork.CompanyRepo.GetAll(includeProperties: nameof(Company.Address));
            return Json(new { success = true, data = companies });
        }

        [HttpDelete]
        [Route("API/[area]/[controller]/Delete")]
        public IActionResult DeleteApi(int id)
        {
            if (id == 0) return Json(new { success = false, message = "Invalid Id" });
            var company = _unitOfWork.CompanyRepo.Get(c => c.Id == id);
            if (company == null) return Json(new { success = false, message = "Error while deleting" });

            _unitOfWork.CompanyRepo.Delete(company);
            _unitOfWork.Save();

            return Json(new { success = true, message = $"Company {company.Name} was successfully deleted" });
        }

        #endregion
    }
}
