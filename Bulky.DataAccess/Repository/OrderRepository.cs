using Bulky.DataAccess.Data;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>
    {
        private AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Order order)
        {
            _context.Update(order);
        }
    }
}
