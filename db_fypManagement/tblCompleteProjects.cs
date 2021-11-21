using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblCompleteProjects
    {
        [Key]
        public int cpID { get; set; }
        public string cpName { get; set; }
        public string cpGroup { get; set; }
        public DateTime cpSubmission { get; set; }
        public string cpGrade { get; set; }
        public string cpRemarks { get; set; }
        
    }
}