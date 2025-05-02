using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ProviderWithServicesDto
    {
        public Guid Id { get; set; }
        public string TradeName { get; set; }
        public string Address { get; set; }
        public Guid IdPostalCode { get; set; }
        public List<ServiceDto> Services { get; set; } = new();
    }

    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid IdServiceType { get; set; }
    }
}
