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
        public Guid IdUser { get; set; }
        public string TradeName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Mail { get; set; }
        public string Website { get; set; }
        public string PostalCodeName { get; set; }
        public string Description { get; set; }
        public Guid IdPostalCode { get; set; }
        public List<ServiceDto> Services { get; set; } = new();
    }
}
