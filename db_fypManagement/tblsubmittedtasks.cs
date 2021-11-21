using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblsubmittedtasks
    {
        [Key]
        public int stID { get; set; }
        public string stName { get; set; }
        public string stSupervisor { get; set; }
       
    }
}