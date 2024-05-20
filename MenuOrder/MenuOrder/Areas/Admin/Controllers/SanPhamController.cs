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
    public class SanPhamController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/SanPham
        public ActionResult Index(int? Page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int iPageNum = (Page ?? 1);
            int iPageSize = 7;
            return View(db.Foods.ToList().OrderBy(n => n.IdFood).ToPagedList(iPageNum, iPageSize));


        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IdFC = new SelectList(db.FoodCategories.ToList().OrderBy(n => n.FoodCategory1), "IdFC", "FoodCategory1");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Food food, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.IdFC = new SelectList(db.FoodCategories.ToList().OrderBy(n => n.FoodCategory1), "IdFC", "FoodCategory1");
            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hay chon anh bia";
                ViewBag.tenSanPham = f["tenSanPham"];
                ViewBag.nhaXuatBan = f["trangthai"];
                ViewBag.SoLuong = int.Parse(f["SoLuong"]);
                ViewBag.gia = int.Parse(f["gia"]);
                ViewBag.IdFC = new SelectList(db.FoodCategories.ToList().OrderBy(n => n.FoodCategory1), "IdFC", "FoodCategory1", int.Parse(f["IdFC"]));

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/products"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }

                    food.NameFood = f["tenSanPham"];
                    food.Status = f["trangthai"];
                    food.anh = sFileName;
                    food.Quantity = int.Parse(f["soLuong"]);
                    food.Price = int.Parse(f["gia"]);
                    food.IdFC = int.Parse(f["IdFC"]);
                    db.Foods.InsertOnSubmit(food);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
        public ActionResult Details(int id)
        {
            var product = db.Foods.SingleOrDefault(n => n.IdFood == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(product);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Foods.SingleOrDefault(n => n.IdFood == id);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = db.Foods.SingleOrDefault(n => n.IdFood == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            var ctdh = db.DetailBills.Where(ct => ct.IdFood == id);


            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "San pham nay co trong bang chi tiet dat hang <br>" + "neu muon xoa thi phai xoa het ma san pham nay trong bang chi tiet dat hang";
                return View(sach);
            }

            db.Foods.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var food = db.Foods.SingleOrDefault(n => n.IdFood == id);
            if (food == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.IdFC = new SelectList(db.FoodCategories.ToList().OrderBy(n => n.FoodCategory1), "IdFC", "FoodCategory1", food.IdFC);

            return View(food);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var food = db.Foods.SingleOrDefault(n => n.IdFood == int.Parse(f["IdFood"]));
            ViewBag.IdFC = new SelectList(db.FoodCategories.ToList().OrderBy(n => n.FoodCategory1), "IdFC", "FoodCategory1", food.IdFC);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/img/products"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }

                    food.anh = sFileName;
                }
                food.NameFood = f["NameFood"];
                food.IdFC = int.Parse(f["IdFC"]);
                food.Status = f["Status"];
                food.Quantity = int.Parse(f["Quantity"]);
                food.Price = int.Parse(f["Price"]);
               
             
                db.SubmitChanges();
                return RedirectToAction("Index");

            }
            
            return View(food);

        }
    }
}