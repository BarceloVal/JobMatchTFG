using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_service_type")]
public partial class m_service_type
{
    [Key]
    public Guid id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string id_mask { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? description { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [InverseProperty("id_service_typeNavigation")]
    public virtual ICollection<m_service> m_services { get; set; } = new List<m_service>();
}
