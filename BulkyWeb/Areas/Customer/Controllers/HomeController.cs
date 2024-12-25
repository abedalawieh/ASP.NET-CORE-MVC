using AutoMapper;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is not null)
            {
                var quantityCartItems = _unitOfWork.ShopCartRepo
                    .Get(s => s.ApplicationUserId == userId, includeProperties: nameof(ShopCart.ShopCartItems))?.ShopCartItems.Count;
                if (quantityCartItems is not null) HttpContext.Session.SetInt32(CONST_Session.ShoppingCart, (int)quantityCartItems);
            }

            var productsVM = _unitOfWork.ProductRepo
                .GetAll(includeProperties: $"{nameof(Product.Category)}, {nameof(Product.Images)}")
                .Select(p => _mapper.Map<ProductVM>(p))
                .ToList();

            return View(productsVM);
        }

        public IActionResult Details(int productId)
        {
            if (productId == 0)
            {
                TempData["errorMessage"] = $"Invalid Id";
                return RedirectToAction(nameof(Index));
            }
            var product = _unitOfWork.ProductRepo.Get(p => p.Id == productId,
                includeProperties: $"{nameof(Product.Category)}, {nameof(Product.Images)}");
            if (product == null)
            {
                TempData["errorMessage"] = $"Resource not found.";
                return RedirectToAction(nameof(Index));
            }
            ShopCartItemVM shopCartItemVM = new()
            {
                ProductId = productId,
                Product = product,
                Quantity = 1
            };

            return View(shopCartItemVM);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShopCartItemVM shopCartItemVM)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (shopCartItemVM.Quantity <= 0 || shopCartItemVM.ProductId == 0) return ValidationProblem();
                    string? userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId == null)
                    {
                        TempData["errorMessage"] = $"User not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    var shopCart = _unitOfWork.ShopCartRepo.Get(c => c.ApplicationUserId == userId,
                        includeProperties: nameof(ShopCart.ShopCartItems));
                    if (shopCart is null)
                    {
                        shopCart = new ShopCart()
                        {
                            ApplicationUserId = userId,
                            AddedIn = DateTime.Now,
                        };
                        _unitOfWork.ShopCartRepo.Add(shopCart);
                        _unitOfWork.Save();
                    }

                    var shopItemDB = shopCart.ShopCartItems.Find(i => i.ProductId == shopCartItemVM.ProductId);
                    if (shopItemDB is null)
                    {
                        var newCartItem = _mapper.Map<ShopCartItem>(shopCartItemVM);

                        newCartItem.SetUnitPrice(_unitOfWork.ProductRepo.Get(i => i.Id == newCartItem.ProductId)
                            ?? throw new Exception("Product not found."));
                        newCartItem.CalculateTotalPrice();
                        newCartItem.ShopCartId = shopCart.Id;
                        newCartItem.AddedIn = DateTime.Now;

                        shopCart.ShopCartItems.Add(newCartItem);
                        shopCart.CalculateTotalValue();
                        shopCart.UpdatedOn = DateTime.Now;

                        _unitOfWork.ShopCartRepo.Update(shopCart);
                        _unitOfWork.ShopCartItemRepo.Add(newCartItem);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        shopItemDB.Quantity += shopCartItemVM.Quantity;
                        shopItemDB.SetUnitPrice(_unitOfWork.ProductRepo.Get(p => p.Id == shopItemDB.ProductId)
                            ?? throw new Exception("Invalid Product"));
                        shopItemDB.CalculateTotalPrice();
                        shopItemDB.UpdatedOn = DateTime.Now;

                        _unitOfWork.ShopCartItemRepo.Update(shopItemDB);
                        _unitOfWork.Save();

                        shopCart.CalculateTotalValue();
                        shopCart.UpdatedOn = DateTime.Now;

                        _unitOfWork.ShopCartRepo.Update(shopCart);
                        _unitOfWork.Save();
                    }

                    transaction.Commit();
                    TempData["successMessage"] = $"Cart updated sucessfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(0, ex, "Erro no processo de Adicionar item ao carrinho.");
                    TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        //[HttpPost]
        //[Authorize]
        //public IActionResult Details(ShopCartItemVM shopCartItem)
        //{
        //    try
        //    {
        //        if (shopCartItem.Quantity <= 0 || shopCartItem.ProductId == 0) return ValidationProblem();

        //        string? userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        if (userId == null) return NotFound();

        //        shopCartItem.ApplicationUserId = userId;
        //        ShopCart? shopCartFromDb = _unitOfWork.ShopCartRepo.Get(sc => sc.ApplicationUserId == userId, includeProperties: nameof(ShopCart.ShopCartItems));
        //        if (shopCartFromDb is null)
        //        {
        //            ShopCart newShopCart = new()
        //            {
        //                ApplicationUserId = userId,
        //                ShopCartItems = new List<ShopCartItem>() { shopCartItem }
        //            };
        //            _unitOfWork.ShopCartRepo.Add(newShopCart);
        //            _unitOfWork.Save();

        //            var QuantityOfItemsInCart = _unitOfWork.ShopCartItemRepo.GetAll(s => s.ApplicationUserId == userId)?.Count();
        //            if (QuantityOfItemsInCart is not null)
        //                HttpContext.Session.SetInt32(CONST_Session.ShoppingCart, (int)QuantityOfItemsInCart);
        //        }
        //        else
        //        {
        //            shopCartFromDb.ShopCartItems.Add(shopCartItem);
        //            _unitOfWork.ShopCartRepo.Update(shopCartFromDb);
        //            _unitOfWork.Save();
        //        }

        //        TempData["successMessage"] = $"Cart updated sucessfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(0, ex, "Erro no processo de UPSERT do item carrinho de compras.");
        //        TempData["errorMessage"] = $"Something went wrong but don't be sad, it wasn't you fault.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}