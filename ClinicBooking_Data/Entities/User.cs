using BCrypt.Net;
using ClinicBooking_Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Entities;

public partial class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Phone { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string PasswordHash { get; set; } = null!;

    public int? GenderId { get; set; }

    public string? Address { get; set; }

    public string? NationalId { get; set; }

    public int? CityId { get; set; }

    public string? BloodType { get; set; }

    public string? AdditionalInfo { get; set; }

    public string? Avatar { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }


    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual City? City { get; set; }

    public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
