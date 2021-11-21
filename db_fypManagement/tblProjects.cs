using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblProjects
    {
        [Key]
        public int projectID { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Name Your Project Please"), MaxLength(30)]
        public string projectName { get; set; }
        public string projectGroup { get; set; }
 
        public string ProjectProposal { get; set; }
        public string approvalStatus { get; set; }
        public virtual ICollection<tblProjectRequests> TblProjectRequests { get; set; }
    }
}