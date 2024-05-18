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
        private readonly IWebHostEnvironment _webEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webEnvironment = webEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objectProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
         

            return View(objectProductList);
        }
        public IActionResult Upsert(int? id)
        {


            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {

                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);

            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj,IFormFile?file)
        {
            if (obj == null)
            {
                return BadRequest("Invalid data submitted.");
            }
            if (ModelState.IsValid)
            {
                string wwwwRootPath = _webEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl)) {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwwRootPath,obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    };  
                    obj.Product.ImageUrl = @"\images\product\" + fileName; 

                }
                if (obj.Product.Id == 0)
                {
                _unitOfWork.Product.Add(obj.Product);

                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);

                }
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index", "Product");

            }
            return View();

        }
      


        #region Api calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objectProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objectProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
             var productToBeDeleted= _unitOfWork.Product.Get(u=>u.Id==id);
            if(productToBeDeleted == null)
            {
                return Json(new { success=false, message="Error While Deleting" });

            }
            var oldImagePath = Path.Combine(_webEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Delete(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion

    }
}
