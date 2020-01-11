using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerData.DataModel
{
    public class Attendee 
    {
        public Attendee() { }

        public int AppointmentID { get; set; }
        
        [EmailAddress(ErrorMessage = "Invalid EMail Address")]
        [StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
        public string AttendeeID { get; set; }

        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}