using Company.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CompanyController(AppDbContext context) : ControllerBase
	{
		private readonly AppDbContext _context = context;

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] Models.Company company)
		{
			if (!company.ISIN[..2].All(char.IsLetter))
			{
				return BadRequest("ISIN must start with two letters.");
			}

			if (await _context.Companies.AnyAsync(c => c.ISIN == company.ISIN))
			{
				return BadRequest("Company with this ISIN already exists.");
			}

			_context.Companies.Add(company);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var company = await _context.Companies.FindAsync(id);
			return company == null ? NotFound() : Ok(company);
		}

		[HttpGet("isin/{isin}")]
		public async Task<IActionResult> GetByIsin(string isin)
		{
			var company = await _context.Companies.FirstOrDefaultAsync(c => c.ISIN == isin);
			return company == null ? NotFound() : Ok(company);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _context.Companies.ToListAsync());
		}

		[HttpPut("{id}")]	
		public async Task<IActionResult> Update(int id, [FromBody] Models.Company updatedCompany)
		{
			var company = await _context.Companies.FindAsync(id);
			if (company == null) return NotFound();

			company.Name = updatedCompany.Name;
			company.Exchange = updatedCompany.Exchange;
			company.Ticker = updatedCompany.Ticker;
			company.ISIN = updatedCompany.ISIN;
			company.WebsiteUrl = updatedCompany.WebsiteUrl;

			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
