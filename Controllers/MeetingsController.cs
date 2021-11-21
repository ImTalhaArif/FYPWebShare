using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class MeetingsController : Controller
    {
        // GET: Meetings
        public ActionResult RequestMeeting()
        {
            if ((string)Session["isLeader"] == "false")
            {

                return RedirectToAction("GroupDashboard","Groups");
            }
                using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups where groupName = '"+Session["studentGroup"]+"'").ToList();
            }
                return View();
        }
        [HttpPost]
        public ActionResult RequestMeeting(DateTime meetingDate, string group, string supervisor)
        {
            using (TableContext db = new TableContext())
            {
                var meeting ="INSERT INTO tblRequestMeetings (rmDateTime, rmGroup, MSupervisor) VALUES ('"+meetingDate+"','"+group+"','"+supervisor+"')";
                db.Database.ExecuteSqlCommand(meeting);
            }
            return RedirectToAction("GroupDashboard","Groups");
        }

      
    }
}