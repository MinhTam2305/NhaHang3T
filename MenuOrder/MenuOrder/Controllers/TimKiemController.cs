using MenuOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Controllers
{
    public class TimKiemController : Controller
    {
        dbDataContext db = new dbDataContext();
        public ActionResult TimKiem(string q)
        {

            if (string.IsNullOrEmpty(q))
            {

                return View(new List<Food>());
            }

            string qLower = q.ToLower();


            List<Food> listSach = db.Foods.Where(ten => ten.NameFood.ToLower().Contains(qLower)).ToList();

            return View(listSach);
        }
    }
}