using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblAdmin
    {
        [Key]
        public int adminID { get; set; }
        public string adminName { get; set; }
        public string adminEmail { get; set; }
        public string adminPassword { get; set; }

    }
}