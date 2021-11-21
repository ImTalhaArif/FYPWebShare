using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public ActionResult CreateNew()
        {
            if ((string)Session["isLeader"] == "false")
            {

                return RedirectToAction("GroupDashboard", "Groups");
            }
            // if(Session["isLeader"] == null)
            // {
            //     return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            // }



            return View();
        }
        [HttpPost]
        public ActionResult CreateNew(tblProjects pro, HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/StudentProjects/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);    //The proposal is saved to a folder in project solution

            using (TableContext ai = new TableContext())
            {
                tblStudents ab = new tblStudents();

                //int userID = Convert.ToInt32(Session["studentGroup_groupID"]);



                var commandText = "INSERT INTO tblProjects (projectName,projectGroup, ProjectProposal,approvalStatus) VALUES ('" + pro.projectName + "','"+Session["studentGroup"]+"','" + file.FileName + "','notApproved')";
                //var leader = "UPDATE tblStudents set isLeader = 'true' where studentID = " + Session["studentID"] + "";
                var project = "UPDATE tblGroups SET groupProject = '"+pro.projectName+"' WHERE groupName = '"+Session["studentGroup"]+"'";


                var name = new SqlParameter("@CategoryName", "Test");
                ai.Database.ExecuteSqlCommand(commandText);
                ai.Database.ExecuteSqlCommand(project);

                //ai.Database.ExecuteSqlCommand(leader, name);
                ai.SaveChanges();

                ViewBag.message = "DONE!";
                // return RedirectToAction("Create", "Group");
                //ai.tblGroups.Add(str);
                ai.SaveChanges();

            }
            //Clear modelstate after registration
            ModelState.Clear();
            //return RedirectToAction("StudentDashboard");



            return RedirectToAction("StudentDashboard", "Dashboard");

            //return View();
        }

        //This Action method below is gonna be used to send project supervision request to the supervisor
        public ActionResult SendRequest()
        {
            if ((string)Session["isLeader"] == "false")
            {

                return RedirectToAction("GroupDashboard", "Groups");
            }
            using (TableContext db = new TableContext())
            {
                ViewData["supervisors"] = db.tblSupervisors.SqlQuery("SELECT * from tblSupervisors ").ToList();
                ViewData["Projects"] = db.tblProjects.SqlQuery("SELECT * FROM tblProjects WHERE projectGroup = '"+Session["studentGroup"]+"'").ToList();
                ViewData["supervisorspr"] = db.tblSupervisorProjects.SqlQuery("SELECT * from tblSupervisorProjects ").ToList();

                return View(ViewData["supervisors"]);
                //ViewData["supervisorspr"]);

                


                
            }
        }
        [HttpPost]
        public ActionResult SendRequest(tblProjectRequests req, string supervisor, string project, string sproject)
        {
            using (TableContext db = new TableContext())
            {

                var request = "INSERT INTO tblProjectRequests ([Group], Project, Supervisor,CreatedDate) VALUES ('" + Session["studentGroup"] + "','" + project + "','" + supervisor + "',GetDate())";
                db.Database.ExecuteSqlCommand(request);
            }
            return RedirectToAction("GroupDashboard","Groups");
        }
       
        public ActionResult SupervisorProjects()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SupervisorProjects(tblSupervisorProjects pro, HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/SupervisorProjects/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);    //The proposal is saved to a folder in project solution

            using (TableContext ai = new TableContext())
            {
                tblStudents ab = new tblStudents();

                //int userID = Convert.ToInt32(Session["studentGroup_groupID"]);



                var commandText = "INSERT INTO tblSupervisorProjects (spName ,spFile, spDate, supervisor_supervisorID) VALUES ('" + pro.spName + "','" + file.FileName + "',GetDate(), '"+Session["supervisorID"]+"')";
                //var leader = "UPDATE tblStudents set isLeader = 'true' where studentID = " + Session["studentID"] + "";
              


                ai.Database.ExecuteSqlCommand(commandText);
                
                ai.SaveChanges();

                ViewBag.message = "DONE!";
                ai.SaveChanges();

            }
            //Clear modelstate after registration
            ModelState.Clear();
            //return RedirectToAction("StudentDashboard");



            return RedirectToAction("Projects", "Supervisor");
        }

        public ActionResult ViewSupervisors()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisors"] = db.tblSupervisors.SqlQuery("SELECT * from tblSupervisors").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewSupervisors(int details)
        {
            TempData["id"] = details;
            
            return RedirectToAction("SingleSupervisor");
        }
        public ActionResult SingleSupervisor()
        {
            using (TableContext db = new TableContext())
            {
                if (TempData.ContainsKey("id"))
                {
                    int mingle = (int)TempData["id"];


                    ViewData["single"] = db.tblSupervisors.SqlQuery("SELECT * FROM tblSupervisors where supervisorID = '" + mingle + "'").ToList();
                    ViewData["projects"] = db.tblSupervisorProjects.SqlQuery("SELECT * FROM tblSupervisorProjects where supervisor_supervisorID = '" + mingle + "'").ToList();


                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SingleSupervisor(string send, int id)
        {
            TempData["ID"] = id;
            TempData["spname"] = send;
            return RedirectToAction("OurRequest");
        }
        public ActionResult OurRequest()
        {
            if (TempData.ContainsKey("ID"))
            {
                int spid = (int)TempData["ID"];
                if (TempData.ContainsKey("spname"))
                {
                    string supervisor = TempData["spname"].ToString();
                    using (TableContext db = new TableContext())
                    {
                        ViewData["List"] = db.tblSupervisors.SqlQuery("select * from tblSupervisors WHERE supervisorName = '" + supervisor + "'").ToList();
                        ViewData["pro"] = db.tblProjects.SqlQuery("SELECT * FROM tblProjects Where projectGroup = '" + Session["studentGroup"] + "'").ToList();
                        ViewData["spro"] = db.tblSupervisorProjects.SqlQuery("SELECT * FROM tblSupervisorProjects WHERE supervisor_supervisorID = '"+spid+"'").ToList();
                    }
                }
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult OurRequest( string project, string sproject, string supervisor)
        {
            if(project != "null")
            {
                using (TableContext db = new TableContext())
                {
                    var request = "INSERT INTO tblProjectRequests ([Group], Supervisor, CreatedDate, Project) VALUES ('"+Session["studentGroup"]+"', '"+supervisor+"', GetDate(), '"+project+"')";
                    db.Database.ExecuteSqlCommand(request);

                }

            }
            else if(sproject != "null")
            {
                using (TableContext db = new TableContext())
                {
                    var request = "INSERT INTO tblProjectRequests ([Group], Supervisor, CreatedDate, Project) VALUES ('" + Session["studentGroup"] + "', '" + supervisor + "', GetDate(), '" + sproject + "')";
                    db.Database.ExecuteSqlCommand(request);

                }

            }
            return RedirectToAction("GroupDashboard","Groups");
        }
    }
}