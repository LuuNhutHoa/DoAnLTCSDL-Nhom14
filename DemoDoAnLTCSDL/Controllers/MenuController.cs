using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDoAnLTCSDL.Models;
using System.Collections;
namespace DemoDoAnLTCSDL.Controllers
{
    public class MenuController : Controller
    {
        private DBPhoneEntities db = new DBPhoneEntities();
        // GET: Menu
        public ActionResult Index()
        {
            var loaisp = db.LoaiSPs.ToList();
            Hashtable arrLoaiSP = new Hashtable();
            foreach (var item in loaisp)
            {
                arrLoaiSP.Add(item.MaLoaiSP, item.TenLoaiSP);
            }
            ViewBag.LoaiSP = arrLoaiSP;
            return PartialView("Index");
        }
    }
}