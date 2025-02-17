using Company.API.Controllers;
using Company.API.DTOs;
using Company.API.Services;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Company.Tests
{
	public class CompanyControllerTests
	{
		private readonly Mock<ICompanyService> _mockService;
		private readonly CompanyController _controller;

		public CompanyControllerTests()
		{
			_mockService = new Mock<ICompanyService>();
			_controller = new CompanyController(_mockService.Object);
		}

		#region GetAll Tests

		[Fact]
		public async Task GetAll_ReturnsOkResult_WithListOfCompanies()
		{
			var mockCompanies = new List<GetCompanyDto> {
				new GetCompanyDto { Id = 1, Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" },
				new GetCompanyDto { Id = 2, Name = "Company 2", ISIN = "US987654321", Exchange = "Test Exchange", Ticker = "TST" }
			};

			_mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(mockCompanies);

			var result = await _controller.GetAll();

			var okResult = Assert.IsType<OkObjectResult>(result);
			var companies = Assert.IsAssignableFrom<List<GetCompanyDto?>>(okResult.Value);
			Assert.Equal(2, companies.Count);
		}

		#endregion

		#region GetById Tests

		[Fact]
		public async Task GetById_ReturnsOkResult_WithCompany_WhenFound()
		{
			var mockCompany = new GetCompanyDto { Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(mockCompany);

			var result = await _controller.GetById(1);

			var okResult = Assert.IsType<OkObjectResult>(result);
			var company = Assert.IsType<GetCompanyDto?>(okResult.Value);
			Assert.Equal("Company 1", company.Name);
		}

		[Fact]
		public async Task GetById_ReturnsNotFound_WhenCompanyNotFound()
		{
			_mockService.Setup(service => service.GetByIdAsync(999)).ReturnsAsync((GetCompanyDto)null);

			var result = await _controller.GetById(999);

			Assert.IsType<NotFoundResult>(result);
		}

		#endregion

		#region GetByIsin Tests

		[Fact]
		public async Task GetByIsin_ReturnsOkResult_WithCompany_WhenFound()
		{
			var mockCompany = new GetCompanyDto { Id = 1, Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.GetByIsinAsync("US123456789")).ReturnsAsync(mockCompany);

			var result = await _controller.GetByIsin("US123456789");

			var okResult = Assert.IsType<OkObjectResult>(result);
			var company = Assert.IsType<GetCompanyDto>(okResult.Value);
			Assert.Equal("Company 1", company.Name);
		}

		[Fact]
		public async Task GetByIsin_ReturnsNotFound_WhenCompanyNotFound()
		{
			_mockService.Setup(service => service.GetByIsinAsync("US999999999")).ReturnsAsync((GetCompanyDto)null);

			var result = await _controller.GetByIsin("US999999999");

			Assert.IsType<NotFoundResult>(result);
		}

		#endregion

		#region Create Tests

		[Fact]
		public async Task Create_ReturnsCreatedResult_WithCompany_WhenSuccessful()
		{
			var companyDto = new CompanyDto { Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			var createdCompany = new GetCompanyDto { Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.CreateAsync(companyDto)).ReturnsAsync((true, null, createdCompany));

			var result = await _controller.Create(companyDto);

			var createdResult = Assert.IsType<CreatedAtActionResult>(result); 
			var company = Assert.IsType<GetCompanyDto>(createdResult.Value); 
			Assert.Equal("Company 1", company.Name); 
			Assert.Equal(201, createdResult.StatusCode); 
		}

		[Fact]
		public async Task Create_ReturnsBadRequest_WhenCreationFails()
		{
			var companyDto = new CompanyDto { Name = "Company 1", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.CreateAsync(companyDto)).ReturnsAsync((false, "Error creating company", null));

			var result = await _controller.Create(companyDto);

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Error creating company", badRequestResult.Value);
		}

		#endregion

		#region Update Tests

		[Fact]
		public async Task Update_ReturnsNoContent_WhenSuccessful()
		{
			var updatedCompanyDto = new CompanyDto { Name = "Updated Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.UpdateAsync(1, updatedCompanyDto)).ReturnsAsync(true);

			var result = await _controller.Update(1, updatedCompanyDto);

			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task Update_ReturnsNotFound_WhenCompanyNotFound()
		{
			var updatedCompanyDto = new CompanyDto { Name = "Updated Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			_mockService.Setup(service => service.UpdateAsync(999, updatedCompanyDto)).ReturnsAsync(false);

			var result = await _controller.Update(999, updatedCompanyDto);

			Assert.IsType<NotFoundResult>(result);
		}

		#endregion
	}
}
