using MVCProject.DataAccess.Data;
using MVCProject.DataAccess.Repository.IRepository;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.DataAccess.Repository
{
    public class CategoryRespository : Repository<Category>,ICategoryRepository
    {

        private readonly ApplicationDbContext _db;
        public CategoryRespository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);        }
    }
}
