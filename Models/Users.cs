using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public partial class Users
    {
        public Users()
        {
            Groups = new HashSet<Groups>();
            GroupsUser = new HashSet<GroupsUser>();
            Labels = new HashSet<Labels>();
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual ICollection<Groups> Groups { get; set; }
        public virtual ICollection<GroupsUser> GroupsUser { get; set; }
        public virtual ICollection<Labels> Labels { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
