using System;
using System.Collections.Generic;

namespace ClinicBooking.Entities;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Status { get; set; }

    public string? ZoomInfo { get; set; }

    public string? MedicalResult { get; set; }

    public int UserId { get; set; }

    public int DoctorId { get; set; }

    public int? ScheduleID { get; set; }  

   
    public virtual Doctor Doctor { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual WorkSchedule? WorkSchedule { get; set; } 

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
