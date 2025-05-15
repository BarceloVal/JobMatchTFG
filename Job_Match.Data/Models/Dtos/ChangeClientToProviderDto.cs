using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ChangeClientToProviderDto
    {
        public Guid IdUser { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; }
        public Guid? IdDocType { get; set; }
        public string? Address { get; set; }
        public Guid? IdPostalCode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }


        public string? TradeName { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? MapCoordinates { get; set; }
        public Guid? IdServiceType { get; set; }
        public Guid? IdService { get; set; }
        public string Password { get; set; }
    }
}
