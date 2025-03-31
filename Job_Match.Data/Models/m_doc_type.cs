using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Models;

[Table("m_doc_type")]
public partial class m_doc_type
{
    [Key]
    public Guid id { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string id_mask { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string name { get; set; } = null!;

    public bool active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime created_on { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last_update { get; set; }

    [InverseProperty("id_doc_typeNavigation")]
    public virtual ICollection<m_user_client> m_user_clients { get; set; } = new List<m_user_client>();

    [InverseProperty("id_doc_typeNavigation")]
    public virtual ICollection<m_user_provider> m_user_providers { get; set; } = new List<m_user_provider>();
}
