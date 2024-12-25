using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ShopCartItemRepository : Repository<ShopCartItem>, IShopCartItemRepository
    {
        private AppDbContext _context;
        
        public ShopCartItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShopCartItem shopCartItem)
        {
            _context.ShopCartItems.Update(shopCartItem);
        }
    }
}
