using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IShopCartRepository : IRepository<ShopCart>
    {
        void Update(ShopCart shopCart);
    }
}
