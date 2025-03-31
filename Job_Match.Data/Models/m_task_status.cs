using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_task_status")]
public partial class m_task_status
{
    [Key]
    public Guid id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string id_mask { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    public int status_id { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [InverseProperty("id_statusNavigation")]
    public virtual ICollection<t_task_request> t_task_requests { get; set; } = new List<t_task_request>();
}
