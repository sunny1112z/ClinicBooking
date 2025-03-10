using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Models
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
