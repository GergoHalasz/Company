using Microsoft.AspNetCore.Mvc;

namespace Company.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CompanyController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
