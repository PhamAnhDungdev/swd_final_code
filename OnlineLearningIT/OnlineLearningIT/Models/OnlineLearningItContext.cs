using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineLearningIT.Models;

public partial class OnlineLearningItContext : DbContext
{
    public OnlineLearningItContext()
    {
    }

    public OnlineLearningItContext(DbContextOptions<OnlineLearningItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCertificate> UserCertificates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =MSI; database = OnlineLearningIT;uid=sa;pwd=123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PK__Certific__E2256D318919DC19");

            entity.ToTable("Certificate");

            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MinCompletionPercentage)
                .HasDefaultValue(100)
                .HasColumnName("min_completion_percentage");
            entity.Property(e => e.TemplateUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("template_url");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Course).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Certifica__cours__37A5467C");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__8F1EF7AE53F6E7E3");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.InstructorName)
                .HasMaxLength(100)
                .HasColumnName("instructor_name");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("draft")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lesson__6421F7BE874F7FBE");

            entity.ToTable("Lesson");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.ContentType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("content_type");
            entity.Property(e => e.ContentUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("content_url");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.IsFree)
                .HasDefaultValue(false)
                .HasColumnName("is_free");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Lesson__course_i__30F848ED");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F2553042D");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserCertificate>(entity =>
        {
            entity.HasKey(e => e.UserCertificateId).HasName("PK__UserCert__B0F97AA18BE94AFE");

            entity.ToTable("UserCertificate");

            entity.HasIndex(e => e.VerificationCode, "UQ__UserCert__218EA4357A898290").IsUnique();

            entity.Property(e => e.UserCertificateId).HasColumnName("user_certificate_id");
            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.CertificateUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("certificate_url");
            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("issue_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("verification_code");

            entity.HasOne(d => d.Certificate).WithMany(p => p.UserCertificates)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCerti__certi__3D5E1FD2");

            entity.HasOne(d => d.User).WithMany(p => p.UserCertificates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserCerti__user___3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
