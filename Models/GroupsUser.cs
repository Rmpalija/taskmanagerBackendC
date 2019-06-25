using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public partial class GroupsUser
    {
        public int Id { get; set; }
        public int GroupsId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Groups Groups { get; set; }
        public virtual Users User { get; set; }
    }
}
