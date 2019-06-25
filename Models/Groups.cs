using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public partial class Groups
    {
        public Groups()
        {
            GroupsTasks = new HashSet<GroupsTasks>();
            GroupsUser = new HashSet<GroupsUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AdminId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Users Admin { get; set; }
        public virtual ICollection<GroupsTasks> GroupsTasks { get; set; }
        public virtual ICollection<GroupsUser> GroupsUser { get; set; }
    }
}
