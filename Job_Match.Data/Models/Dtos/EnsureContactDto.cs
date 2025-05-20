using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class EnsureContactDto
    {
        public Guid IdUserClient { get; set; }
        public Guid IdUserProvider { get; set; }
        public Guid? IdTaskRequest { get; set; }
    }

}
