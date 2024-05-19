using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MenuOrder.Controllers
{
    public class TrangMenuController : Controller
    {
        // GET: TrangMenu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QRTHANHTOAN()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LienHe(FormCollection f)
        {
            string customerEmail = f["email"];
            string customerName = f["name"];
            string customerMessage = f["message"];

            if (string.IsNullOrEmpty(customerEmail))
            {
                ViewBag.LienHe = "Email không thể để trống";
            }
            else if (string.IsNullOrEmpty(customerName))
            {
                ViewBag.LienHe = "Tên không thể để trống";
            }
            else if (string.IsNullOrEmpty(customerMessage))
            {
                ViewBag.LienHe = "Nội dung không thể để trống";
            }
            else
            {
                try
                {
                    SendCustomerEmail(customerEmail, customerName, customerMessage);
                    ViewBag.LienHe = "Đã gửi thành công";
                }
                catch (Exception ex)
                {
                    ViewBag.LienHe = "Đã xảy ra lỗi khi gửi email: " + ex.Message;
                }
            }
            return View();
        }

        public void SendCustomerEmail(string customerEmail, string customerName, string customerMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tam2003hkt@gmail.com", "uliw obgp dqnw eetq"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("tam2003hkt@gmail.com"),
                Subject = "Liên hệ của khách hàng",
                Body = $"Tên: {customerName}\nEmail: {customerEmail}\nNội dung: {customerMessage}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add("tam2003hkt@gmail.com");

            smtpClient.Send(mailMessage);
        }
    }
}