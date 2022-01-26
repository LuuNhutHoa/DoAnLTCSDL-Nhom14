using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using DemoDoAnLTCSDL.Models;
namespace QuanLyBanHang.Controllers
{
    public class GioHangController : Controller
    {
        private DBPhoneEntities db = new DBPhoneEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }
        public RedirectToRouteResult AddToCart(string MaSP)
        {
            if (Session["giohang"] == null) //chưa có giỏ hàng
            {//Tạo
                Session["giohang"] = new List<CartItem>();
            }
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            if (giohang.FirstOrDefault(m => m.MaSP == MaSP) == null)//sản phẩm chưa có trong giỏ hàng
            {
                SanPham sp = db.SanPhams.Find(MaSP);
                CartItem item = new CartItem();
                item.MaSP = MaSP;
                item.TenSP = sp.TenSP;
                item.DonGia = Convert.ToDouble(sp.Dongia);
                item.SoLuong = 1;
                giohang.Add(item);
            }
            else //sản phẩm đã có tong giỏ hàng => tăng số lương + 1 
            {
                CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
                item.SoLuong++;
            }
            Session["giohang"] = giohang;
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Update(string MaSP, int txtSoluong)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                item.SoLuong = txtSoluong;
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Delete(string MaSP, int txtSoluong)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaSP == MaSP);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Order(string Email, string Phone)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            string sMsg = "<html><body><table><caption>Thông tin đặt hàng</caption>";
            sMsg += "<tr><th>STT</th><th>Tên hàng</th><th>Số lượng</th><th>Đơn giá</th><th>Thành Tiền</th></tr>";
            int i = 0;
            double tongtien = 0;
            foreach (var item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.SoLuong + "</td>";
                sMsg += "<td>" + item.DonGia.ToString("#,###") + "</td>";
                sMsg += "<td>" + item.ThanhTien.ToString("#,###") + "</td>";
                sMsg += "</tr>";
                tongtien += item.ThanhTien;
            }
            sMsg += "<tr><th colspan ='5'>Tổng cộng" + tongtien.ToString("#,###") + "</th></tr>";
            sMsg += "</table></body></html>";
            try
            {
                MailMessage mail = new MailMessage("lnhoa2103@gmail.com", Email, "Thông tin đặt hàng", sMsg);
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("lnhoa2103", "nhuthoa2103");
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                //ViewBag.SendMail = "Đặt hàng thành công";
                //ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}