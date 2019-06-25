using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public class Labels
    {
        public Labels()
        {
            LabelsTasks = new HashSet<LabelsTasks>();
        }

        public int Id { get; set; }
        public string LabelTitle { get; set; }
        public string LabelDescription { get; set; }
        public string LabelColor { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<LabelsTasks> LabelsTasks { get; set; }
    }
}
