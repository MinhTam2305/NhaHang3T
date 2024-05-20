using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        dbDataContext db = new dbDataContext();
        public ActionResult Index()
        {
            var product = from s in db.Foods select s;
            return View(product);
        }
        public ActionResult SliderPartial()
        {
            return View();
        }
        public ActionResult HeaderPartial()
        {
            return View();
        }
        public ActionResult HinhSlider()
        {
            var item = from c in db.Sliders select c;
            return PartialView(item);
        }
        public ActionResult ItemPatrial()
        {
            var item = from c in db.FoodCategories select c;
            return PartialView(item);
        }
        public ActionResult ChiTietMon(int id)
        {
            var products = from s in db.Foods
                           where s.IdFood == id
                           select s;
            return View(products.Single());
        }
        public ActionResult ItemSanPhamOrder(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Foods where s.IdFC == 1 select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult DoUong(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Foods where s.IdFC == 2 select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Ban()
        {
            var item = from c in db.FoodTables select c;
            return PartialView(item);
        }
    }
}