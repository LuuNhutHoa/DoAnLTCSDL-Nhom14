using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoDoAnLTCSDL.Models;
using System.Security.Cryptography;
using System.Data.Entity;
namespace DemoDoAnLTCSDL.Controllers
{
    public class FunctionLoginController : Controller
    {
        // GET: FunctionLogin
        public ActionResult Index()
        {
            return View();
        }
    }
}