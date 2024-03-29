using Microsoft.AspNetCore.Mvc;

namespace ClothX.Controllers
{
    public class ErrorController : Controller
    {

        // Action to display the server error page
        public IActionResult ServerError()
        {
            return View();
        }
        // Action to display the not found page
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
