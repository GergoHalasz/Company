using Moq;
using Xunit;
using Company.API.Services;
using Company.API.Repositories;
using Company.API.Models;
using Company.API.DTOs;
using AutoMapper;

namespace Company.Tests
{
	public class CompanyServiceTests
	{
		private readonly Mock<ICompanyRepository> _mockRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly CompanyService _companyService;

		public CompanyServiceTests()
		{
			_mockRepository = new Mock<ICompanyRepository>();
			_mockMapper = new Mock<IMapper>();

			_companyService = new CompanyService(_mockRepository.Object, _mockMapper.Object);
		}

		[Fact]
		public async Task GetByIdAsync_ValidId_ReturnsCompanyDto()
		{
			var company = new CompanyEntity { Id = 1, Name = "Test Company", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" };
			var companyDto = new GetCompanyDto { Id = 1, Name = "Test Company", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" };

			_mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);
			_mockMapper.Setup(m => m.Map<GetCompanyDto>(company)).Returns(companyDto);

			var result = await _companyService.GetByIdAsync(1);

			Assert.NotNull(result);
			Assert.Equal("Test Company", result.Name);
			_mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
		}

		[Fact]
		public async Task GetByIdAsync_InvalidId_ReturnsNull()
		{
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CompanyEntity?)null);

			var result = await _companyService.GetByIdAsync(99);

			Assert.Null(result);
			_mockRepository.Verify(r => r.GetByIdAsync(99), Times.Once);
		}

		[Fact]
		public async Task GetAllAsync_ReturnsListOfCompanyDto()
		{
			var companies = new List<CompanyEntity>
			{
				new CompanyEntity { Id = 1, Name = "Test Company 1", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" },
				new CompanyEntity { Id = 2, Name = "Test Company 2", ISIN = "US987654321", Exchange = "Test Stock", Ticker = "TEST" }
			};

			var companyDtos = new List<GetCompanyDto>
			{
				new GetCompanyDto { Id = 1, Name = "Test Company 1", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" },
				new GetCompanyDto { Id = 2, Name = "Test Company 2", ISIN = "US987654321", Exchange = "Test Stock", Ticker = "TEST" }
			};

			_mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(companies);
			_mockMapper.Setup(m => m.Map<List<GetCompanyDto>>(companies)).Returns(companyDtos);

			var result = await _companyService.GetAllAsync();

			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
			_mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_InvalidISIN_ReturnsError()
		{
			var companyDto = new CompanyDto { Name = "New Company", ISIN = "123456789", Exchange = "Test Stock", Ticker = "TEST" };
			var company = new CompanyEntity { Name = "New Company", ISIN = "123456789", Exchange = "Test Stock", Ticker = "TEST" };

			_mockMapper.Setup(m => m.Map<CompanyEntity>(companyDto)).Returns(company);
			_mockRepository.Setup(r => r.ExistsByIsinAsync(It.IsAny<string>())).ReturnsAsync(false);

			var result = await _companyService.CreateAsync(companyDto);

			Assert.False(result.Success);
			Assert.Equal("ISIN must start with two letters.", result.ErrorMessage);
		}

		[Fact]
		public async Task CreateAsync_ValidCompany_ReturnsSuccess()
		{
			var companyDto = new CompanyDto { Name = "New Company", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" };
			var companyEntity = new CompanyEntity { Name = "New Company", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" };

			_mockRepository.Setup(r => r.ExistsByIsinAsync(It.IsAny<string>())).ReturnsAsync(false);
			_mockMapper.Setup(m => m.Map<CompanyEntity>(companyDto)).Returns(companyEntity);
			_mockRepository.Setup(r => r.AddAsync(companyEntity)).Returns(Task.CompletedTask);
			_mockMapper.Setup(m => m.Map<CompanyDto>(companyEntity)).Returns(companyDto);

			var result = await _companyService.CreateAsync(companyDto);

			Assert.True(result.Success);
			Assert.NotNull(result.Company);
			Assert.Equal("New Company", result.Company.Name);
			_mockRepository.Verify(r => r.AddAsync(It.IsAny<CompanyEntity>()), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_DuplicateIsin_ReturnsError()
		{
			var companyDto = new CompanyDto { Name = "Duplicate Company", ISIN = "US123456789", Exchange = "Test Stock", Ticker = "TEST" };

			_mockRepository.Setup(r => r.ExistsByIsinAsync("US123456789")).ReturnsAsync(true);

			var result = await _companyService.CreateAsync(companyDto);

			Assert.False(result.Success);
			Assert.Equal("Company with this ISIN already exists.", result.ErrorMessage);
			_mockRepository.Verify(r => r.AddAsync(It.IsAny<CompanyEntity>()), Times.Never);
		}

		[Fact]
		public async Task GetByIsinAsync_ValidIsin_ReturnsCompanyDto()
		{
			var companyEntity = new CompanyEntity { Name = "Test Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };
			var companyDto = new GetCompanyDto { Name = "Test Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" };

			_mockRepository.Setup(r => r.GetByIsinAsync("US123456789")).ReturnsAsync(companyEntity);
			_mockMapper.Setup(m => m.Map<GetCompanyDto>(companyEntity)).Returns(companyDto);

			var result = await _companyService.GetByIsinAsync("US123456789");

			Assert.NotNull(result);
			Assert.Equal("Test Company", result.Name);
			_mockRepository.Verify(r => r.GetByIsinAsync("US123456789"), Times.Once);
		}

		[Fact]
		public async Task GetByIsinAsync_InvalidIsin_ReturnsNull()
		{
			_mockRepository.Setup(r => r.GetByIsinAsync(It.IsAny<string>())).ReturnsAsync((CompanyEntity?)null);

			var result = await _companyService.GetByIsinAsync("INVALID");

			Assert.Null(result);
			_mockRepository.Verify(r => r.GetByIsinAsync("INVALID"), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_ValidId_ReturnsTrue()
		{
			var companyEntity = new CompanyEntity { Id = 1, Name = "Old Company", ISIN = "US567890123", Exchange = "Old Stock", Ticker = "OLD" };
			var updatedDto = new CompanyDto { Name = "Updated Company", ISIN = "US567890123", Exchange = "New Stock", Ticker = "NEW" };

			_mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(companyEntity);
			_mockMapper.Setup(m => m.Map(updatedDto, companyEntity));
			_mockRepository.Setup(r => r.UpdateAsync(companyEntity)).Returns(Task.CompletedTask);

			var result = await _companyService.UpdateAsync(1, updatedDto);

			Assert.True(result);
			_mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CompanyEntity>()), Times.Once);
		}

		[Fact]
		public async Task UpdateAsync_InvalidId_ReturnsFalse()
		{
			_mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CompanyEntity?)null);

			var result = await _companyService.UpdateAsync(99, new CompanyDto { Name = "Test Company", ISIN = "US123456789", Exchange = "Test Exchange", Ticker = "TST" });

			Assert.False(result);
			_mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CompanyEntity>()), Times.Never);
		}

	}
}
