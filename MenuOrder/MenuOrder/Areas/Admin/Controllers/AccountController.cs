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
    public class AccountController : Controller
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
            return View(db.Accounts.ToList().OrderBy(n => n.Id).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdTypeAccount = new SelectList(db.TypeAccounts.ToList().OrderBy(n => n.TypeAccount1), "IdAccount", "TypeAccount1");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Account nd, FormCollection f)
        {
            ViewBag.IdTypeAccount = new SelectList(db.TypeAccounts.ToList().OrderBy(n => n.TypeAccount1), "IdAccount", "TypeAccount1");

            var sHoTen = f["DisplayName"];
            var sTaiKhoan = f["Username"];
            var sMatKhau = f["Password"];
            var sDiaChi = f["Address"];
            var sEmail = f["Email"];
           
            var typeAccount = int.Parse(f["IdTypeAccount"]);

            if (db.Accounts.SingleOrDefault(n => n.Username == sTaiKhoan) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại.";
            }
            else if (db.Accounts.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng.";
            }
            else
            {
             
                nd.Username = sTaiKhoan;
                nd.Password = GetMD5(sMatKhau);
                nd.DisplayName = sHoTen;
                nd.Email = sEmail;
                nd.Address = sDiaChi;
                nd.IdTypeAccount = typeAccount;
                db.Accounts.InsertOnSubmit(nd);
                db.SubmitChanges();

                return Redirect("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nd = db.Accounts.SingleOrDefault(n => n.Id == id);
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
            var nd = db.Accounts.SingleOrDefault(n => n.Id == id);

            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.Accounts.DeleteOnSubmit(nd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.IdTypeAccount = new SelectList(db.TypeAccounts.ToList().OrderBy(n => n.TypeAccount1), "IdAccount", "TypeAccount1");
            var nd = db.Accounts.SingleOrDefault(n => n.Id == id);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            return View(nd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var nd = db.Accounts.SingleOrDefault(n => n.Id == int.Parse(f["Id"]));
            ViewBag.IdTypeAccount = new SelectList(db.TypeAccounts.ToList().OrderBy(n => n.TypeAccount1), "IdAccount", "TypeAccount1");

            var sHoTen = f["DisplayName"];
            var sTaiKhoan = f["Username"];
            var sMatKhau = f["Password"];
            var sDiaChi = f["Address"];
            var sEmail = f["Email"];
            var typeAccount = int.Parse(f["IdTypeAccount"]);

            if (ModelState.IsValid)
            {

                nd.Username = sTaiKhoan;
                nd.Password = GetMD5(sMatKhau);
                nd.DisplayName = sHoTen;
                nd.Email = sEmail;
                nd.Address = sDiaChi;
                nd.IdTypeAccount = typeAccount;

                db.SubmitChanges();
                return RedirectToAction("Index");

            }

            return View(nd);

        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}