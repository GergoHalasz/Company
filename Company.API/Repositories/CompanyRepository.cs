using Company.API.Data;
using Company.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Repositories
{
	public class CompanyRepository : ICompanyRepository
	{
		private readonly AppDbContext _context;

		public CompanyRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<CompanyEntity?> GetByIdAsync(int id) => await _context.Companies.FindAsync(id);

		public async Task<CompanyEntity?> GetByIsinAsync(string isin) =>
			await _context.Companies.FirstOrDefaultAsync(c => c.ISIN == isin);

		public async Task<List<CompanyEntity>> GetAllAsync() => await _context.Companies.ToListAsync();

		public async Task AddAsync(CompanyEntity company)
		{
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(CompanyEntity company)
		{
			_context.Companies.Update(company);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsByIsinAsync(string isin) =>
			await _context.Companies.AnyAsync(c => c.ISIN == isin);
	}
}
