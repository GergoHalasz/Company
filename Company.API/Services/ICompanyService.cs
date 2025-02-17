using Company.API.DTOs;

namespace Company.API.Services
{
	public interface ICompanyService
	{
		Task<GetCompanyDto?> GetByIdAsync(int id);
		Task<GetCompanyDto?> GetByIsinAsync(string isin);
		Task<List<GetCompanyDto>> GetAllAsync();
		Task<(bool Success, string? ErrorMessage, GetCompanyDto? Company)> CreateAsync(CompanyDto companyDto);
		Task<bool> UpdateAsync(int id, CompanyDto updatedCompanyDto);
	}
}
