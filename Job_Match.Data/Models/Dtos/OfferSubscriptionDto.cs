using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models.Dtos
{
    public class OfferSubscriptionDto
    {
        public Guid IdTaskRequest { get; set; }
        public Guid IdUserProvider { get; set; }
    }
}
