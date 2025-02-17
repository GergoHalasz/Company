using AutoMapper;
using Company.API.DTOs;

public class CompanyProfile : Profile
{
	public CompanyProfile()
	{
		CreateMap<Company.API.Models.CompanyEntity, CompanyDto>().ReverseMap();
		CreateMap<Company.API.Models.CompanyEntity, GetCompanyDto>();
	}
}