using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class SupervisorController : Controller
    {
        // GET: Supervisor
        public ActionResult SupervisionRequest()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["request"] = db.tblProjectRequests.SqlQuery("select * from tblProjectRequests WHERE Supervisor = '"+Session["supervisorName"]+"'").ToList();
                ViewData["project"] = db.tblProjects.SqlQuery("SELECT * from tblProjects ").ToList();
                // ViewData["project"] = db.tblProjects.SqlQuery("").ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult SupervisionRequest(string join, string details)
        {
            if (details != null)
            {
                TempData["detail"] = details;
                return RedirectToAction("RequestDetails");
            }
            using (TableContext db = new TableContext())
            {
                var ab = "Update tblGroups set tblSupervisors_supervisorID = '" + Session["supervisorID"] + "' where groupName = '" + join + "'";
                db.Database.ExecuteSqlCommand(ab);
                var grp = "Update tblGroups set groupSupervisor = '" + Session["supervisorName"] + "' where groupName = '" + join + "' ";
                db.Database.ExecuteSqlCommand(grp);
                var deletereq = "DELETE FROM tblProjectRequests WHERE [Group] = '" + join + "'";
                db.Database.ExecuteSqlCommand(deletereq);

                return RedirectToAction("SupervisorDashboard", "Dashboard");


            }
            //  return View();
        }
        public ActionResult RequestDetails()
        {
            string names = "abc";
            if (TempData.ContainsKey("detail"))
                names = TempData["detail"].ToString();
            using (TableContext db = new TableContext())
            {
                ViewData["groupdetails"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '" + names + "'").ToList();
                ViewData["project"] = db.tblProjects.SqlQuery("SELECT * FROM tblProjects WHERE projectGroup = '" + names + "'").ToList();
                ViewData["members"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentGroup = '" + names + "'").ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult RequestDetails(string join, string deny)
        {
            if (deny != null)
            {
                using (TableContext db = new TableContext())
                {
                    var del = "DELETE FROM tblProjectRequests WHERE [Group] = '"+deny+"'";
                    db.Database.ExecuteSqlCommand(del);
                    var not = "INSERT INTO tblRequestStatus (rsGroup, rsSupervisor, rsStatus) VALUES ('"+deny+"', '"+Session["supervisorName"]+"', 'Declined')";
                    db.Database.ExecuteSqlCommand(not);
                    return RedirectToAction("SupervisorDashboard","Dashboard");
                }


                }
            else if (join != null)
            { 
                using (TableContext db = new TableContext())
                {
                    var ab = "Update tblGroups set tblSupervisors_supervisorID = '" + Session["supervisorID"] + "' where groupName = '" + join + "'";
                    db.Database.ExecuteSqlCommand(ab);
                    var grp = "Update tblGroups set groupSupervisor = '" + Session["supervisorName"] + "' where groupName = '" + join + "' ";
                    db.Database.ExecuteSqlCommand(grp);
                    var deletereq = "DELETE FROM tblProjectRequests WHERE [Group] = '" + join + "'";
                    var not = "INSERT INTO tblRequestStatus (rsGroup, rsSupervisor, rsStatus) VALUES ('" + join + "', '" + Session["supervisorName"] + "', 'Approved')";
                    db.Database.ExecuteSqlCommand(not);
                    db.Database.ExecuteSqlCommand(deletereq);

                    return RedirectToAction("SupervisorDashboard", "Dashboard");
                }
            }
            return View();
        }
        public ActionResult Projects()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["MyProjects"] = db.tblSupervisorProjects.SqlQuery("SELECT * FROM tblSupervisorProjects WHERE supervisor_supervisorID = '" + Session["supervisorID"] + "'").ToList();
            }
            return View();

        }
        [HttpPost]
        public ActionResult Projects(string abc)
        {
            using (TableContext db = new TableContext())
            {
                var delete = "DELETE FROM tblSupervisorProjects WHERE spName = '" + abc + "'";
                db.Database.ExecuteSqlCommand(delete);
            }
            return RedirectToAction("SupervisorProjects", "Project");
        }

        public ActionResult Mygroups()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["mygroups"] = db.tblGroups.SqlQuery("Select * from tblGroups where groupSupervisor = '" + Session["supervisorName"] + "'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Mygroups(string more)
        {
            TempData["group"] = more;
            return RedirectToAction("ViewMore");
        }
        public ActionResult ViewMore()
        {
            using (TableContext db = new TableContext())
            {
                string names = "abc";

                if (TempData.ContainsKey("group"))
                    names = TempData["group"].ToString();
                // names = "abc";
                ViewData["gantt"] = db.tblGanttCharts.SqlQuery("SELECT * FROM tblGanttCharts WHERE gcGroup = '" + names + "'").ToList();
                ViewData["logchart"] = db.tblLogCharts.SqlQuery("SELECT * FROM tblLogCharts where lcGroup = '" + names + "'").ToList();
                ViewData["single"] = db.tblGroups.SqlQuery("SELECT * from tblGroups where groupName = '" + names + "' ").ToList();
                ViewData["grouptask"] = db.tblGroupTasks.SqlQuery("SELECT * from tblGroupTasks where Taskgroup = '"+names+"'").ToList();
                ViewData["progress"] = db.tblProjectProgresses.SqlQuery("SELECT * from tblProjectProgresses where progressGroup = '"+names+"'").ToList();
                ViewData["members"] = db.tblStudents.SqlQuery("SELECT * from tblStudents where studentGroup = '" + names + "' ").ToList();
                ViewData["project"] = db.tblProjects.SqlQuery("SELECT * from tblProjects where projectGroup = '" + names + "'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewMore(string student, string group)
        {
            if(group != null)
            {
                TempData["group"] = group;
                return RedirectToAction("singleGroup");
            }
            TempData["mystudent"] = student;
            return RedirectToAction("NewTask");

            //   using (TableContext db = new TableContext())
            //   {
            //       var insert = "Insert into tblGroupTasks (gtDetails, gtStatus, gtCreatedOn, gtDeadline, AssignedTo, Taskgroup) VALUES ('" + details + "', 'pending', GETDATE(), '" + deadline + "', '" + taskto + "', '" + group + "')";
            //       db.Database.ExecuteSqlCommand(insert);
            //       return RedirectToAction("Mygroups");
            //   }
            // return View();
        }
    
        public ActionResult NewTask()
        {

            if (TempData.ContainsKey("mystudent"))
            {
                string mystud = TempData["mystudent"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["stdetails"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentEmail = '" + mystud + "'").ToList();
                    return View();
                }
                //return View();
            }

            return View();

        }
        [HttpPost]
        public ActionResult NewTask(string title, string details, DateTime deadline, string student, string group, HttpPostedFileBase file )
        {
            string path = Path.Combine(Server.MapPath("~/SupervisorTaskFile/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);
            using (TableContext db = new TableContext())
            {
                 {
                     var insert = "Insert into tblGroupTasks (gtDetails, gtStatus, gtCreatedOn, gtDeadline, AssignedTo, Taskgroup, gtTaskfile, gtprogress) VALUES ('" + details + "', 'pending', GETDATE(), '" + deadline + "', '" + student + "', '" + group + "','" + file.FileName + "','10')";
                     db.Database.ExecuteSqlCommand(insert);
                    var notify = "INSERT INTO tblTaskNotifications (tnName, tnStudent) VALUES ('"+title+"', '"+student+"')";
                    db.Database.ExecuteSqlCommand(notify);
                     return RedirectToAction("Mygroups");
                 }
                //return View();
            }
            //return View();

        }

        //This Action Result is For the Approval Of Meetings Requested by a Certain group that is supervised by the Supervisor
        public ActionResult RequestedMeetings()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["meetings"] = db.tblRequestMeetings.SqlQuery("SELECT * FROM tblRequestMeetings WHERE MSupervisor = '" + Session["supervisorName"] + "'").ToList();
            }
                    
                    return View();
        }
        [HttpPost]
        public ActionResult RequestedMeetings(string Approve, string group, DateTime date, string zoom)
        {
            using (TableContext db = new TableContext())
            {
                var meeting = "INSERT INTO tblMeetings (mDatenTime,meetingLink, meetingGroup, meetingSupervisor) VALUES ('"+date+"', '"+zoom+"', '"+group+"', '"+ Session["supervisorName"] +"')";
                db.Database.ExecuteSqlCommand(meeting);
                var deleterequest = "DELETE FROM tblRequestMeetings where rmGroup = '" + group + "'";
                db.Database.ExecuteSqlCommand(deleterequest);
            }
            return RedirectToAction("Mygroups","Supervisor");
        }


        public ActionResult GroupTasks()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["mygroups"] = db.tblGroups.SqlQuery("SELECT * from tblGroups where groupSupervisor = '"+Session["supervisorName"]+"' ").ToList();
            }
                    return View();
        }
        [HttpPost]
        public ActionResult GroupTasks(string group)
        {
            TempData["group"] = group;
            return RedirectToAction("singleGroup");
        }
        public ActionResult singleGroup()
        {
            string names = "abc";

            if (TempData.ContainsKey("group"))
                names = TempData["group"].ToString();
            using (TableContext db = new TableContext())
            {
                ViewData["select"] = db.tblGroupTasks.SqlQuery("SELECT * from tblGroupTasks where TaskGroup = '" + names + "'").ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult singleGroup(string remarks, string group)
        {
            using (TableContext db = new TableContext())
            {

                var Rem = db.Database.ExecuteSqlCommand("update tblGroupTasks set TaskRemarks = '"+remarks+"' WHERE AssignedTo = '"+group+"' ");
            
            }
            return RedirectToAction("GroupTasks");
        }
        public ActionResult ApprovedMeetings()
        {
            using (TableContext db =new TableContext())
            {
                ViewData["approved"] = db.tblMeetings.SqlQuery("SELECT * FROM tblMeetings where meetingSupervisor = '"+Session["supervisorName"]+"'").ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult ApprovedMeetings(int meet)
        {
            using (TableContext db = new TableContext())
            {
                var delete = "Delete FROM tblMeetings where meetingID = '"+meet+"' ";
                db.Database.ExecuteSqlCommand(delete);
                
                return RedirectToAction("ApprovedMeetings");
            }
            //return View();
        }
        public ActionResult EditProfile()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["myprofile"] = db.tblSupervisors.SqlQuery("SELECT * FROM tblSupervisors where supervisorName = '"+Session["supervisorName"]+"'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(string yourname, string domain, string email, string password)
        {
            using (TableContext db = new TableContext())
            {
                var update = "UPDATE tblSupervisors SET supervisorName = '"+yourname+"', supervisorEmail = '"+email+"', supervisorDomain = '"+domain+"', supervisorPassword = '"+password+"' WHERE supervisorName = '"+Session["supervisorName"]+"'" ;
                db.Database.ExecuteSqlCommand(update);
            }
            return RedirectToAction("SupervisorDashboard","Dashboard");
        }
    }
}