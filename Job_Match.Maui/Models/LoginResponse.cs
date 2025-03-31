using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public _UserDto user { get; set; }
    }

    public class _UserDto
    {
        public Guid id { get; set; }
        public string mail { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
