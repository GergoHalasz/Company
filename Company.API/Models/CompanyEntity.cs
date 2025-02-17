namespace Company.API.Models
{
	public class CompanyEntity
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Exchange { get; set; }
		public required string Ticker { get; set; }
		public required string ISIN { get; set; }
		public string? WebsiteUrl { get; set; }
	}
}
