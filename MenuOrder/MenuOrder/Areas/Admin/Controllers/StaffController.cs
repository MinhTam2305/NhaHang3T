using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MenuOrder.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        // GET: Admin/NguoiDung
        dbDataContext db = new dbDataContext();
        // GET: Admin/DanhMuc
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.Staffs.ToList().OrderBy(n => n.IdStaff).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.a = new SelectList(db.Accounts.ToList().OrderBy(n => n.Username), "Id", "Username");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Staff nv, FormCollection f)
        {
            ViewBag.a = new SelectList(db.Accounts.ToList().OrderBy(n => n.Username), "Id", "Username");

            try
            {
                
                var sNameStaff = f["NameStaff"];
                var sNumberPhone = f["NumberPhone"];

               /* if (db.Staffs.SingleOrDefault(n => n.NameStaff == sNameStaff) != null)
                {
                    ViewBag.ThongBao = "Tên nv đã tồn tại.";
                }
                else if (db.Staffs.SingleOrDefault(n => n.NumberPhone == int.Parse(sNumberPhone)) != null)
                {
                    ViewBag.ThongBao = "Email đã được sử dụng.";
                }
                else*/ if (!string.IsNullOrEmpty(f["a"]) && !string.IsNullOrEmpty(sNameStaff) && !string.IsNullOrEmpty(sNumberPhone))
                {
                    nv.IdAccount = int.Parse(f["a"]);
                    nv.NameStaff = sNameStaff;
                    nv.NumberPhone = int.Parse(sNumberPhone); // Store as string if the database field is varchar

                    db.Staffs.InsertOnSubmit(nv);
                    db.SubmitChanges();

                    ViewBag.ThongBao = "Staff created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ThongBao = "Please fill in all required fields.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Error: " + ex.Message;
            }

            return View();
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nd = db.Staffs.SingleOrDefault(n => n.IdStaff == id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nd);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nd = db.Staffs.SingleOrDefault(n => n.IdStaff == id);

            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.Staffs.DeleteOnSubmit(nd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Account = new SelectList(db.Accounts.ToList().OrderBy(n => n.Username), "Id", "Username");
            var nv = db.Staffs.SingleOrDefault(n => n.IdStaff == id);
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(nv);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nv = db.Staffs.SingleOrDefault(n => n.IdStaff == int.Parse(f["IdStaff"]));
            ViewBag.Account = new SelectList(db.Accounts.ToList().OrderBy(n => n.Username), "Id", "Username");

            var typeAccount = f["Account"];
            var sNameStaff = f["NameStaff"];

            var sNumberPhone = f["NumberPhone"];


            if (ModelState.IsValid)
            {

                nv.IdAccount = int.Parse(typeAccount);
                nv.NameStaff = sNameStaff;
                nv.NumberPhone = int.Parse(sNumberPhone);

                db.SubmitChanges();
                return RedirectToAction("Index");

            }

            return View(nv);

        }

    }
}