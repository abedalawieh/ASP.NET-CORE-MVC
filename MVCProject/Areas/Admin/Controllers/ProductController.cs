using MVCProject.DataAccess.Data;
using MVCProject.DataAccess.Repository.IRepository;
using MVCProject.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCProject.Models.ViewModels;

namespace MVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objectProductList = _unitOfWork.Product.GetAll().ToList();
         

            return View(objectProductList);
        }
        public IActionResult Create()
        {

         
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index", "Product");

            }
            return View();

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["update"] = "Product Updated Successfully";

                return RedirectToAction("Index", "Product");

            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Delete(productFromDb);


            _unitOfWork.Save();
            TempData["delete"] = "Product Deleted Successfully";

            return RedirectToAction("Index", "Product");


        }
    }
}
