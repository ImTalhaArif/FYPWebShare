using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TheFinalProduct_FYP_.db_fypManagement
{
    public partial class TableContext:DbContext
    {
        public TableContext()
          :base("Name=FYPcontext")
        {

        }
        public DbSet <tblAdmin> tblAdmins { get; set; }
        public DbSet<tblAdminTasks> tblAdminTasks { get; set; }
        public DbSet<tblGroup> tblGroups { get; set; }
        public DbSet<tblGroupTasks> tblGroupTasks { get; set; }
        public DbSet<tblMeeting> tblMeetings { get; set; }
        public DbSet<tblProjectRequests> tblProjectRequests { get; set; }
        public DbSet<tblProjects> tblProjects { get; set; }

        public DbSet<tblNotifications> tblNotifications { get; set; }
        public DbSet<tblRequestMeeting> tblRequestMeetings { get; set; }
        public DbSet<tblStudents> tblStudents { get; set; }
        public DbSet<tblSupervisorProjects> tblSupervisorProjects { get; set; }
        public DbSet<tblSupervisors> tblSupervisors { get; set; }
        public DbSet<tblRequestStatus> tblRequestStatus { get; set; }
        public DbSet<tblTaskNotification> tblTaskNotification { get; set; }
        public DbSet<tblsubmittedtasks> tblsubmittedtasks { get; set; }
        public DbSet<tblProjectProgress> tblProjectProgresses { get; set; }
        public DbSet<tblLogChart> tblLogCharts { get; set; }
        public DbSet<tblGanttChart> tblGanttCharts { get; set; }
        public DbSet<tblAdminDeadlines> tblAdminDeadlines { get; set; }
        public DbSet<tblChats> tblChats { get; set; }
        public DbSet<tblCompleteProjects> tblCompleteProjects { get; set; }
    }
}