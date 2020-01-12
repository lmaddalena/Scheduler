using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulerDomainModel
{
    public class Reminder : Appointment
    {
        public Reminder() {}

        [Required]
        public bool IsDone { get; set; }
    }
}