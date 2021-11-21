using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblRequestMeeting
    {
        [Key]
        public int requestMettingID { get; set; }
        public string rmGroup { get; set; }
        public string MSupervisor { get; set; }
        public DateTime rmDateTime { get; set; }

    }
}