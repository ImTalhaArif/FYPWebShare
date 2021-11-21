using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblProjectProgress
    {
        [Key]
        public int ppID { get; set; }
        public string progressGroup { get; set; }
        public int projectProgress { get; set; }
       
    }
}