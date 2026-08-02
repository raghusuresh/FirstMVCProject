using Microsoft.AspNetCore.Mvc;

namespace FirstMVCProject.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View();
        }
    }
}
