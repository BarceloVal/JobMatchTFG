using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_user")]
public partial class m_user
{
    [Key]
    public Guid id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string id_mask { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string mail { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string password { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string first_name { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string last_name { get; set; } = null!;

    public bool is_provider { get; set; }

    public bool is_client { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_access { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [InverseProperty("id_userNavigation")]
    public virtual ICollection<m_user_client> m_user_clients { get; set; } = new List<m_user_client>();

    [InverseProperty("id_userNavigation")]
    public virtual ICollection<m_user_provider> m_user_providers { get; set; } = new List<m_user_provider>();

    [InverseProperty("id_user_reciverNavigation")]
    public virtual ICollection<t_task_message> t_task_messageid_user_reciverNavigations { get; set; } = new List<t_task_message>();

    [InverseProperty("id_user_senderNavigation")]
    public virtual ICollection<t_task_message> t_task_messageid_user_senderNavigations { get; set; } = new List<t_task_message>();

    [InverseProperty("id_user_reviewedNavigation")]
    public virtual ICollection<t_task_score> t_task_scoreid_user_reviewedNavigations { get; set; } = new List<t_task_score>();

    [InverseProperty("id_user_reviewerNavigation")]
    public virtual ICollection<t_task_score> t_task_scoreid_user_reviewerNavigations { get; set; } = new List<t_task_score>();
}
