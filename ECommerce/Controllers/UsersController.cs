using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Classes;

namespace ECommerce.Controllers
{
    public class UsersController : Controller
    {
        private EcommerceContext db = new EcommerceContext();


        //Function to list cities in cascade
        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(m => m.DepartmentsId == departmentId);
            return Json(cities);
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Cities).Include(u => u.Companies).Include(u => u.Departments);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name");
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name");
            ViewBag.DepartmentsId = new SelectList(CombosHelper.GetDepartments(), "DepartmentsId", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();

                UserHelper.CreateUserASP(user.UserName, "User");

                var pic = string.Empty;
                var folder = "~/Content/users";

                var file = string.Format("{0}.jpg", user.UserId);

                if (user.PhotoFile != null)
                {
                    var response = FilesHelper.uploadPhoto(user.PhotoFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        user.Photo = pic;
                        db.Entry(user).State = EntityState.Modified; //modified. To save on DB
                        db.SaveChanges();

                    }


                }
               
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentsId = new SelectList(CombosHelper.GetDepartments(), "DepartmentsId", "Name", user.DepartmentsId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentsId = new SelectList(CombosHelper.GetDepartments(), "DepartmentsId", "Name", user.DepartmentsId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {

                var pic = string.Empty;
                var folder = "~/Content/users";

                var file = string.Format("{0}.jpg", user.UserId);

                if (user.PhotoFile != null)
                {
                    var response = FilesHelper.uploadPhoto(user.PhotoFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        user.Photo = pic;
                        

                    }


                }

                var db2 = new EcommerceContext();
                var currentUser = db2.Users.Find(user.UserId);

                if(currentUser.UserName != user.UserName)
                {
                    UserHelper.UpdateUser(currentUser.UserName, user.UserName);
                }

                db2.Dispose();

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentsId = new SelectList(CombosHelper.GetDepartments(), "DepartmentsId", "Name", user.DepartmentsId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            UserHelper.DeleteUser(user.UserName);


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
