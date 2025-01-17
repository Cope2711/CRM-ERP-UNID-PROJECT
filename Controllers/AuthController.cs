using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
