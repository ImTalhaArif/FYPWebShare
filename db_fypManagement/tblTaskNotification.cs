using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblTaskNotification
    {
        [Key]
        public int tnID { get; set; }
        public string tnName { get; set; }
        public string tnStudent { get; set; }
    }
}