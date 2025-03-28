using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicBooking.Entities
{
    public class WorkSchedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime WorkDate { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int Status { get; set; } = 1;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation properties
        public virtual Doctor? Doctor { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
