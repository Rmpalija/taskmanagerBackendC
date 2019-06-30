using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public class Tasks
    {
        public Tasks()
        {
            GroupsTasks = new HashSet<GroupsTasks>();
            LabelsTasks = new HashSet<LabelsTasks>();
        }

        public int Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<GroupsTasks> GroupsTasks { get; set; }
        public virtual ICollection<LabelsTasks> LabelsTasks { get; set; }
    }
}
