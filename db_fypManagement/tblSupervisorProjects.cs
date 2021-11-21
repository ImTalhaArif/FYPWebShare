using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblSupervisorProjects
    {
        [Key]
        public int spID { get; set; }
        public string spName { get; set;}
        public string spFile { get; set; }
        public DateTime spDate { get; set; }
        public virtual tblSupervisors supervisor { get; set; }
        public string IsSelected { get; set; }


    }
}