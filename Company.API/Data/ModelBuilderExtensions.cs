using Microsoft.EntityFrameworkCore;
namespace Company.API.Data;
public static class ModelBuilderExtensions
{
	public static ModelBuilder Seed(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Models.CompanyEntity>().HasData(
		new Models.CompanyEntity
		{
			Id = 1,
			Name = "Apple Inc.",
			Exchange = "NASDAQ",
			Ticker = "AAPL",
			ISIN = "US0378331005",
			WebsiteUrl = "http://www.apple.com",
		},
		new Models.CompanyEntity
		{
			Id = 2,
			Name = "British Airways Plc",
			Exchange = "Pink Sheets",
			Ticker = "BAIRY",
			ISIN = "US1104193065",
		},
		new Models.CompanyEntity
		{
			Id = 3,
			Name = "Heineken NV",
			Exchange = "Euronext Amsterdam",
			Ticker = "HEIA",
			ISIN = "NL0000009165",
		},
		new Models.CompanyEntity
		{
			Id = 4,
			Name = "Panasonic Corp",
			Exchange = "Tokyo Stock Exchange",
			Ticker = "6752",
			ISIN = "JP3866800000",
			WebsiteUrl = "http://www.panasonic.co.jp",
		},
		new Models.CompanyEntity
		{
			Id = 5,
			Name = "Porsche Automobil",
			Exchange = "Deutsche Börse",
			Ticker = "PAH3",
			ISIN = "DE000PAH0038",
			WebsiteUrl = "https://www.porsche.com/",
		}
		);
		return modelBuilder;
	}
}