using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class TaskRequestWithProvidersDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public string Service { get; set; } = "";
        public string ServiceType { get; set; } = "";
        public string PostalCodeName { get; set; } = "";
        public bool Active { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public List<ProviderTaskRequestDto> Providers { get; set; } = new();
    }
}
