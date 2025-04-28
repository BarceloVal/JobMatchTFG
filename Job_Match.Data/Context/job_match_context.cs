using System;
using System.Collections.Generic;
using Job_Match.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Job_Match.Data.Context;

public partial class job_match_context : DbContext
{
    public job_match_context()
    {
    }

    public job_match_context(DbContextOptions<job_match_context> options)
        : base(options)
    {
    }

    public virtual DbSet<m_doc_type> m_doc_types { get; set; }

    public virtual DbSet<m_postal_code> m_postal_codes { get; set; }

    public virtual DbSet<m_service> m_services { get; set; }

    public virtual DbSet<m_service_provider> m_service_providers { get; set; }

    public virtual DbSet<m_service_type> m_service_types { get; set; }

    public virtual DbSet<m_task_status> m_task_statuses { get; set; }

    public virtual DbSet<m_user> m_users { get; set; }

    public virtual DbSet<m_user_client> m_user_clients { get; set; }

    public virtual DbSet<m_user_provider> m_user_providers { get; set; }

    public virtual DbSet<t_task_contact> t_task_contacts { get; set; }

    public virtual DbSet<t_task_message> t_task_messages { get; set; }

    public virtual DbSet<t_task_request> t_task_requests { get; set; }

    public virtual DbSet<t_task_score> t_task_scores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=VALENTINA\\SQLEXPRESS;Database=JOB_MATCH;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<m_doc_type>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<m_postal_code>(entity =>
        {
            entity.ToTable("m_postal_code", tb => tb.HasTrigger("trg_upd_m_postal_code_lastupdate"));

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<m_service>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.id_service_typeNavigation).WithMany(p => p.m_services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_service_m_service_type");
        });

        modelBuilder.Entity<m_service_type>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<m_service_provider>(entity =>
        {
            entity.ToTable("m_service_provider");
        });

        modelBuilder.Entity<m_task_status>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<m_user>(entity =>
        {
            entity.ToTable("m_user", tb => tb.HasTrigger("trg_upd_m_user_lastupdate"));

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<m_user_client>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.id_doc_typeNavigation).WithMany(p => p.m_user_clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_client_m_doc_type");

            entity.HasOne(d => d.id_postal_codeNavigation).WithMany(p => p.m_user_clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_client_m_postal_code");

            entity.HasOne(d => d.id_userNavigation).WithMany(p => p.m_user_clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_client_m_user");
        });

        modelBuilder.Entity<m_user_provider>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_m_data_provider");

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.id_doc_typeNavigation).WithMany(p => p.m_user_providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_provider_m_doc_type");

            entity.HasOne(d => d.id_postal_codeNavigation).WithMany(p => p.m_user_providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_provider_m_postal_code");

            entity.HasOne(d => d.id_userNavigation).WithMany(p => p.m_user_providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_m_user_provider_m_user");
        });

        modelBuilder.Entity<t_task_contact>(entity =>
        {
            entity.HasOne(d => d.id_task_requestNavigation).WithMany(p => p.t_task_contacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_contact_t_task_request");

            entity.HasOne(d => d.id_user_providerNavigation).WithMany(p => p.t_task_contacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_contact_m_user_provider");
        });

        modelBuilder.Entity<t_task_message>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.id_task_requestNavigation).WithMany(p => p.t_task_messages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_message_t_task_request");

            entity.HasOne(d => d.id_user_reciverNavigation).WithMany(p => p.t_task_messageid_user_reciverNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_message_m_user_reciver");

            entity.HasOne(d => d.id_user_senderNavigation).WithMany(p => p.t_task_messageid_user_senderNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_message_m_user_sender");
        });

        modelBuilder.Entity<t_task_request>(entity =>
        {
            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.id_postal_codeNavigation).WithMany(p => p.t_task_requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_request_m_postal_code");

            entity.HasOne(d => d.id_serviceNavigation).WithMany(p => p.t_task_requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_request_m_service");

            entity.HasOne(d => d.id_statusNavigation).WithMany(p => p.t_task_requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_request_m_task_status");

            entity.HasOne(d => d.id_user_clientNavigation).WithMany(p => p.t_task_requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_request_m_user_client");
        });

        modelBuilder.Entity<t_task_score>(entity =>
        {
            entity.HasOne(d => d.id_task_requestNavigation).WithMany(p => p.t_task_scores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_score_t_task_request");

            entity.HasOne(d => d.id_user_reviewedNavigation).WithMany(p => p.t_task_scoreid_user_reviewedNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_score_m_user_reviewed");

            entity.HasOne(d => d.id_user_reviewerNavigation).WithMany(p => p.t_task_scoreid_user_reviewerNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_t_task_score_m_user_reviewer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
