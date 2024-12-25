using AutoMapper;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = CONST_Roles.Admin)]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _logger = logger;
        }


        #region ACTIONS

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var productVM = new ProductVM();

            if (id.HasValue)
                productVM = _mapper.Map<ProductVM>(
                    _unitOfWork.ProductRepo.Get(p => p.Id == id, includeProperties: $"{nameof(Product.Images)}"));
            productVM.CategoryList = PopulateCategoryList();

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile>? files)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    productVM.CategoryList = PopulateCategoryList();
                    return View(productVM);
                }

                string actionMessage;
                var product = _mapper.Map<Product>(productVM);
                if (product.Id == 0)
                {
                    _unitOfWork.ProductRepo.Add(product);
                    actionMessage = "created";
                }
                else
                {
                    _unitOfWork.ProductRepo.Update(product);
                    actionMessage = "updated";
                }
                _unitOfWork.Save();

                if (files != null)
                {
                    product.Images = SaveImages(product.Id, files);
                    _unitOfWork.ProductRepo.Update(product);
                    _unitOfWork.Save();
                }

                TempData["successMessage"] = $"Product {product.Title} {actionMessage} successfuly";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
                _logger.LogError(0, ex, "Erro no processo de UPSERT do Produto.");
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult DeleteOneImage(int imageId)
        {
            var image = _unitOfWork.ProductImageRepo.Get(i => i.Id == imageId);
            if (image == null || image.ImageUrl.IsNullOrEmpty())
            {
                TempData["errorMessage"] = "Resource not found";
                return RedirectToAction(nameof(Index));
            }

            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var oldImagePath = Path.Combine(wwwRootPath, image.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            _unitOfWork.ProductImageRepo.Delete(image);
            _unitOfWork.Save();

            TempData["successMessage"] = $"Image removed successfuly";
            return RedirectToAction(nameof(Upsert), new { id = image.ProductId });
        }

        #endregion


        #region API's

        [HttpGet]
        [Route("[area]/API/[controller]/GetAll")]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.ProductRepo
                .GetAll(includeProperties: "Category");
            return Json(new { success = true, data = products });
        }

        [Route("[area]/API/[controller]/Delete")]
        public IActionResult DeleteApi(int? id)
        {
            var product = _unitOfWork.ProductRepo.Get(p => p.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Error while deleting" });
            if (product.Images.Any())
                DeleteImages(product.Id);

            _unitOfWork.ProductRepo.Delete(product);
            _unitOfWork.Save();

            return Json(new { success = true, message = $"Product {product.Title} deleted successfully." });
        }

        #endregion


        #region PRIVATE FUNCTIONS 

        private IEnumerable<SelectListItem> PopulateCategoryList()
        {
            return _unitOfWork.CategoryRepo
                .GetAll()
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
        }

        private List<ProductImage> SaveImages(int productId, List<IFormFile> files)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;

            List<ProductImage> newProductImages = new(); 
            foreach (var file in files)
            {
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = @$"images\products\product-{productId}";
                string finalPath = Path.Combine(wwwRootPath, productPath);

                if (!Directory.Exists(finalPath)) Directory.CreateDirectory(finalPath);
                using (var fileStream = new FileStream(Path.Combine(finalPath, newFileName), FileMode.CreateNew))
                {
                    file.CopyTo(fileStream);
                }

                newProductImages.Add(new ProductImage()
                {
                    ImageUrl = @$"\{productPath}\{newFileName}",
                    ProductId = productId
                });
            }

            return newProductImages;
        }

        private void DeleteImages(int productId)
        {
            string productPath = @$"images\products\product-{productId}";
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);
            if (!Directory.Exists(finalPath)) return;

            string[] filePaths = Directory.GetFiles(finalPath);
            foreach (string filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            }
            Directory.Delete(finalPath);
        }

        #endregion
    }
}
