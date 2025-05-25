using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class CreateTaskScoreDto
    {
        public Guid IdUserReviewer { get; set; }
        public Guid IdUserReviewed { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
