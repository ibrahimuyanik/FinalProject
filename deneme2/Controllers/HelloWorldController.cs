using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace deneme2.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
