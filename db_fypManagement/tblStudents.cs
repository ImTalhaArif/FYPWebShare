using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblStudents
    {
        [Key]
        public int studentID { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "What should we call you?"), MaxLength(30)]
        public string studentFname { get; set; }
        [Display(Name = "Last Name")]
       [Required(ErrorMessage = "And your Last name?"), MaxLength(30)]
        public string studentLname { get; set; }
        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Please enter your Father's name"), MaxLength(30)]
        public string studentPname { get; set; }
        [Display(Name = "Email Address")]
       [Required(ErrorMessage = "Email Address is required"), MaxLength(30)]
        public string studentEmail { get; set; }
        [Display(Name = "Set a Password")]
   
       [Required(ErrorMessage = "Password????"), MaxLength(30)]
        public string studentPassword { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "When is your Bithday?")]
        public DateTime studentDOB { get; set; }
      [HiddenInput(DisplayValue = false)]
        public string isLeader { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string studentGroup { get; set; }

        public virtual ICollection<tblGroup> TblGroup { get; set; }
        public virtual ICollection<tblGroupTasks> TblGroupTasks { get; set; }
        public virtual ICollection<tblNotifications> TblNotifications { get; set; }

        // public virtual ICollection<tblNotifications> TblNotifications { get; set; }





    }
}