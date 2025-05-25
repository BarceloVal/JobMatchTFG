using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class TaskScoreDisplayDto
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReviewerName { get; set; }
        public bool IsMine { get; set; }

        public Guid IdUserReviewer { get; set; }
        public Guid IdUserReviewed { get; set; }
    }
}
