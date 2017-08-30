/*
 * Author: Pedro
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Departments
    {

        
        [Key]
        
        public int DepartmentsId { get; set; }

        [Required(ErrorMessage="The field {0} is required")]
        [MaxLength(50, ErrorMessage= "The field cant have more than 50 characters")]
        [Display(Name= "Department Name")]
        [Index("Department_Name_index", IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

    }
}