using System.Web.Mvc;

namespace YTech.IM.Paramita.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
