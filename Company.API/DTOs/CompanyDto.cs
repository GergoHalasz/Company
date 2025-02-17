namespace Company.API.DTOs
{
	public class CompanyDto
	{
		public required string Name { get; set; }
		public required string Exchange { get; set; }
		public required string Ticker { get; set; }
		public required string ISIN { get; set; }
		public string? WebsiteUrl { get; set; }
	}
}
