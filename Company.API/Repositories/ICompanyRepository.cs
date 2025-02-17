using Company.API.Models;

namespace Company.API.Repositories
{
	public interface ICompanyRepository
	{
		Task<CompanyEntity?> GetByIdAsync(int id);
		Task<CompanyEntity?> GetByIsinAsync(string isin);
		Task<List<CompanyEntity>> GetAllAsync();
		Task AddAsync(CompanyEntity company);
		Task UpdateAsync(CompanyEntity company);
		Task<bool> ExistsByIsinAsync(string isin);
	}
}
