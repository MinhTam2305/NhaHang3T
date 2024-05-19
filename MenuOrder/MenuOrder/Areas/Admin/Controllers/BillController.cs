using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Areas.Admin.Controllers
{
    public class BillController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/DonHang
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;

            return View(db.Bills.ToList().OrderBy(n => n.IdBill).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dh = db.Bills.SingleOrDefault(n => n.IdBill == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.IdTable = new SelectList(db.FoodTables.ToList().OrderBy(n => n.IdTable), "IdTable", "IdTable", dh.IdBill);
            ViewBag.IdStaff = new SelectList(db.Staffs.ToList().OrderBy(n => n.NameStaff), "IdStaff", "NameStaff", dh.IdBill);
            return View(dh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var dh = db.Bills.SingleOrDefault(n => n.IdBill == int.Parse(f["IdBill"]));
            ViewBag.IdTable = new SelectList(db.FoodTables.ToList().OrderBy(n => n.IdTable), "IdTable", "IdTable", dh.IdBill);
            ViewBag.IdStaff = new SelectList(db.Staffs.ToList().OrderBy(n => n.NameStaff), "IdStaff", "NameStaff", dh.IdBill);
           
                if (ModelState.IsValid)
                {

                    dh.IdBill = int.Parse(f["IdBill"]);
                    dh.TimeCome = Convert.ToDateTime(f["TimeCome"]);
                    if (f["TimeOut"] == null)
                    {
                        dh.TimeOut = Convert.ToDateTime(f["TimeOut"]);
                    }
                    dh.Status = f["TrangThai"];
                    dh.Discount = f["Discount"];
                    dh.PaymentMethod = f["PaymentMethod"]; ;
                    dh.TotalMoney = int.Parse(f["TotalMoney"]);
                    dh.IdTable = int.Parse(f["IdTable"]);
                    dh.IdStaff = int.Parse(f["IdStaff"]);
                    db.SubmitChanges();
                    return RedirectToAction("Index");

                
            }
            
            return View(dh);

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var bill = db.Bills.SingleOrDefault(n => n.IdBill == id);
            if (bill == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(bill);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var bill = db.Bills.SingleOrDefault(n => n.IdBill == id);
            if (bill == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            var ctdh = db.DetailBills.Where(ct => ct.IdBill == id);


            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "San pham nay co trong bang chi tiet dat hang neu muon xoa thi phai xoa het ma san pham nay trong bang chi tiet dat hang";
                return View(bill);
            }

            db.Bills.DeleteOnSubmit(bill);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Detail(int id)
        {
            var bill = db.Bills.SingleOrDefault(n => n.IdBill == id);
            if (bill == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(bill);
        }
    }
}