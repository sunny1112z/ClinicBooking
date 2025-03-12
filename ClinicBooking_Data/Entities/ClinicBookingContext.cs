using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Entities;

public partial class ClinicBookingContext : DbContext
{
    public ClinicBookingContext()
    {
    }

    public ClinicBookingContext(DbContextOptions<ClinicBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<RatingLevel> RatingLevels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ClinicBookingDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2099114B8");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.MedicalResult).HasMaxLength(500);
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Subject).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ZoomInfo).HasMaxLength(255);

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Docto__3B75D760");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__UserI__3A81B327");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCDE7675A50");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDFFA7C982B");

            entity.HasIndex(e => e.Username, "UQ__Doctors__536C85E483474454").IsUnique();

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfileImage).HasMaxLength(255);
            entity.Property(e => e.Qualification).HasMaxLength(100);
            entity.Property(e => e.RoleId)
                .HasDefaultValue(2)
                .HasColumnName("RoleID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.ZoomInfo).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Doctors__Departm__35BCFE0A");

            entity.HasOne(d => d.Role).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctors__RoleID__36B12243");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.Faqid).HasName("PK__FAQ__4B89D1E29E2A5744");

            entity.ToTable("FAQ");

            entity.Property(e => e.Faqid).HasColumnName("FAQID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.SentDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Faqs)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__FAQ__DoctorID__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.Faqs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FAQ__UserID__3F466844");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Gender__4E24E8171CF41534");

            entity.ToTable("Gender");

            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.GenderName).HasMaxLength(10);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD3B94AF49A");

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.PublishDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF85C33C569E4");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.RatingLevelId).HasColumnName("RatingLevelID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Ratings__Appoint__49C3F6B7");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Ratings__DoctorI__48CFD27E");

            entity.HasOne(d => d.RatingLevel).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RatingLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ratings__RatingL__46E78A0C");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ratings__UserID__47DBAE45");
        });

        modelBuilder.Entity<RatingLevel>(entity =>
        {
            entity.HasKey(e => e.RatingLevelId).HasName("PK__RatingLe__36EFBE87BFC3C0C0");

            entity.Property(e => e.RatingLevelId).HasColumnName("RatingLevelID");
            entity.Property(e => e.RatingDescription).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3ADDD1ACDB");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC6B4872FA");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4CFCEDE58").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Users__5C7E359E7877752B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053477164BD1").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Users__E9AA321AE21C3351").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.BloodType).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.NationalId)
                .HasMaxLength(20)
                .HasColumnName("NationalID");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ResetToken).HasMaxLength(255);
            entity.Property(e => e.ResetTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Gender).WithMany(p => p.Users)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("FK__Users__GenderID__2D27B809");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
