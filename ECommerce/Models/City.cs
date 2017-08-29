using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class City
    {

        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        [Range(1, double.MaxValue, ErrorMessage="Select a department")]
        public int DepartmentsId { get; set; }


        public virtual Departments Department { get; set; }
    }
}