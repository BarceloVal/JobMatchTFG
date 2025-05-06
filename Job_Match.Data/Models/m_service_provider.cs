using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Data.Models
{
    [Table("m_service_provider")]
    public class m_service_provider
    {
        public Guid id_user_provider { get; set; }
        public Guid id_service { get; set; }
        [Required]
        [StringLength(10)]
        public string id_mask { get; set; }
        public bool active { get; set; } = true;
        public DateTime created_on { get; set; }
        public DateTime? last_update { get; set; }

        [ForeignKey(nameof(id_user_provider))]
        [InverseProperty("m_service_providers")]
        public virtual m_user_provider id_user_providerNavigation { get; set; } = null!;

        [ForeignKey(nameof(id_service))]
        public virtual m_service m_service { get; set; } = null!;
    }
}
