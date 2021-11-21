using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblRequestStatus
    {
        [Key]
        public int rsID { get; set; }
        public string rsGroup { get; set; }
        public string rsSupervisor { get; set; }
        public string rsStatus { get; set; }

    }
}