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
    public class GroupsController : Controller
    {
        //This controller is Session authenticated and a group generation Action method for Student 
        public ActionResult StdGroupsCreate()
        {
            if (Session["studentFname"] != null)
            {
                return View();

                if (Session["studentGroup"] == "none")
                {
                    return View();
                }
                else if (Session["isLeader"] == "true")
                {
                    return RedirectToAction("GroupDashboard");
                }
                else
                {
                    return RedirectToAction("MGroupDashboard");
                }
            }
            return RedirectToAction("StdLogin", "Login");
        }
        [HttpPost] //utilising the HttpPost method for form submission here
        public ActionResult StdGroupsCreate(tblGroup grp)
        {
            using (TableContext db = new TableContext())
            {

                var create = "INSERT INTO tblGroups (groupName, leader,createdOn, MembersCount) values ('" + grp.groupName + "','" + Session["studentFname"].ToString() + "',GetDate(), 1)";
                db.Database.ExecuteSqlCommand(create);
                var stdupdate = "UPDATE tblStudents set isLeader = 'true', studentGroup = '" + grp.groupName + "'  where studentID = '" + Session["studentID"] + "' ";
                db.Database.ExecuteSqlCommand(stdupdate);
                var progress = "INSERT INTO tblProjectProgresses (progressGroup, projectProgress) VALUES ('"+grp.groupName+"', 0)";
                db.Database.ExecuteSqlCommand(progress);
                Session["studentGroup"] = grp.groupName.ToString();
                Session["isLeader"] = "true";

                return RedirectToAction("AddMembers");

            }

        }

        //created a group, now we need members
        //The action method below is For inviting members to join the group

        public ActionResult AddMembers()
        {
            if ((string)Session["isLeader"] == "false")
            {

                return RedirectToAction("GroupDashboard", "Groups");
            }
            using (TableContext db = new TableContext())
            {
                //ViewData to list the students in an array and send to view
                ViewData["students"] = db.tblStudents.SqlQuery("SELECT * from tblStudents where studentGroup = 'none' ").ToList();
                return View(ViewData["students"]);
            }
        }
        [HttpPost]
        public ActionResult AddMembers(int[] invite)
        {
            using (TableContext db = new TableContext())
            {
                //we have used an array here to bring in all the students we gonna invite to the group
                foreach (int a in invite)
                {
                    var add = "INSERT INTO tblNotifications (NotificationTitle,NotificationDetails, SendTo_studentID, GroupName) values ('Group Invite','Please Join our group','" + a + "','" + Session["studentGroup"] + "')";
                    db.Database.ExecuteSqlCommand(add);
                }
            }

            return RedirectToAction("StudentDashboard", "Dashboard");
        }
        //This controller method is gonna be an accept invitation to join group, we will use ExecuteSqlcommand here and concatenate studentID in Query here

        public ActionResult AcceptInvite()
        {
            using (TableContext db = new TableContext())
            {
                //ViewData to list the students in an array and send to view
                ViewData["students"] = db.tblNotifications.SqlQuery("SELECT * from tblNotifications where SendTo_studentID = '" + Session["studentID"] + "'").ToList();
                return View(ViewData["students"]);
            }
        }
        [HttpPost]
        public ActionResult AcceptInvite(string invite)
        {
            using (TableContext db = new TableContext())
            {
                //hence, after this the student group is set to the invite or proposal 
                var add = "UPDATE tblstudents SET studentGroup = '" + invite + "' WHERE studentID = '" + Session["studentID"] + "'";
                db.Database.ExecuteSqlCommand(add);
                var count = "update tblGroups set MembersCount = MembersCount + 1 where groupName = '" + invite + "' ";
                db.Database.ExecuteSqlCommand(count);
                // var upd = "UPDATE tblStudents set tblGroup_groupID = '" + invite + "' where studentID = '" + Session["studentID"] + "'";
                // db.Database.ExecuteSqlCommand(upd);
                var notification = "delete from tblNotifications where SendTo_studentID = '" + Session["studentID"] + "'";
                db.Database.ExecuteSqlCommand(notification);
                // Session.Abandon();
                Session["studentGroup"] = invite.ToString();
                return RedirectToAction("MGroupDashboard", "Groups");
            }




            //return View();
        }
        //Now this Action method is the group dashboard where you can see how many members do you have nyour group and the supervisor of your group, after it is assigned
        public ActionResult GroupDashboard()
        {

            
           
            using (TableContext db = new TableContext())
            {
                //var grp = db.tblGroups.SingleOrDefault();
                ViewData["gantt"] = db.tblGanttCharts.SqlQuery("SELECT * FROM tblGanttCharts WHERE gcGroup = '"+Session["studentGroup"]+"'").ToList();
                ViewData["logchart"] = db.tblLogCharts.SqlQuery("SELECT * FROM tblLogCharts where lcGroup = '"+Session["studentGroup"]+"'").ToList();
                ViewData["students"] = db.tblStudents.SqlQuery("SELECT * from tblStudents where studentGroup = '" + Session["studentGroup"] + "'").ToList();
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * from tblGroups where groupName = '" + Session["studentGroup"] + "'").ToList();
                ViewData["groupTasks"] = db.tblGroupTasks.SqlQuery("SELECT * from tblGroupTasks where Taskgroup = '" + Session["studentGroup"] + "'").ToList();
                ViewData["AdminTask"] = db.tblAdminTasks.SqlQuery("SELECT * FROM tblAdminTasks where [Group] = '" + Session["studentGroup"] + "'").ToList();
                ViewData["progress"] = db.tblProjectProgresses.SqlQuery("SELECT * FROM tblProjectProgresses WHERE progressGroup = '" + Session["studentGroup"] + "'").ToList();  
                ViewData["download"] = db.tblProjects.SqlQuery("SELECT * from tblProjects WHERE projectGroup = '"+Session["studentGroup"]+"'").ToList();

                ViewData["ProjectLink"] = db.tblProjects.SqlQuery("SELECT * FROM tblProjects where projectGroup = '" + Session["studentGroup"] + "'").ToList();
               
                
                return View();
            }
        }
        //If The leader wishes to remove a member
        [HttpPost]
        public ActionResult GroupDashboard(string remove, string lcname, string detail, DateTime? lcdate, string week, string title, string desc, string pro)
        {
            if(pro != null)
            {
                TempData["download"] = pro;
                return RedirectToAction("downloadProject");
            }

            if(desc != null)
            {
                using (TableContext db = new TableContext())
                {
                    var gc = "INSERT INTO tblGanttCharts (gcGroup, gcWeek, gcName) VALUES ('"+Session["studentGroup"]+"' , '"+week+"','"+title+"')";
                    db.Database.ExecuteSqlCommand(gc);
                    return RedirectToAction("GroupDashboard");
                }
            }



            if(lcname != null)
            {
                using (TableContext db = new TableContext())
                {
                    var log = "INSERT INTO tblLogCharts (lcGroup, lcDetail, lcDate, lcCreatedby) VALUES ('"+Session["studentGroup"]+"', '"+detail+"', '"+lcdate+"', '"+lcname+"')";
                    db.Database.ExecuteSqlCommand(log);
           
                    return RedirectToAction("GroupDashboard");
                }
            }
           

            
                using (TableContext db = new TableContext())
                {
                    var rem = "UPDATE tblStudents SET studentGroup = 'none' where studentID = '" + remove + "'";
                    db.Database.ExecuteSqlCommand(rem);
                var count = "update tblGroups set MembersCount = MembersCount - 1 where groupName = '" + Session["studentGroup"] + "' ";
                db.Database.ExecuteSqlCommand(count);
                return RedirectToAction("GroupDashboard");



                }
            //    return View();

            
                

            

        }

        public ActionResult Marksheet()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["generate"] = db.tblCompleteProjects.SqlQuery("SELECT * FROM tblCompleteProjects WHERE cpGroup = '" + Session["studentGroup"] + "'").ToList();
            }
                return View();
        }

        public ActionResult generatePDF()
        {
            return new Rotativa.ActionAsPdf("Marksheet");
        }


        public ActionResult MGroupDashboard()
        {

            using (TableContext db = new TableContext())
            {
                //var grp = db.tblGroups.SingleOrDefault();


                ViewData["students"] = db.tblStudents.SqlQuery("SELECT * from tblStudents where studentGroup = '" + Session["studentGroup"] + "'").ToList();
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * from tblGroups where groupName = '" + Session["studentGroup"] + "'").ToList();
                ViewData["groupTasks"] = db.tblGroupTasks.SqlQuery("SELECT *from tblGroupTasks where Taskgroup = '" + Session["studentGroup"] + "'").ToList();
                ViewData["AdminTask"] = db.tblAdminTasks.SqlQuery("SELECT * FROM tblAdminTasks where [Group] = '" + Session["studentGroup"] + "'").ToList();
                ViewData["ProjectLink"] = db.tblProjects.SqlQuery("SELECT * FROM tblProjects where projectGroup = '" + Session["studentGroup"] + "'").ToList();
                return View(ViewData["students"]);
            }
        }

        public ActionResult NotAllowed()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NotAllowed(tblGroup gr)
        {
            return RedirectToAction("GroupDashboard");

        }

        public ActionResult TaskSubmission()
        {


            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '"+Session["studentGroup"]+"'").ToList();
                ViewData["details"] = db.tblGroupTasks.SqlQuery("SELECT * from tblGroupTasks where AssignedTo = '" + Session["studentEmail"] + "'").ToList();
            }
                    return View();
        }
        [HttpPost]
        public ActionResult TaskSubmission(HttpPostedFileBase taskfile, string supervisor, int progress)
        {
            string path = Path.Combine(Server.MapPath("~/studentTasks/"), Path.GetFileName(taskfile.FileName));
            taskfile.SaveAs(path);    //The proposal is saved to a folder in project solution

            using (TableContext ai = new TableContext())
            {
                tblStudents ab = new tblStudents();

                //int userID = Convert.ToInt32(Session["studentGroup_groupID"]);



                var commandText = "update tblGroupTasks set submissionFile = '" + taskfile.FileName + "', submissionDate = GETDATE() , gtStatus = 'Completed' where Taskgroup = '" + Session["studentGroup"] + "' ";
                var notify = "INSERT INTO tblsubmittedtasks (stName, stSupervisor) VALUES ('"+taskfile.FileName+"', '"+supervisor+"')";
                ai.Database.ExecuteSqlCommand(notify);
                var progression = "UPDATE tblProjectProgresses SET projectProgress = projectProgress + 10 WHERE progressGroup = '" + Session["studentGroup"] + "'";
                ai.Database.ExecuteSqlCommand(progression);
                //var leader = "UPDATE tblStudents set isLeader = 'true' where studentID = " + Session["studentID"] + "";


            
                ai.Database.ExecuteSqlCommand(commandText);
               ai.SaveChanges();

                ViewBag.message = "DONE!";
                // return RedirectToAction("Create", "Group");
                //ai.tblGroups.Add(str);
                ai.SaveChanges();

            }
            //Clear modelstate after registration
            ModelState.Clear();
            //return RedirectToAction("StudentDashboard");



            return RedirectToAction("GroupDashboard", "Groups");

        }
        public ActionResult RequestMeeting()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '" + Session["studentGroup"] + "'").ToList();
            }
            return View();
        }

        [HttpPost]
        public ActionResult RequestMeeting( string groupName, DateTime date, string supervisor)
        {
            using (TableContext db = new TableContext())
            {
                var meeting = "INSERT INTO tblRequestMeetings (rmDateTime, rmGroup, MSupervisor) VALUES ('"+date+"','"+Session["studentGroup"]+"','"+supervisor+"')";
                db.Database.ExecuteSqlCommand(meeting);
            }


            return RedirectToAction("GroupDashboard","Groups");
        }
        public ActionResult ApprovedMeetings()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["meetings"] = db.tblMeetings.SqlQuery("SELECT * FROM tblMeetings WHERE meetingGroup = '"+Session["studentGroup"]+"'").ToList();
                    }
            return View();
        }

        
        public ActionResult ATaskSubmission()
        {
            if ((string)Session["isLeader"] == "true")
            {
                return View();
            }
            else { return RedirectToAction("MGroupDashboard"); }

            
        }
        [HttpPost]
        public ActionResult ATaskSubmission(HttpPostedFileBase file)
        {
           
                string path = Path.Combine(Server.MapPath("~/AdminTasks/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);    //The proposal is saved to a folder in project solution

                using (TableContext ai = new TableContext())
                {
                    tblStudents ab = new tblStudents();

                    //int userID = Convert.ToInt32(Session["studentGroup_groupID"]);



                    var commandText = "update tblAdminTasks set AtaskFile = '" + file.FileName + "',  AtaskStatus = 'Completed'  where [Group] = '" + Session["studentGroup"] + "'";
                //var status = "UPDATE tblAdminTasks set AtaskStatus = 'Completed where [Group] = '" + Session["studentGroup"] + "'";
                    //var leader = "UPDATE tblStudents set isLeader = 'true' where studentID = " + Session["studentID"] + "";



                    ai.Database.ExecuteSqlCommand(commandText);
                //ai.Database.ExecuteSqlCommand(status);
                    ai.SaveChanges();

                    ViewBag.message = "DONE!";
                    // return RedirectToAction("Create", "Group");
                    //ai.tblGroups.Add(str);
                    ai.SaveChanges();

                }
           
            //Clear modelstate after registration
            ModelState.Clear();
            //return RedirectToAction("StudentDashboard");



            return RedirectToAction("GroupDashboard", "Groups");
            //return View();
        }

        public ActionResult ProjectSubmission()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProjectSubmission(HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/CompleteProjects/"), Path.GetFileName(file.FileName));
            file.SaveAs(path);    //The proposal is saved to a folder in project solution

            using (TableContext ai = new TableContext())
            {
                var commandText = "INSERT INTO tblCompleteProjects (cpName, cpGroup, cpSubmission) VALUES ('" + file.FileName + "','" + Session["studentGroup"] + "', GETDATE())";
               ai.Database.ExecuteSqlCommand(commandText);
              
                ai.SaveChanges();

            }
            return RedirectToAction("GroupDashboard", "Groups");

            //return View();

        }
    }
}