using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;
namespace TheFinalProduct_FYP_.Controllers
{
    public class LoginController : Controller
    {

        
        // This Controller contains All Login Classes for Student Supervisor and Admin
        // The Form Submission is working through [HTTPPOST] and ISessionHandler to get session validation
        public ActionResult StdLogin()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult StdLogin(tblStudents log)
        {
            //Since, we are using CodeFirst approach for database so we gonna use the context class for calling in the database
            using (TableContext db = new TableContext())
            {
                var student = db.tblStudents.FirstOrDefault(p => p.studentEmail == log.studentEmail && p.studentPassword == log.studentPassword);
                if (student != null)
                {
                    Session["studentEmail"] = student.studentEmail.ToString();
                    Session["StudentID"] = student.studentID.ToString();
                    Session["studentFname"] = student.studentFname.ToString();
                    Session["isLeader"] = student.isLeader.ToString();
                        Session["studentGroup"] = student.studentGroup.ToString();Session["studentGroup"] = student.studentGroup.ToString();
                    if((string)Session["isLeader"] == "false")
                    {
                        return RedirectToAction("MemberDashboard", "Dashboard");
                    }
                    //{
                    //    return RedirectToAction("GroupDashboard", "Groups");
                    //}
                    
                   // if (Session["studentGroup"] != null)
                   // {
                   //     Session["studentGroup"] = student.studentGroup.ToString();
                   // }

                    Session["studentLname"] = student.studentLname.ToString();
                    return RedirectToAction("StudentDashboard","Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "");
                }
            }

            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("StdLogin");
        }
        public ActionResult SLogOut()
        {
            Session.Abandon();
            return RedirectToAction("SupervisorLogin");
        }

        // Student Registration controller
        public ActionResult StudentRegistration()
        {
            return View();
        }
        //Student registration using ModelState on HTTpPost...
        [HttpPost]
        

        public ActionResult StudentRegistration([Bind(Include = "studentID,studentFname,studentLname,studentPname,studentEmail,studentPassword,studentDOB,isLeader ,studentGroup")] tblStudents tblStudents, DateTime DateOfBirth)
        {

            if (ModelState.IsValid)
            {
                
                tblStudents.studentDOB = DateOfBirth;
                tblStudents.isLeader = "false";
                tblStudents.studentGroup = "none";
                using (TableContext db = new TableContext())
                        {
                    db.tblStudents.Add(tblStudents);
                    db.SaveChanges();
                    return RedirectToAction("StdLogin");
                } }

            return View();

        }



        //SupervisorLogin BELOWW.....

        public ActionResult SupervisorLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SupervisorLogin(tblSupervisors sup)
        {
            using (TableContext db = new TableContext())
            {
                var supervisor = db.tblSupervisors.SingleOrDefault(p => p.supervisorEmail == sup.supervisorEmail && p.supervisorPassword == sup.supervisorPassword);
                if (supervisor != null)
                {
                    Session["supervisorID"] = supervisor.supervisorID.ToString();
                    Session["supervisorName"] = supervisor.supervisorName.ToString();

                    return RedirectToAction("SupervisorDashboard","Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "");
                }

            }
            return View();
        }

        //Registration controls for supervisor
        public ActionResult SupervisorRegistration()
        {
            return View();
        }
        //Supervisor registration using ModelState on HTTpPost...
        [HttpPost]
        public ActionResult SupervisorRegistration([Bind(Include = "supervisorID,supervisorName,supervisorEmail,supervisorPassword,supervisorDomain")] tblSupervisors tblSupervisors)
        {
            using (TableContext db = new TableContext())
            {

                if (ModelState.IsValid)
                {
                    db.tblSupervisors.Add(tblSupervisors);
                    db.SaveChanges();
                    return RedirectToAction("SupervisorLogin");
                }
            }

            // return RedirectToAction("Login");



            return View();
        }

        //Admin Login Controls

        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(tblAdmin adm)
        {
            using (TableContext db = new TableContext())
            {
                var supervisor = db.tblAdmins.SingleOrDefault(p => p.adminEmail == adm.adminEmail && p.adminPassword == adm.adminPassword);
                if (supervisor != null)
                {
                    Session["adminID"] = supervisor.adminID.ToString();
                    Session["adminName"] = supervisor.adminName.ToString();
                    Session["adminEmail"] = supervisor.adminEmail.ToString();

                    return RedirectToAction("AdminDashboard", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "");
                }

            }
            return View();
        }
        public ActionResult ALogOut()
        {
            Session.Abandon();
            return RedirectToAction("AdminLogin");
        }



    }
}