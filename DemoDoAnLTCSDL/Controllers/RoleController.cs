using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DemoDoAnLTCSDL.Models;
using System.Net;
using System.Threading.Tasks;
namespace DemoDoAnLTCSDL.Controllers
{
    [Authorize(Roles = "Admin")]//chỉ cho các user thuộc Role Admin truy cập vào Controller
    public class RoleController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Role
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            return View(roleManager.Roles);
        }

        //GET: Role/Details/5

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var role = await roleManager.FindByIdAsync(id);
            var listUser = new List<ApplicationUser>();
            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user.Id, role.Name))
                    listUser.Add(user);
            }
            ViewBag.Users = listUser;
            ViewBag.UserCount = listUser.Count();
            return View(role);
        }
        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if(ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
                var check = await roleManager.CreateAsync(role);
                if(!check.Succeeded)
                {
                    ModelState.AddModelError("", check.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return HttpNotFound();
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel roleViewModel )
        {
            if(ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>
                  (new RoleStore<IdentityRole>(context));
                var role = await roleManager.FindByIdAsync(roleViewModel.Id );
                role.Name = roleViewModel.Name;
                await roleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Role/Delete/5
        public async  Task<ActionResult> Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return HttpNotFound();
            return View();
        }

        // POST: Role/Delete/5
        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));
                var role = await roleManager.FindByIdAsync(id);
                if (role == null)
                    return HttpNotFound();
                var check = await roleManager.DeleteAsync(role);
                if (!check.Succeeded)
                {
                    ModelState.AddModelError("", check.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
