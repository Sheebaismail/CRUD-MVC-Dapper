using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrudDapper.Models
{
    public class EmpModel
    {

        public int EmpId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please select one.")]


        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please select the language.")]

        public string Language { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile Number")]
        [StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        public string Phone { get; set; }
        public int Status { get; internal set; }


    }
}