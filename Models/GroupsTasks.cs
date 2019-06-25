using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public partial class GroupsTasks
    {
        public int Id { get; set; }
        public int GroupsId { get; set; }
        public int TasksId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Groups Groups { get; set; }
        public virtual Tasks Tasks { get; set; }
    }
}
