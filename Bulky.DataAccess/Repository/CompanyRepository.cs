using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Company company)
        {
            var companyDB = _context.Companies.Include(nameof(Company.Address)).FirstOrDefault(c=>c.Id == company.Id);

            if (companyDB != null)
            {
                companyDB.Name = company.Name;
                companyDB.CNPJ = company.CNPJ;
                companyDB.Address.StreetAddress = company.Address?.StreetAddress ?? companyDB.Address.StreetAddress;
                companyDB.Address.City = company.Address?.City ?? companyDB.Address.City;
                companyDB.Address.State = company.Address?.State ?? companyDB.Address.State;
                companyDB.Address.PostalCode = company.Address?.PostalCode ?? companyDB.Address.PostalCode;

                _context.Companies.Update(companyDB);
            }
        }
    }
}
