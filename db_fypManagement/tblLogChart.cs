using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblLogChart
    {
        [Key]
        public int lcID { get; set; }
        public string lcGroup { get; set; }
        public string lcDetail { get; set; }
        public DateTime lcDate { get; set; }
        public string lcCreatedby { get; set; }

    }
}