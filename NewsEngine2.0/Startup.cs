using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using NewsEngine2._0.Models;
using Owin;
using System.Data;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(NewsEngine2._0.Startup))]
namespace NewsEngine2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminUserAndApplicationRoles();
            mediaTypes();
        }

        private void mediaTypes()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var image = db.MediaTypes.Where(x => x.Name.Contains("Image")).FirstOrDefault();
            var video = db.MediaTypes.Where(x => x.Name.Contains("Video")).FirstOrDefault();

            if (image == null)
            {
                MediaType imageType = new MediaType
                {
                    Name = "Image"
                };
                db.MediaTypes.Add(imageType);
                db.SaveChanges();
            }
            if(video == null)
            {
                MediaType videoType = new MediaType
                {
                    Name = "Video"
                };
                db.MediaTypes.Add(videoType);
                db.SaveChanges();
            }
        }
        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei             
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator                 
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

                // se adauga utilizatorul administrator                 
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com"; 

                var adminCreated = UserManager.Create(user, "Administrator1!");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");
                }
            } 
 
            if (!roleManager.RoleExists("+" +
                ""))            
            {                 
                var role = new IdentityRole();
                role.Name = "Editor";                 
                roleManager.Create(role);             
            } 
 
            if (!roleManager.RoleExists("User"))             
            {                 
                 var role = new IdentityRole();
                role.Name = "User";                 
                roleManager.Create(role);             
            } 
 
      } 
    }
}
