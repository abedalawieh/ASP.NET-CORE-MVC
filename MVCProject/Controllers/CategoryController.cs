using MVCProject.DataAccess.Data;
using MVCProject.DataAccess.Repository.IRepository;
using MVCProject.Models;

using Microsoft.AspNetCore.Mvc;

namespace MVCProjectWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository db)
        {
            categoryRepository = db;
        }
        public IActionResult Index()
        {
            List<Category> objectCategoryList = categoryRepository.GetAll().ToList();
            return View(objectCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(obj.Name== obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display Order and Name cannot be same");
            }
            if(ModelState.IsValid) {
                categoryRepository.Add(obj);
                categoryRepository.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index", "Category");

            }
            return View();  

        }
        public IActionResult Edit(int?id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = categoryRepository.Get(u=>u.Id==id);
            if(categoryFromDb==null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            
            if (ModelState.IsValid)
            {
                categoryRepository.Update(obj);
                categoryRepository.Save();
                TempData["update"] = "Category Updated Successfully";

                return RedirectToAction("Index", "Category");

            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = categoryRepository.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost,ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Category categoryFromDb = categoryRepository.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            categoryRepository.Delete(categoryFromDb);


            categoryRepository.Save();
            TempData["delete"] = "Category Deleted Successfully";

            return RedirectToAction("Index", "Category");


        }
    }
}
