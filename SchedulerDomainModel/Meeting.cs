using System;
using System.Collections.Generic;

namespace SchedulerDomainModel
{
    public class Meeting : Appointment
    {
        public Meeting() { }

        public RecurrencyTypeEnum RecurrencyType { get; set; }

        public IEnumerable<Attendee> Attendees { get; set; }

    }
}