using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebBanCoffeeABC.Models;
namespace WebBanCoffeeABC.Controllers
{
    public class ABCNewsController : Controller
    {
        QLCoffee_ABCEntities db = new QLCoffee_ABCEntities();
        // GET: News
        public ActionResult Index()
        {
            var items = db.tTinTucs.Take(5);
            return View(items);
        }
        
        public ActionResult Detail(int id)
        {
            var item = db.tTinTucs.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
    }
}