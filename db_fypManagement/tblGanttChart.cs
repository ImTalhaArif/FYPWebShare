using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblGanttChart
    {
        [Key]
        public int gcID { get; set; }
        public string gcGroup { get; set; }
     
        public string gcWeek { get; set; }
        public string gcName { get; set; }
    }
}