using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DemoDoAnLTCSDL.Models;
[assembly: OwinStartupAttribute(typeof(DemoDoAnLTCSDL.Startup))]
namespace DemoDoAnLTCSDL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //InitUserRole();
        }
        private void InitUserRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //tạo role Admin
            if(!roleManger.RoleExists("Admin"))// chưa có mới tạo
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManger.Create(role);
                //tạo user
                var user = new ApplicationUser();
                user.UserName = "1954052035hoa@ou.edu.vn";
                user.Email = "1954052035hoa@ou.edu.vn";
                //string pass = "123456";//sẽ đổi pass sau
                var check = userManger.Create(user, "123456");
                // đưa user Quản lí điện thoại vào role Admin
                if (check.Succeeded)
                    userManger.AddToRole(user.Id, "Admin");
                user = new ApplicationUser();
                user.UserName = "lnhoa2103@gmail.com";
                user.Email = "lnhoa2103@gmail.com";
                //string pass = "123456";//sẽ đổi pass sau
                check = userManger.Create(user, "123456");
                // đưa user Quản lí điện thoại vào role Admin
                if (check.Succeeded)
                    userManger.AddToRole(user.Id, "WebMaster");
            }    
        }
    }
}
