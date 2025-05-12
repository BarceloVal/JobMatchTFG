using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class TaskRequestWithOfferDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Title { get; set; }
        public string ClientName { get; set; }
        public string ServiceTypeName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid IdPostalCode { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string PostalCodeName { get; set; }

    }
}
