using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class CreateTaskRequestDto
    {
        public Guid IdUserClient { get; set; }
        public Guid IdPostalCode { get; set; }
        public Guid IdService { get; set; }
        public Guid IdStatus { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
