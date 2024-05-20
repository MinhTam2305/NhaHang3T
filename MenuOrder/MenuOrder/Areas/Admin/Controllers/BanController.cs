using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Areas.Admin.Controllers
{
    public class BanController : Controller
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
            return View(db.FoodTables.ToList().OrderBy(n => n.IdTable).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FoodTable table, FormCollection f)
        {

            if (f["trangthai"]==null)
            {
                ViewBag.trangthai = f["trangthai"];
            }
           
                if (ModelState.IsValid)
                {


   
                table.Status = f["trangthai"];
                   
                  
                    db.FoodTables.InsertOnSubmit(table);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }

            

            return View();
        }
        public ActionResult Details(int id)
        {
            var category = db.FoodTables.SingleOrDefault(n => n.IdTable == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(category);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var table = db.FoodTables.SingleOrDefault(n => n.IdTable == id);
            if (table == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(table);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var table = db.FoodTables.SingleOrDefault(n => n.IdTable == id);
            if (table == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.FoodTables.DeleteOnSubmit(table);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var table = db.FoodTables.SingleOrDefault(n => n.IdTable == id);
            if (table == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(table);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var table = db.FoodTables.SingleOrDefault(n => n.IdTable == int.Parse(f["IdTable"]));


            if (f["trangthai"] == null)
            {
                ViewBag.trangthai = f["trangthai"];
            }

            if (ModelState.IsValid)
            {
                table.Status = f["trangthai"];
                db.FoodTables.InsertOnSubmit(table);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }


            return View(table);

        }
    }
}