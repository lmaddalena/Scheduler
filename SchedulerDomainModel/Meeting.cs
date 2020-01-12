using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulerDomainModel
{
    public class Meeting : Appointment
    {
        public Meeting() { }

        [Required]
        public RecurrencyTypeEnum RecurrencyType { get; set; }

        public IEnumerable<Attendee> Attendees { get; set; }

    }
}