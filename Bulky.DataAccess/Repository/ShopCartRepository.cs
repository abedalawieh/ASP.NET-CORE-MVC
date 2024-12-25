using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ShopCartRepository : Repository<ShopCart>, IShopCartRepository
    {
        private AppDbContext _context;
        
        public ShopCartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ShopCart shopCart)
        {
            _context.ShopCarts.Update(shopCart);
        }
    }
}
