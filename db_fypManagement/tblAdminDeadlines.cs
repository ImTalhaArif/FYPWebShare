using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblAdminDeadlines
    {
        [Key]
        public int adID { get; set; }
        public string adTitle { get; set; }
        public DateTime adTime { get; set; }
        public string adDetails { get; set; }
        public string adReceiver { get; set; }
    }
}