using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WerewolfKill.Controllers
{
    public class RoomController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Wait()
        {
            return PartialView();
        }

        public ActionResult Game()
        {
            return PartialView();
        }
    }
}