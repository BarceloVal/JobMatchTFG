using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class ServiceProviderDto
    {
        public Guid ProviderId { get; set; }
        public List<Guid> ServiceIds { get; set; }
    }
}
