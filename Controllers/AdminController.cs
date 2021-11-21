using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Supervisors()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisors"] = db.tblSupervisors.SqlQuery("SELECT * FROM tblSupervisors").ToList();



            } return View();
        }
        
        [HttpPost]
        public ActionResult Supervisors(string name, string sname, string email, string password, string domain, string desc, string details)
        {

            if(details != null)
            {
                TempData["supervisorDetails"] = details;
                return RedirectToAction("SupervisorDetails");
            }

            if(desc != null)
            {
                using (TableContext db = new TableContext())
                {
                    var addsp = "INSERT INTO tblSupervisors (supervisorName, supervisorEmail, supervisorPassword, supervisorDomain) VALUES ('"+sname+ "','" + email + "','" + password + "','" + domain + "')";
                    db.Database.ExecuteSqlCommand(addsp);
                    return RedirectToAction("Supervisors");
                }
            }



            using (TableContext db = new TableContext())
            {
                var remove = "DELETE FROM tblSupervisors WHERE supervisorEmail = '" + name + "'";
                db.Database.ExecuteSqlCommand(remove);
            }
            return RedirectToAction("Supervisors");
        }
        public ActionResult adminProfile()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["myprofile"] = db.tblAdmins.SqlQuery("SELECT * FROM tblAdmins WHERE adminEmail = '"+Session["adminEmail"]+"'").ToList();
 }
            return View();
        }
        [HttpPost]
        public ActionResult adminProfile(string name, string email, string password)
        {
            using(TableContext db = new TableContext())
            {
                var update = "UPDATE tblAdmins SET adminName = '"+name+"', adminEmail = '"+email+"', adminPassword = '"+password+"'";
                db.Database.ExecuteSqlCommand(update);
                return RedirectToAction("adminProfile");

            }
        }

        public ActionResult SupervisorDetails()
        {
            if(TempData.ContainsKey("supervisorDetails"))
            {
               string supervisorName = TempData["supervisorDetails"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["details"] = db.tblSupervisors.SqlQuery("SELECT * FROM tblSupervisors Where supervisorName = '"+supervisorName+"'").ToList();
                    ViewData["groups"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups Where groupSupervisor = '"+supervisorName+"'").ToList();

                }
            }
            return View();
        }



        public ActionResult Groups()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["Groups"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups").ToList();



            }

            return View();
        }
        [HttpPost]
        public ActionResult Groups(string group, string detail)
        {
            if(detail != null)
            {
                TempData["grpdetail"] = detail;
                return RedirectToAction("GroupDetails");
            }

            using (TableContext db = new TableContext())
            {
                var terminate = "DELETE FROM tblGroups Where groupName = '" + group + "'";
                db.Database.ExecuteSqlCommand(terminate);
            }
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        public ActionResult GroupDetails()
        {
            if(TempData.ContainsKey("grpdetail"))
            {
                string grpname = TempData["grpdetail"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["gantt"] = db.tblGanttCharts.SqlQuery("SELECT * FROM tblGanttCharts WHERE gcGroup = '" + grpname + "'").ToList();
                    ViewData["logchart"] = db.tblLogCharts.SqlQuery("SELECT * FROM tblLogCharts where lcGroup = '" + grpname + "'").ToList();
                    ViewData["single"] = db.tblGroups.SqlQuery("SELECT * from tblGroups where groupName = '" + grpname + "' ").ToList();
                    ViewData["grouptask"] = db.tblGroupTasks.SqlQuery("SELECT * from tblGroupTasks where Taskgroup = '" + grpname + "'").ToList();
                    ViewData["progress"] = db.tblProjectProgresses.SqlQuery("SELECT * from tblProjectProgresses where progressGroup = '" + grpname + "'").ToList();
                    ViewData["members"] = db.tblStudents.SqlQuery("SELECT * from tblStudents where studentGroup = '" + grpname + "' ").ToList();
                    ViewData["project"] = db.tblProjects.SqlQuery("SELECT * from tblProjects where projectGroup = '" + grpname + "'").ToList();

                }
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult GroupDetails(string student)
        {
            using (TableContext db = new TableContext())
            {
                var delete = "UPDATE tblStudents SET studentGroup = 'none' WHERE studentEmail = '"+student+"'";
                db.Database.ExecuteSqlCommand(delete);
                return RedirectToAction("Groups");
            
            }

        }


        public ActionResult Projects()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["projects"] = db.tblProjectRequests.SqlQuery("SELECT * FROM tblProjectRequests").ToList();
              

            }
            return View();
        }
        [HttpPost]
        public ActionResult Projects(string delete, string accept, string supervisor)
        {
            if (accept != null)
            {
                using (TableContext db = new TableContext())
                {
                    var accepted = "UPDATE tblGroups set groupSupervisor = '"+supervisor+"' WHERE groupProject = '"+accept+"'";
                    db.Database.ExecuteSqlCommand(accepted);
                    var deleted = "DELETE FROM tblProjectRequests WHERE project = '"+accept+"'";
                    db.Database.ExecuteSqlCommand(deleted);
                    return RedirectToAction("Projects");
                }
            }
            using (TableContext db = new TableContext())
            {
                var remove = "DELETE FROM tblProjectRequests WHERE project = '" + delete + "'";
                db.Database.ExecuteSqlCommand(remove);
            }
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        public ActionResult Deadlines()
        {

            return View();

        }
        [HttpPost]
        public ActionResult Deadlines(string title, string details, DateTime date, string receiver)
        {
            using (TableContext db = new TableContext())
            {
                if (receiver == "supervisor")
                {
                    ViewData["supervisor"] = db.tblSupervisors.SqlQuery("SELECT * from tblSupervisors").ToList();
                    foreach (var sr in ViewData["supervisor"] as IEnumerable<TheFinalProduct_FYP_.db_fypManagement.tblSupervisors>)
                    {
                        var deadline = "INSERT INTO tblAdminDeadlines (adTitle, adTime, adDetails, adReceiver) VALUES ('" + title + "','" + date + "','" + details + "','" + sr.supervisorName + "')";
                        db.Database.ExecuteSqlCommand(deadline);
                    }
                    return RedirectToAction("deadlines");
                }
                if (receiver == "group")
                {
                    ViewData["group"] = db.tblGroups.SqlQuery("SELECT * from tblGroups").ToList();
                    foreach (var sr in ViewData["group"] as IEnumerable<TheFinalProduct_FYP_.db_fypManagement.tblGroup>)
                    {
                        var deadline = "INSERT INTO tblAdminDeadlines (adTitle, adTime, adDetails, adReceiver) VALUES ('" + title + "','" + date + "','" + details + "','" + sr.groupName + "')";
                        db.Database.ExecuteSqlCommand(deadline);
                    }
                    return RedirectToAction("deadlines");
                }
            }
            return View();

        }

        public ActionResult AssignGroupTask()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["Groups"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups").ToList();


            }
            return View();
        }
        [HttpPost]
        public ActionResult AssignGroupTask(string groupName)
        {
            TempData["name"] = groupName;
            return RedirectToAction("SendAdminTask");


            // using (TableContext db = new TableContext())
            // {
            //     var assign = "INSERT INTO tblAdminTasks (AtaskName, AtaskDetails, AtaskStatus, AtaskRemarks, Deadline, AtaskCreated, [Group])VALUES('"+name+"','"+details+ "','Pending','Not Given','" + deadline + "',GETDATE(), '" + groupName+"')";
            //     db.Database.ExecuteSqlCommand(assign);
            // }
            // return RedirectToAction("AdminDashboard","Dashboard");
        }
        public ActionResult SendAdminTask()
        {
            if(TempData.ContainsKey("name"))
            {
                string group = TempData["name"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["SINGULAR"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '"+group+"'").ToList();
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SendAdminTask(string name, DateTime deadline, string details ,string group)
        {
            using (TableContext db = new TableContext())
            {

                    var assign = "INSERT INTO tblAdminTasks (AtaskName, AtaskDetails, AtaskStatus, AtaskRemarks, Deadline, AtaskCreated, [Group])VALUES('"+name+"','"+details+ "','Pending','Not Given','" + deadline + "',GETDATE(), '" + group+"')";
                    db.Database.ExecuteSqlCommand(assign);
                }
                 return RedirectToAction("AdminDashboard","Dashboard");
            }
            public ActionResult AssignedTasks()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["Tasks"] = db.tblAdminTasks.SqlQuery("SELECT * FROM tblAdminTasks").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AssignedTasks( string remarks, string group)
        {
            using (TableContext db = new TableContext())
            {
            
                  var rem = "UPDATE tblAdminTasks SET AtaskRemarks = '" + remarks + "' WHERE [Group] = '"+group+"' ";
                  db.Database.ExecuteSqlCommand(rem);
              
           
              return RedirectToAction("AdminDashboard","Dashboard");
            }
            //return View();
        }
        public ActionResult Students()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["students"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents").ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult Students( int student)
        {
            using (TableContext db = new TableContext())
            {
                var terminate = "DELETE FROM tblStudents Where studentID = '" + student + "'";
                db.Database.ExecuteSqlCommand(terminate);
            }
            return RedirectToAction("AdminDashboard","Dashboard");
        }
        //public ActionResult Remarks(string remarks)
        //{
        //
        //    using (TableContext db = new TableContext())
        //    {
        //        ViewData["task"] = db.tblAdminTasks.SqlQuery("SELECT * FROM tblAdminTasks Where [Group} = '"+groupnname+"'").ToList();
        //        TempData["remarks"] = remarks;
        //    } return View();
        //}
        //[HttpPost]
        //public ActionResult Remarks()
        //{
        //    string points = ''
        //    if (TempData.ContainsKey("group"))
        //        names = TempData["group"].ToString();
        //
        //    using (TableContext db = new TableContext())
        //    {
        //        var rem = "UPDATE tblAdminTasks SET AtaskRemarks = '" + remarks + "'";
        //        db.Database.ExecuteSqlCommand(rem);
        //    }
        //
        //    return RedirectToAction("AdminDashboard","Dashboard");
        //}

        public ActionResult CompleteProjects()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["projects"] = db.tblCompleteProjects.SqlQuery("SELECT * FROM tblCompleteProjects").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult CompleteProjects(string remarks)
        {
            TempData["group"] = remarks;
            return RedirectToAction("Marksheet");
        }
        public ActionResult Marksheet()
        {
            if(TempData.ContainsKey("group"))
            {
                string markgroup = TempData["group"].ToString();
                using (TableContext db = new TableContext())
                    ViewData["cpgroup"] = db.tblCompleteProjects.SqlQuery("SELECT * FROM tblCompleteProjects WHERE cpGroup = '"+markgroup+"'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Marksheet(string remarks, string grade, string group)
        {
            using(TableContext db = new TableContext())
            {
                var update = "UPDATE tblCompleteProjects set cpGrade = '"+grade+"', cpRemarks = '"+remarks+"' WHERE cpGroup = '"+group+"'";
                db.Database.ExecuteSqlCommand(update);
                
            }
            TempData["mark"] = group;
            return RedirectToAction("ViewMarksheet");

        }
        public ActionResult ViewMarksheet()
        {
            if(TempData.ContainsKey("mark"))
            {
                string name = TempData["mark"].ToString();
                using(TableContext db = new TableContext())
                {
                    ViewData["generate"] = db.tblCompleteProjects.SqlQuery("SELECT * FROM tblCompleteProjects WHERE cpGroup = '"+name+"'").ToList();
                }
            }
            return View();
            
        }
       

    }
}