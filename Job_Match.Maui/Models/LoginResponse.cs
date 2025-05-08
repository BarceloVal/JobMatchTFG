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
        public Guid? id_user_client { get; set; }
        public Guid? id_user_provider { get; set; }
        public string id_mask { get; set; }
        public string mail { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_client { get; set; }
        public bool is_provider { get; set; }
        public DateTime? last_access { get; set; }
        public bool active { get; set; }
        public DateTime created_on { get; set; }
        public DateTime? last_update { get; set; }

        // BONUS (si querés usarlo fácil en la UI)
        public string full_name => $"{first_name} {last_name}";
    }
}
