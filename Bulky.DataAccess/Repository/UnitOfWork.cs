using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository CategoryRepo { get; private set; }
        public IProductRepository ProductRepo { get; private set; }
        public ICompanyRepository CompanyRepo { get; private set; }
        public IShopCartRepository ShopCartRepo { get; private set; }
        public IApplicationUserRepository ApplicationUserRepo { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepo { get; private set; }
        public IOrderDetailRepository OrderDetailRepo { get; private set; }
        public IProductImageRepository ProductImageRepo {  get; private set; }
        public IShopCartItemRepository ShopCartItemRepo {  get; private set; }
        public OrderRepository OrderRepo {  get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryRepo = new CategoryRepository(_context);
            ProductRepo = new ProductRepository(_context);
            CompanyRepo = new CompanyRepository(_context);
            ShopCartRepo = new ShopCartRepository(_context);
            ApplicationUserRepo = new ApplicationUserRepository(_context);
            OrderHeaderRepo = new OrderHeaderRepository(_context);
            OrderDetailRepo = new OrderDetailRepository(_context);
            ProductImageRepo = new ProductImageRepository(_context);
            ShopCartItemRepo = new ShopCartItemRepository(_context);
            OrderRepo = new OrderRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
