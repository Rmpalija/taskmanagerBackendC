using System;
using System.Collections.Generic;

namespace taskmanagerBackendC.Models
{
    public class LabelsTasks
    {
        public int Id { get; set; }
        public int TasksId { get; set; }
        public int LabelsId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Labels Labels { get; set; }
        public virtual Tasks Tasks { get; set; }
    }
}
