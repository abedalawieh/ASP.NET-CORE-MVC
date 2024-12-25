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
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var categoriesVM = _unitOfWork.CategoryRepo.GetAll().Select(c => _mapper.Map<CategoryVM>(c));
            return View(categoriesVM);
        }

        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new();
            if (id.GetValueOrDefault() == 0)
                return View(categoryVM);

            categoryVM = _mapper.Map<CategoryVM>(_unitOfWork.CategoryRepo.Get(c => c.Id == id));
            if (categoryVM == null)
            {
                TempData["errorMessage"] = $"Resource not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoryVM);
        }

        [HttpPost]
        public IActionResult Upsert(CategoryVM categoryVM)
        {
            try
            {
                if (!ModelState.IsValid) return View();

                string actionMessage;
                var category = _mapper.Map<Category>(categoryVM);
                if (category.Id == 0)
                {
                    _unitOfWork.CategoryRepo.Add(category);
                    actionMessage = "created";
                }
                else
                {
                    _unitOfWork.CategoryRepo.Update(category);
                    actionMessage = "updated";
                }
                _unitOfWork.Save();

                TempData["successMessage"] = $"Category {category.Name} {actionMessage} successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
                _logger.LogError(0, ex, "Erro no processo de UPSERT do Produto.");
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                TempData["errorMessage"] = $"Resource not found.";
                return RedirectToAction(nameof(Index));
            }
            var category = _unitOfWork.CategoryRepo.Get(c => c.Id == id);
            if (category == null)
            {
                TempData["errorMessage"] = $"Resource not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<CategoryVM>(category));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            try
            {
                if (id == 0)
                {
                    TempData["errorMessage"] = $"Resource not found.";
                    return RedirectToAction("Index");
                }
                var category = _unitOfWork.CategoryRepo.Get(c => c.Id == id);
                if (category == null)
                {
                    TempData["errorMessage"] = $"Resource not found.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.CategoryRepo.Delete(category);
                _unitOfWork.Save();

                TempData["successMessage"] = $"Category {category.Name} deleted successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
                _logger.LogError(0, ex, "Erro no processo de Edit em Category.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
