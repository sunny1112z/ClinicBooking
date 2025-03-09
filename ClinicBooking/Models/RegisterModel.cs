using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Username { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }
}
