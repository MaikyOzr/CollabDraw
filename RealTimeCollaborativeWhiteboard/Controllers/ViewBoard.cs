using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RealTimeCollaborativeWhiteboard.Controllers
{
    public class ViewBoard : Controller
    {
        // GET: ViewBoard
        public ActionResult Index()
        {
            return View();
        }
    }
}
