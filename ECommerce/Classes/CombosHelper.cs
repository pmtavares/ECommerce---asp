using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Classes
{
    public class CombosHelper : IDisposable
    {

        private static EcommerceContext db = new EcommerceContext();
        public static List<Departments> GetDepartments()
        {
            
            //Extructure to show the departments ordered
            var dep = db.Departments.ToList();
            dep.Add(new Departments
            {
                DepartmentsId = 0,
                Name = "[ Select a department ]"
            });
            dep = dep.OrderBy(d => d.Name).ToList();

            return dep;
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}