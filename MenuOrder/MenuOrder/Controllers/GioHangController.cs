using MenuOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MenuOrder.Controllers
{
    public class GioHangController : Controller
    {
    

        dbDataContext data = new dbDataContext();
     
        public ActionResult XoaSPKhoiGioHang(int id)
        {
            var detailBill = data.DetailBills.SingleOrDefault(n => n.IdDetailBill == id);
            data.DetailBills.DeleteOnSubmit(detailBill);
            data.SubmitChanges();
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int id, FormCollection f)
        {
            var detailBill = data.DetailBills.SingleOrDefault(n => n.IdDetailBill == id);
            var food = data.Foods.SingleOrDefault(n => n.IdFood == detailBill.IdFood);
            //Cap nhat gia trong detailbill
            detailBill.Quality = int.Parse(f["txtSoLuong"].ToString());
            detailBill.PriceDetailBill = int.Parse(f["txtSoLuong"].ToString()) * food.Price;
            //Cap nhat gia cua bill
            var updatedBill = data.Bills.FirstOrDefault(b => b.IdBill == detailBill.IdBill && b.Status == "Chưa thanh toán");
            if (updatedBill != null)
            {
                double totalMoney = 0;
                foreach (var detail in updatedBill.DetailBills)
                {
                    totalMoney += detail.PriceDetailBill ?? 0;
                }
                updatedBill.TotalMoney = totalMoney;
                data.SubmitChanges();
            }
            data.SubmitChanges();
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            int idBan = 0;
            HttpCookie selectedTableCookie = Request.Cookies["selectedTable"];
            string selectedTable = null;
            if (selectedTableCookie != null)
            {
                selectedTable = selectedTableCookie.Value;
                idBan = int.Parse(selectedTable);
            }

            // Khởi tạo DataLoadOptions
            DataLoadOptions options = new DataLoadOptions();
            options.LoadWith<Bill>(b => b.DetailBills);
            options.LoadWith<DetailBill>(d => d.Food);

            // Áp dụng DataLoadOptions vào DataContext
            data.LoadOptions = options;

            // Truy vấn cơ sở dữ liệu để lấy hóa đơn của bàn được chọn (tableId)
            var bill = data.Bills.FirstOrDefault(b => b.IdTable == idBan && b.Status == "Chưa thanh toán");

            if (bill == null)
            {
                Session["HoaDon"] = "Bàn không có hóa đơn! Hãy tạo hóa đơn";
                // Nếu không tìm thấy hóa đơn, bạn có thể xử lý hiển thị thông báo lỗi hoặc chuyển hướng người dùng đến trang khác
                return RedirectToAction("Index", "Menu"); // Ví dụ: chuyển hướng người dùng đến trang chủ
            }

            return View(bill);
        }

        public ActionResult GioHangPartial()
        {
            int idBan = 0;
            HttpCookie selectedTableCookie = Request.Cookies["selectedTable"];
            string selectedTable = null;
            if (selectedTableCookie != null)
            {
                selectedTable = selectedTableCookie.Value;
                idBan = int.Parse(selectedTable);
            }
            var bill = data.Bills.FirstOrDefault(b => b.IdTable == idBan && b.Status == "Chưa thanh toán");
            var detailBills = from c in data.DetailBills where c.IdBill == bill.IdBill select c;
            return PartialView(detailBills);

        }
        public ActionResult Create(Bill bill, int ms, DetailBill detailBill, string url)
        {
            if (Session["TaiKhoan"] != null)
            {
                int idBan = 0;
                HttpCookie selectedTableCookie = Request.Cookies["selectedTable"];
                string selectedTable = null;
                if (selectedTableCookie != null)
                {
                    selectedTable = selectedTableCookie.Value;
                    idBan = int.Parse(selectedTable);
                }
                string status = "Chưa thanh toán";
                Bill bill2 = data.Bills.FirstOrDefault(n => n.IdTable == idBan && n.Status == status);
                Food f = data.Foods.FirstOrDefault(n => n.IdFood == ms);
                if (bill2 == null)
                {




                    bill.IdAccount = 1;
                    bill.TimeCome = DateTime.Now;
                    bill.Status = "Chưa thanh toán";
                    bill.IdTable = idBan;
                    bill.TotalMoney = f.Price;
                    data.Bills.InsertOnSubmit(bill);
                    data.SubmitChanges();

                    // Tạo chi tiết hóa đơn mới

                    DetailBill newDetailBill = new DetailBill();
                    newDetailBill.IdBill = bill.IdBill; // Lấy idBill từ hóa đơn mới
                    newDetailBill.IdFood = f.IdFood;
                    newDetailBill.Quality = 1;
                    newDetailBill.PriceDetailBill = f.Price;
                    var table = data.FoodTables.FirstOrDefault(t => t.IdTable == idBan);
                    if (table != null)
                    {
                        table.Status = "Có khách";
                    }
                    Session["thembill"] = "Da them vao gio hang";
                    data.DetailBills.InsertOnSubmit(newDetailBill);
                    data.SubmitChanges();

                }
                else
                {

                    int idBillMoi = bill2.IdBill;

                    // Kiểm tra nếu món đồ ăn đã tồn tại trong chi tiết hóa đơn
                    DetailBill existingDetail = data.DetailBills.FirstOrDefault(d => d.IdBill == idBillMoi && d.IdFood == f.IdFood);
                    if (existingDetail != null)
                    {
                        // Nếu đã tồn tại, chỉ cập nhật số lượng và giá
                        existingDetail.Quality += 1;
                        existingDetail.PriceDetailBill = existingDetail.Quality * f.Price;
                    }
                    else
                    {
                        // Nếu chưa tồn tại, tạo mới một chi tiết hóa đơn
                        detailBill.IdBill = idBillMoi;
                        detailBill.IdFood = f.IdFood;
                        detailBill.Quality = 1;
                        detailBill.PriceDetailBill = f.Price;
                        data.DetailBills.InsertOnSubmit(detailBill);
                    }

                    Session["thembill"] = "Da them vao gio hang";
                    data.SubmitChanges();
                }
                // Cập nhật tổng tiền của hóa đơn
                var updatedBill = data.Bills.FirstOrDefault(b => b.IdTable == idBan && b.Status == status);
                if (updatedBill != null)
                {
                    double totalMoney = 0;
                    foreach (var detail in updatedBill.DetailBills)
                    {
                        totalMoney += detail.PriceDetailBill ?? 0;
                    }
                    updatedBill.TotalMoney = totalMoney;
                    data.SubmitChanges();
                }

                return Redirect(url);
            }
            
                Session["themgiohang"] = "Dang nhap de su sung chuc nang";
            return RedirectToAction("Index","Menu");

        }
      
       
        public ActionResult Ban()
        {
            var item = from c in data.FoodTables where c.Status=="Trống" select c;
            return PartialView(item);
        }

        public ActionResult XacNhanThanhToan(string paymentMethod)
        {
            int idBan = 0;
            HttpCookie selectedTableCookie = Request.Cookies["selectedTable"];
            string selectedTable = null;
            if (selectedTableCookie != null)
            {
                selectedTable = selectedTableCookie.Value;
                idBan = int.Parse(selectedTable);
            }

            var bill = data.Bills.FirstOrDefault(b => b.IdTable == idBan && b.Status == "Chưa thanh toán");
            if (bill != null)
            {
                bill.TimeOut = DateTime.Now;
                bill.Status = "Đã thanh toán";
                // Lưu giá trị phương thức thanh toán vào đối tượng hóa đơn
                bill.PaymentMethod = paymentMethod;
                data.SubmitChanges();
            }

            // Trả về một phản hồi thành công
            return Json(new { success = true });
        }

        public ActionResult XoaHoaDon()
        {
           

            int idBan = 0;
            HttpCookie selectedTableCookie = Request.Cookies["selectedTable"];
            string selectedTable = null;
            if (selectedTableCookie != null)
            {
                selectedTable = selectedTableCookie.Value;
                idBan = int.Parse(selectedTable);
            }
            var bill = data.Bills.FirstOrDefault(b => b.IdTable == idBan && b.Status == "Chưa thanh toán");
            if (bill != null)
            {
                // Lấy tất cả các mục chi tiết của hóa đơn
                foreach (var detail in bill.DetailBills.ToList())
                {
                    data.DetailBills.DeleteOnSubmit(detail);
                }

              
                // Xóa hóa đơn chính
                data.Bills.DeleteOnSubmit(bill);

                // Lưu các thay đổi vào cơ sở dữ liệu
                data.SubmitChanges();
            }
            // Trả về một phản hồi thành công
            return Json(new { success = true });
        }

    }
}