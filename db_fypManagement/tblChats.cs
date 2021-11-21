using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public class tblChats
    {
        [Key]
        public int chatID { get; set; }
        public string senderName { get; set; }
        public string message { get; set; }
        public string receiverName { get; set; }
        public DateTime timesent { get; set; }
    }
}