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
        public string FullName { get; set; } = "";
    }
}
