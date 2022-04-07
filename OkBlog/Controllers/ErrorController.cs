using Microsoft.AspNetCore.Mvc;

namespace OkBlog.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}
