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


        public static List<City> GetCities()
        {

            //Extructure to show the departments ordered
            var cit = db.Cities.ToList();
            cit.Add(new City
            {
                CityId = 0,
                Name = "[ Select a city ]"
            });
            cit = cit.OrderBy(c => c.Name).ToList();

            return cit;
        }

        public static List<Company> GetCompanies()
        {

            //Extructure to show the departments ordered
            var comp = db.Companies.ToList();
            comp.Add(new Company
            {
                CompanyId = 0,
                Name = "[ Select a company ]"
            });
            comp = comp.OrderBy(c => c.Name).ToList();

            return comp;
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}