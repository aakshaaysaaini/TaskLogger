using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskLoggerApplication.Models
{
    public class ProjectTasksViewModel
    {
        public List<Project> Projects { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}