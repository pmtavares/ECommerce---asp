using ECommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ECommerce.Classes
{
    public class UserHelper :IDisposable
    {

        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static EcommerceContext db = new EcommerceContext();


        //Delete users
        public static bool DeleteUser(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByEmail(email);

            if(userASP == null)
            {
                return false;
            }

            var response = userManager.Delete(userASP);

            return response.Succeeded;
        }

        //update users

        public static bool UpdateUser(string currentUser, string newUser)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByEmail(currentUser);

            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = newUser;
            userASP.Email = newUser;
            var response = userManager.Update(userASP);

            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            if(!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassword"];
            var userASP = userManager.FindByName(email);

            if(userASP == null)
            {
                CreateUserASP(email, "Admin");
                return;
            }

            userManager.AddToRole(userASP.Id, "Admin");
        }

        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email
                
            };

            userManager.Create(userASP, email);
            userManager.AddToRole(userASP.Id, roleName);
        }



        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if(userASP == null)
            {
                return;
            }

            var user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
            if(user ==null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}{1}{2:04}*",
                user.FirstName.Trim().ToUpper().Substring(0,1),
                user.LastName.Trim().ToLower(),
                random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "The Password was Recovered";
            var body = string.Format(@"
            <h1>Taxes Passsword Recovery</h1>
            <p> Your new Password is: <strong>{0}</strong></p>
            <p>Please change it as soon you can", newPassword);

            await MailHelper.SenMail(email, subject, body);


        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}