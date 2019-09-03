using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskLoggerApplication.Models
{
    public enum Status
    {
        Backlog,
        InProgress,
        Completed
    }
    public class Tasks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public Status Status { get; set; }
        public virtual Project Project { get; set; }
        public ApplicationUser User { get; set; }
    }
}