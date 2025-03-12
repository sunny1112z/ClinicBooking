using System;
using System.Collections.Generic;

namespace ClinicBooking.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? GenderId { get; set; }

    public string? Address { get; set; }

    public string? NationalId { get; set; }

    public string? BloodType { get; set; }

    public string? Avatar { get; set; }

    public int RoleId { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role Role { get; set; } = null!;
}
