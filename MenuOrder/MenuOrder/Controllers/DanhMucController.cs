using MenuOrder.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Controllers
{
    public class DanhMucController : Controller
    {
        int ma = 0;
        dbDataContext db = new dbDataContext();
        // GET: XemChiTietDanhMuc

        public ActionResult Index(int id, int? page)
        {

            ViewBag.maDanhMuc = id;
            int iPageNum = (page ?? 1);
            int iPageSize = 8;

            // Thay đổi kiểu dữ liệu của câu truy vấn để trả về các đối tượng Product
            var products = from s in db.Foods
                           where s.IdFC == id
                           select s;

            // Chuyển danh sách sản phẩm sang PagedList
            var pagedProductList = products.ToPagedList(iPageNum, iPageSize);

            return View(pagedProductList);
        }
        public ActionResult TangDan(int id, int? page)
        {
            ViewBag.maDanhMuc = id;
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Foods
                          where s.IdFC == id
                          orderby s.Price descending
                          select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult GiamDan(int id, int? page)
        {
            ViewBag.maDanhMuc = id;
            int iPageNum = (page ?? 1);
            int iPageSize = 8;
            var product = from s in db.Foods
                          where s.IdFC == id
                          orderby s.Price ascending
                          select s;
            return View(product.ToPagedList(iPageNum, iPageSize));
        }
    }
}