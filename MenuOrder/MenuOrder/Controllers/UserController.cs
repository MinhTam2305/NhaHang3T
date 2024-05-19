using MenuOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Controllers
{
    public class UserController : Controller
    {
        dbDataContext db = new dbDataContext();
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f, string url)
        {

            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];


            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập (;-;)";

            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu ";
            }
            else
            {

                Account kh = db.Accounts.SingleOrDefault(n => n.Username == sTenDN && n.Password == GetMD5(sMatKhau));
                if (kh != null)
                {
                    ViewBag.ThongBao = "dang nhap thanh cong";
                    Session["TaiKhoan"] = kh;
                    Session["ten"] = kh.Username;
                    Session["id"] = kh.Id;
                    Session["MatKhau"] = sMatKhau;
                    if (f["remember"].Contains("true"))
                    {
                        Response.Cookies["Username"].Value = sTenDN;
                        Response.Cookies["Password"].Value = sMatKhau;
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                    }
                    if (url == null)
                        return RedirectToAction("Index", "Menu");
                    else
                        return Redirect(url);
                }
                else
                {
                    ViewBag.ThongBao = "ten dang nhap hoac mk k dung";

                }

            }
            Session["XLDangNhap"] = true;
            return View();
        }
        public ActionResult DangKy()
        {

            return View();
        }
        public ActionResult DangXuat(string url)
        {
            Session["abc"] = null;
            Session["TaiKhoan"] = null;
            Session["LoginGF"] = null;
            if (url == null)
                return RedirectToAction("Index", "Menu");
            else
                return Redirect(url);
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
        public ActionResult TTUser()
        {

            var nd = db.Accounts.SingleOrDefault(n => n.Id == (int)Session["id"]);
            if (nd == null)
            {


                return RedirectToAction("DangNhap");
            }
            return View(nd);
        }
    }
}