using System;

namespace SchedulerDomainModel
{
    public class Reminder : Appointment
    {
        public Reminder() {}

        public bool IsDone { get; set; }
    }
}