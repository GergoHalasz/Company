using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Company.API.Data;
using Company.API.Models;
using Company.API.Repositories;

namespace Company.Tests
{
	public class CompanyRepositoryTests
	{
		private readonly AppDbContext _context;
		private readonly CompanyRepository _repository;

		public CompanyRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb")
				.Options;

			_context = new AppDbContext(options);
			_repository = new CompanyRepository(_context);
		}

		[Fact]
		public async Task AddAsync_ValidCompany_CallsAddAndSaveChanges()
		{
			var company = new CompanyEntity { Id = 1, Name = "New Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };

			await _repository.AddAsync(company);

			var result = await _context.Companies.FindAsync(1);

			Assert.NotNull(result);
			Assert.Equal("New Company", result.Name);
		}

		[Fact]
		public async Task GetByIdAsync_ValidId_ReturnsCompany()
		{
			var company = new CompanyEntity { Id = 2, Name = "Existing Company", ISIN = "US987654321", Exchange = "Test Exchange", Ticker = "TST" };
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();

			var result = await _repository.GetByIdAsync(2);

			Assert.NotNull(result);
			Assert.Equal("Existing Company", result.Name);
		}

		[Fact]
		public async Task GetByIsinAsync_ValidIsin_ReturnsCompany()
		{
			var company = new CompanyEntity { Id = 3, Name = "Company A", ISIN = "US111111111", Exchange = "Test Exchange", Ticker = "TST" };
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();

			var result = await _repository.GetByIsinAsync("US111111111");

			Assert.NotNull(result);
			Assert.Equal("Company A", result.Name);
		}

		[Fact]
		public async Task UpdateAsync_ValidCompany_UpdatesSuccessfully()
		{
			var company = new CompanyEntity { Id = 6, Name = "Before Update", ISIN = "US555555555", Exchange = "Test Exchange", Ticker = "TST" };
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();

			company.Name = "After Update";
			await _repository.UpdateAsync(company);

			var updatedCompany = await _context.Companies.FindAsync(6);
			Assert.NotNull(updatedCompany);
			Assert.Equal("After Update", updatedCompany.Name);
		}

		[Fact]
		public async Task ExistsByIsinAsync_ExistingIsin_ReturnsTrue()
		{
			var company = new CompanyEntity { Id = 7, Name = "Exists Company", ISIN = "US777777777", Exchange = "Test Exchange", Ticker = "TST" };
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();

			var result = await _repository.ExistsByIsinAsync("US777777777");

			Assert.True(result);
		}

		[Fact]
		public async Task ExistsByIsinAsync_NonExistingIsin_ReturnsFalse()
		{
			var result = await _repository.ExistsByIsinAsync("INVALID_ISIN");

			Assert.False(result);
		}
	}
}
