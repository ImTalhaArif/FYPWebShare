using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblProjectRequests
    {
        [Key]
        public int ProjectRequestID { get; set; }
        public string  Group { get; set; }

        public string Supervisor { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Project { get; set; }
      

        
        


    }
}