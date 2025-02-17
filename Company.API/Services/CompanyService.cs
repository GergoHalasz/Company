using AutoMapper;
using Company.API.DTOs;
using Company.API.Models;
using Company.API.Repositories;

namespace Company.API.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IMapper _mapper;

    public CompanyService(ICompanyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetCompanyDto?> GetByIdAsync(int id)
    {
        var company = await _repository.GetByIdAsync(id);
        return company == null ? null : _mapper.Map<GetCompanyDto>(company);
    }

    public async Task<GetCompanyDto?> GetByIsinAsync(string isin)
    {
        var company = await _repository.GetByIsinAsync(isin);
        return company == null ? null : _mapper.Map<GetCompanyDto>(company);
    }

    public async Task<List<GetCompanyDto>> GetAllAsync()
    {
        var companies = await _repository.GetAllAsync();
        return _mapper.Map<List<GetCompanyDto>>(companies);
    }

    public async Task<(bool Success, string? ErrorMessage, GetCompanyDto? Company)> CreateAsync(CompanyDto companyDto)
    {
        if (!companyDto.ISIN[..2].All(char.IsLetter))
        {
            return (false, "ISIN must start with two letters.", null);
        }

        if (await _repository.ExistsByIsinAsync(companyDto.ISIN))
        {
            return (false, "Company with this ISIN already exists.", null);
        }

        var company = _mapper.Map<CompanyEntity>(companyDto);
        await _repository.AddAsync(company);
        return (true, null, _mapper.Map<GetCompanyDto>(company));
    }

    public async Task<bool> UpdateAsync(int id, CompanyDto updatedCompanyDto)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null) return false;

        _mapper.Map(updatedCompanyDto, company);

        await _repository.UpdateAsync(company);
        return true;
    }
}
