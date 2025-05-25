using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid IdServiceType { get; set; }
        public string ServiceTypeName { get; set; }
        public string ServiceName { get; set; }
    }
}
