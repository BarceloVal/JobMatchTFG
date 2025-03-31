using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_user_provider")]
public partial class m_user_provider
{
    [Key]
    public Guid id { get; set; }

    public Guid id_user { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string trade_name { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? description { get; set; }

    public Guid id_doc_type { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string num_doc { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? trade_logo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? map_coordinates { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string address { get; set; } = null!;

    public Guid id_postal_code { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string phone_number_1 { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? phone_number_2 { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? mail { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? web_site { get; set; }

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [ForeignKey("id_doc_type")]
    [InverseProperty("m_user_providers")]
    public virtual m_doc_type id_doc_typeNavigation { get; set; } = null!;

    [ForeignKey("id_postal_code")]
    [InverseProperty("m_user_providers")]
    public virtual m_postal_code id_postal_codeNavigation { get; set; } = null!;

    [ForeignKey("id_user")]
    [InverseProperty("m_user_providers")]
    public virtual m_user id_userNavigation { get; set; } = null!;

    [InverseProperty("id_user_providerNavigation")]
    public virtual ICollection<t_task_contact> t_task_contacts { get; set; } = new List<t_task_contact>();
}
