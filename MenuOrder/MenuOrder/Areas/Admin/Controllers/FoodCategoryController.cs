using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Areas.Admin.Controllers
{
    public class FoodCategoryController : Controller
    {
        dbDataContext db = new dbDataContext();
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.FoodCategories.ToList().OrderBy(n => n.IdFC).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FoodCategory fc, FormCollection f)
        {

            if (ModelState.IsValid)
            {



                fc.FoodCategory1 = f["FoodCategory1"];


                db.FoodCategories.InsertOnSubmit(fc);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }



            return View();
        }
        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var fc = db.FoodCategories.SingleOrDefault(n => n.IdFC == id);
            if (fc == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(fc);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var fc = db.FoodCategories.SingleOrDefault(n => n.IdFC == id);
            
                if (fc == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.FoodCategories.DeleteOnSubmit(fc);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fc = db.FoodCategories.SingleOrDefault(n => n.IdFC == id);
            if (fc == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(fc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var fc = db.FoodCategories.SingleOrDefault(n => n.IdFC == int.Parse(f["IdFC"]));


            if (f["FoodCategory1"] == null)
            {
                ViewBag.trangthai = f["FoodCategory1"];
            }

            if (ModelState.IsValid)
            {
                fc.FoodCategory1 = f["FoodCategory1"];
                db.FoodCategories.InsertOnSubmit(fc);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }


            return View(fc);

        }
    }
}