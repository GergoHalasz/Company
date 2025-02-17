using Microsoft.AspNetCore.Mvc;
using Company.API.DTOs;
using Company.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Company.API.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
	private readonly ICompanyService _service;

	public CompanyController(ICompanyService service)
	{
		_service = service;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetAll()
	{
		var companies = await _service.GetAllAsync();
		return Ok(companies);
	}

	[HttpGet("{id}")]
	[Authorize]
	public async Task<IActionResult> GetById(int id)
	{
		var company = await _service.GetByIdAsync(id);
		return company == null ? NotFound() : Ok(company);
	}

	[HttpGet("isin/{isin}")]
	[Authorize]
	public async Task<IActionResult> GetByIsin(string isin)
	{
		var company = await _service.GetByIsinAsync(isin);
		return company == null ? NotFound() : Ok(company);
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Create([FromBody] CompanyDto companyDto)
	{
		var (success, errorMessage, createdCompany) = await _service.CreateAsync(companyDto);

		if (!success)
		{
			return BadRequest(errorMessage);
		}

		return CreatedAtAction(
		nameof(GetById),
		new { id = createdCompany!.Id },
		createdCompany);
	}

	[HttpPut("{id}")]
	[Authorize]
	public async Task<IActionResult> Update(int id, [FromBody] CompanyDto updatedCompanyDto)
	{
		var success = await _service.UpdateAsync(id, updatedCompanyDto);
		return success ? NoContent() : NotFound();
	}
}