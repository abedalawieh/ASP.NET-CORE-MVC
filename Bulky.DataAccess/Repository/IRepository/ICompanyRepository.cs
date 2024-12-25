using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void update(Company company);
    }
}
