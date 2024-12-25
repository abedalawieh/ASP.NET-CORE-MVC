using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                HttpContext.Session.Clear();
                return Task.FromResult<IViewComponentResult>(View(0));
            }
            else
            {
                var QuatityOfItensInCart = _unitOfWork.ShopCartRepo.Get(c => c.ApplicationUserId == userId, 
                    includeProperties: nameof(ShopCart.ShopCartItems))?.ShopCartItems.Count;
                if (QuatityOfItensInCart == null)
                    HttpContext.Session.SetInt32(CONST_Session.ShoppingCart, 0);
                else
                    HttpContext.Session.SetInt32(CONST_Session.ShoppingCart, (int)QuatityOfItensInCart);
          
                return Task.FromResult<IViewComponentResult>(View(HttpContext.Session.GetInt32(CONST_Session.ShoppingCart)));
            }
        }
    }
}
