using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IShopCartItemRepository : IRepository<ShopCartItem>
    {
        void Update(ShopCartItem shoppingCartItem);
    }
}
