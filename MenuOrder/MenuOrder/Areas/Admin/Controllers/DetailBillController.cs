using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Areas.Admin.Controllers
{
    public class DetailBillController : Controller
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
            return View(db.DetailBills.ToList().OrderBy(n => n.IdDetailBill).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.idBill = new SelectList(db.Bills.ToList().OrderBy(n => n.IdBill), "IdBill", "IdBill");
            ViewBag.idFood = new SelectList(db.Foods.ToList().OrderBy(n => n.NameFood), "IdFood", "NameFood");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(DetailBill detailbill, FormCollection f)
        {
            ViewBag.idBill = new SelectList(db.Bills.ToList().OrderBy(n => n.IdBill), "IdBill", "IdBill");
            ViewBag.idFood = new SelectList(db.Foods.ToList().OrderBy(n => n.NameFood), "IdFood", "NameFood");

           

            if (ModelState.IsValid)
            {



                detailbill.IdBill = int.Parse(f["idBill"]);
                detailbill.IdFood = int.Parse(f["idFood"]);

                db.DetailBills.InsertOnSubmit(detailbill);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }



            return View();
        }
  
        public ActionResult Delete(int id)
        {
            var detailbill = db.DetailBills.SingleOrDefault(n => n.IdDetailBill == id);
            if (detailbill == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(detailbill);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var detailbill = db.DetailBills.SingleOrDefault(n => n.IdDetailBill == id);
            if (detailbill == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.DetailBills.DeleteOnSubmit(detailbill);
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