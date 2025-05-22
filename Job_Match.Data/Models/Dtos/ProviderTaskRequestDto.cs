using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ProviderTaskRequestDto
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public string FullName { get; set; } = "";

        public string TradeName { get; set; }
        public string Address { get; set; }
        public string PostalCodeName { get; set; }
        public string Mail { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Description { get; set; }
        public Guid IdTaskRequest { get; set; }
    }
}
