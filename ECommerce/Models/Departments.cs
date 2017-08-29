/*
 * Author: Pedro
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Departments
    {

        
        [Key]
        
        public int DepartmentsId { get; set; }

        [Required(ErrorMessage="The field {0} is required")]
        [Display(Name= "Department Name")]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

    }
}