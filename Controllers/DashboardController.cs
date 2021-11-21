using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class DashboardController : Controller
    {
        // Student Dashboard
        public ActionResult StudentDashboard()
        {
            if (Session["studentFname"] != null)
            {
                using (TableContext db = new TableContext())
                {
                    ViewData["AdminDeadline"] = db.tblAdminDeadlines.SqlQuery("SELECT * from tblAdminDeadlines Where adReceiver = '" + Session["studentGroup"] + "'   ").ToList();
                    ViewData["notifications"] = db.tblNotifications.SqlQuery("SELECT * FROM tblNotifications Where sendTo_studentID = '"+Session["studentID"]+"'").ToList();
                    ViewData["meetings"] = db.tblMeetings.SqlQuery("SELECT * FROM tblMeetings WHERE meetingGroup = '"+Session["studentGroup"]+"'").ToList();
                    ViewData["request"] = db.tblRequestMeetings.SqlQuery("SELECT * FROM tblRequestMeetings WHERE rmGroup = '" + Session["studentGroup"] + "'").ToList();
                    ViewData["tasks"] = db.tblGroupTasks.SqlQuery("SELECT * FROM tblGroupTasks WHERE Taskgroup = '"+Session["studentGroup"]+"'").ToList();
                    ViewData["Admin"] = db.tblAdminTasks.SqlQuery("SELECT * FROM tblAdminTasks WHERE [Group] = '"+Session["studentGroup"]+"'").ToList();
                    ViewData["supervision"] = db.tblRequestStatus.SqlQuery("SELECT * FROM tblRequestStatus WHERE rsGroup = '"+Session["studentGroup"]+"'").ToList();
                    ViewData["mytask"] = db.tblTaskNotification.SqlQuery("SELECT * FROM tblTaskNotifications WHERE tnStudent = '" + Session["studentEmail"]+"'").ToList();
                }
                return View();
            }
            return RedirectToAction("StdLogin", "Login");
        }
        [HttpPost]
        public ActionResult StudentDashboard( string remove, string desc, string readadmin)
        {
            if(readadmin != null)
            {
                using (TableContext db = new TableContext())
                {
                    var done = "DELETE from tblAdminDeadlines WHERE adReceiver = '"+readadmin+"'";
                    db.Database.ExecuteSqlCommand(done);
                    return RedirectToAction("StudentDashboard");
                }
            }

            if(desc != null)
            {
                using (TableContext db = new TableContext())
                {
                    var errupt = "DELETE FROM tblTaskNotifications WHERE tnName = '"+desc+"'";
                    db.Database.ExecuteSqlCommand(errupt);
                    return RedirectToAction("GroupDashboard","Groups");
                }
            }
            using (TableContext db = new TableContext())
            {
                var alter = "DELETE FROM tblRequestStatus WHERE rsGroup = '" + remove + "'";
                db.Database.ExecuteSqlCommand(alter);
                return RedirectToAction("StudentDashboard");
            }
            return View();
        }


        public ActionResult MemberDashboard()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["notifications"] = db.tblNotifications.SqlQuery("SELECT * FROM tblNotifications Where sendTo_studentID = '" + Session["studentID"] + "'").ToList();
                ViewData["meetings"] = db.tblMeetings.SqlQuery("SELECT * FROM tblMeetings WHERE meetingGroup = '" + Session["studentGroup"] + "'").ToList();
                ViewData["tasks"] = db.tblGroupTasks.SqlQuery("SELECT * FROM tblGroupTasks WHERE AssignedTo = '" + Session["studentEmail"] + "'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult MemberDashboard(tblNotifications not)
        {
            return RedirectToAction("AcceptInvite", "Groups");
        }

        //The supervisor Dashboard
        public ActionResult SupervisorDashboard()
        {
            if (Session["supervisorName"] != null)
            {
                using (TableContext db = new TableContext())
                {
                    ViewData["AdminDeadline"] = db.tblAdminDeadlines.SqlQuery("SELECT * from tblAdminDeadlines Where adReceiver = '" + Session["supervisorName"] + "'   ").ToList();
                    ViewData["request"] = db.tblProjectRequests.SqlQuery("SELECT * FROM tblProjectRequests WHERE Supervisor = '"+Session["supervisorName"]+"'").ToList();
                    ViewData["mygroups"] = db.tblGroups.SqlQuery("Select * from tblGroups where groupSupervisor = '" + Session["supervisorName"] + "'").ToList();
                    ViewData["submitted"] = db.tblsubmittedtasks.SqlQuery("SELECT * FROM tblsubmittedtasks Where stSupervisor = '"+Session["supervisorName"]+"'").ToList();
                    return View();
                }
            }
            return RedirectToAction("SupervisorLogin","Login");
        }
        [HttpPost]
        public ActionResult SupervisorDashboard( string more, string desc, string readadmin)
        {
            if (readadmin != null)
            {
                using (TableContext db = new TableContext())
                {
                    var done = "DELETE from tblAdminDeadlines WHERE adReceiver = '" + Session["supervisorName"] + "'";
                    db.Database.ExecuteSqlCommand(done);
                    return RedirectToAction("SupervisorDashboard");
                }
            }

            if (more != null)
            {
                TempData["group"] = more;
                return RedirectToAction("ViewMore","Supervisor");
            }

            using (TableContext db = new TableContext())
            {
                var del = "DELETE from tblsubmittedtasks WHERE stName = '" + desc + "'";
                db.Database.ExecuteSqlCommand(del);
            }

            if(desc != null)
            {
                return RedirectToAction("SupervisionRequest","Supervisor");
            }



            return RedirectToAction("GroupTasks", "Supervisor");

        }

        //Admin Dashboard
        public ActionResult AdminDashboard()
        {
            if(Session["adminName"] != null)
            {
                using (TableContext db = new TableContext())
                {
                    ViewData["Group"] = db.tblGroups.SqlQuery("SELECT * from tblGroups").ToList();
                    ViewData["progress"] = db.tblProjectProgresses.SqlQuery("SELECT * FROM tblProjectProgresses").ToList();
                    ViewData["supervisor"] = db.tblSupervisors.SqlQuery("SELECT * FROM tblSupervisors").ToList();
                    ViewData["ganttchart"] = db.tblGanttCharts.SqlQuery("SELECT * FROM tblGanttCharts").ToList();
                    ViewData["pendingTask"] = db.tblGroupTasks.SqlQuery("SELECT * FROM tblGroupTasks WHERE gtStatus = 'pending'").ToList();
                    ViewData["completeTask"] = db.tblGroupTasks.SqlQuery("SELECT * FROM tblGroupTasks WHERE gtStatus = 'Completed'").ToList();

                }

                return View();
            }
            return RedirectToAction("AdminLogin","Login");
        }
        [HttpPost]
        public ActionResult AdminDashboard(string supervisor)
        {
            TempData["supervisorDetails"] = supervisor;
            return RedirectToAction("SupervisorDetails", "Admin");
        }

        public ActionResult LeaderProfile()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["mydetails"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentID = '"+Session["studentID"]+"'").ToList();
            }

            return View();
        }
        [HttpPost]
        public ActionResult LeaderProfile(string firstname, string lastname, string fathername, string email, string password, DateTime dob)
        {
            using (TableContext db = new TableContext())
            {
                var profile = "UPDATE tblStudents set studentFname = '" + firstname + "' , studentLname = '" + lastname + "', studentPname = '" + fathername + "', studentEmail = '" + email + "', studentPassword = '" + password + "', studentDOB = '" + dob + "' where studentID = '" + Session["studentID"] + "'";
                db.Database.ExecuteSqlCommand(profile);
            }
            return RedirectToAction("StudentDashboard");
        }
        public ActionResult MemberProfile()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["mydetails"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentID = '" + Session["studentID"] + "'").ToList();
            }

            return View();
        }
        [HttpPost]
        public ActionResult MemberProfile(string firstname, string lastname, string fathername, string email, string password, DateTime dob)
        {
            using (TableContext db = new TableContext())
            {
                var profile = "UPDATE tblStudents set studentFname = '" + firstname + "' , studentLname = '" + lastname + "', studentPname = '" + fathername + "', studentEmail = '" + email + "', studentPassword = '" + password + "', studentDOB = '" + dob + "' where studentID = '" + Session["studentID"] + "'";
                db.Database.ExecuteSqlCommand(profile);
            }
            return RedirectToAction("MemberDashboard");
        }
    }
}