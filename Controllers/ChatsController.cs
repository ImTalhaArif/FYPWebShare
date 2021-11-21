using System;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFinalProduct_FYP_.db_fypManagement;

namespace TheFinalProduct_FYP_.Controllers
{
    public class ChatsController : Controller
    {
        // GET: Chats
        [HttpGet]
        public ActionResult Chat()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentGroup = '" + Session["studentGroup"] + "'").ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Chat(string a)
        {
            return View();
        }

        public ActionResult studentInbox()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentGroup = '" + Session["studentGroup"] + "'").ToList();

            }
            return View();
        }
        [HttpPost]
        public ActionResult studentInbox(string student)
        {
            TempData["member"] = student;
            return RedirectToAction("memberChat");
        }

        public ActionResult memberChat()
        {
            if(TempData.ContainsKey("member"))
            {
                string chatnow = TempData["member"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["supervisor"] = db.tblStudents.SqlQuery("SELECT * FROM tblStudents WHERE studentFname = '" + chatnow + "'").ToList();
                    ViewData["messages"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + Session["studentFname"] + "' AND receiverName = '" + chatnow + "' ORDER BY timesent ASC").ToList();
                    ViewData["replies"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + chatnow + "' AND receiverName = '" + Session["studentFname"] + "' ORDER BY timesent ASC").ToList();

                }
                TempData.Keep("member");
                Response.AddHeader("Refresh", "20");
            }
            return View();
        }
        [HttpPost]
        public ActionResult memberChat(string receiver, string message)
        {
            using (TableContext db = new TableContext())
            {
                var send = "INSERT INTO tblChats (senderName, message, receiverName, timesent) VALUES ('" + Session["studentFname"] + "', '" + message + "', '" + receiver + "', GETDATE())";
                db.Database.ExecuteSqlCommand(send);
                TempData.Keep("member");
                return RedirectToAction("memberchat");
            }

        }

        public ActionResult Inbox()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Inbox(string supervisor, string student)
        {
            if (supervisor != null)
            {
                return RedirectToAction("supervisorInbox");
            }
            if (student != null)
            {
                return RedirectToAction("studentInbox");
            }
            return View();
        }
        public ActionResult supervisorInbox()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '" + Session["studentGroup"] + "'").ToList();
                foreach (var sr in ViewData["supervisor"] as IEnumerable<TheFinalProduct_FYP_.db_fypManagement.tblGroup>)
                {
                    ViewData["messages"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + sr.groupSupervisor + "' AND receiverName = '" + Session["studentFname"] + "' ORDER BY timesent ASC").ToList();
                    ViewData["replies"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + Session["studentFname"] + "' AND receiverName = '" + sr.groupSupervisor+ "' ORDER BY timesent ASC").ToList();

                }
                Response.AddHeader("Refresh", "20"); 
                return View();
            }

        }
        [HttpPost]
        public ActionResult supervisorInbox(string receiver, string message)
        {
            using (TableContext db = new TableContext())
            {
                var send = "INSERT INTO tblChats (senderName, message, receiverName, timesent) VALUES ('"+Session["studentFname"]+"', '"+message+"', '"+receiver+"', GETDATE())";
                db.Database.ExecuteSqlCommand(send);
                return RedirectToAction("supervisorInbox");
            }

        }
        public ActionResult supervisorGroup()
        {
            using (TableContext db = new TableContext())
            {
                ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupSupervisor = '" + Session["supervisorName"] + "'").ToList();
            }
                return View();
        }
        [HttpPost]
        public ActionResult supervisorGroup(string groupName)
        {
            TempData["name"] = groupName;
            return RedirectToAction("LeaderInbox");

        }

        public ActionResult LeaderInbox()
        {
            if (TempData.ContainsKey("name"))
            {
                string group = TempData["name"].ToString();
                using (TableContext db = new TableContext())
                {
                    ViewData["supervisor"] = db.tblGroups.SqlQuery("SELECT * FROM tblGroups WHERE groupName = '" + group + "'").ToList();
                    foreach (var sr in ViewData["supervisor"] as IEnumerable<TheFinalProduct_FYP_.db_fypManagement.tblGroup>)
                    {
                        ViewData["messages"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + sr.leader + "' AND receiverName = '" + Session["supervisorName"] + "' ORDER BY timesent ASC").ToList();
                        ViewData["replies"] = db.tblChats.SqlQuery("SELECT * FROM tblChats WHERE senderName = '" + Session["supervisorName"] + "' AND receiverName = '" + sr.leader + "' ORDER BY timesent ASC").ToList();

                    }
                    TempData.Keep("name");
                    Response.AddHeader("Refresh", "20");
                    return View();
                }
            }
            return View();       
        }
        [HttpPost]
        public ActionResult LeaderInbox(string receiver, string message)
        {
            using (TableContext db = new TableContext())
            {
                var send = "INSERT INTO tblChats (senderName, message, receiverName, timesent) VALUES ('" + Session["supervisorName"] + "', '" + message + "', '" + receiver + "', GETDATE())";
                db.Database.ExecuteSqlCommand(send);
                TempData["name"] = receiver;
                TempData.Keep("name");
                return RedirectToAction("LeaderInbox");
            }

        }
        
    }
}