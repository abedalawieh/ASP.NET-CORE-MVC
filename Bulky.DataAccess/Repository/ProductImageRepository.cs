using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ProductImage productImage)
        {
            _context.Update(productImage);
        }
    }
}
