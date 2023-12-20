using Microsoft.AspNetCore.Mvc;

namespace Shop.MVC.Controllers
{
	public class SiteBaseController : Controller
	{
		protected string ErrorMessage = "ErrorMessage";
		protected string SuccessMessage = "SuccessMessage";
		protected string InfoMessage = "InfoMessage";
		protected string WarningMessage = "WarningMessage";
	}
}
