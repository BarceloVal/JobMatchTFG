using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_service")]
public partial class m_service
{
    [Key]
    public Guid id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string id_mask { get; set; } = null!;

    public Guid id_service_type { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string description { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_service_type")]
    [InverseProperty("m_services")]
    public virtual m_service_type id_service_typeNavigation { get; set; } = null!;

    [InverseProperty("id_serviceNavigation")]
    public virtual ICollection<t_task_request> t_task_requests { get; set; } = new List<t_task_request>();

    [InverseProperty(nameof(m_service_provider.m_service))]
    public virtual ICollection<m_service_provider> m_service_providers { get; set; } = new List<m_service_provider>();

}
