using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SchedulerData.DataModel
{
    public class Appointment 
    {
        public Appointment() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
        public string Description { get; set; }

        [Required]
        public RecurrencyTypeEnum RecurrencyType { get; set; }

        public virtual ICollection<Attendee> Attendees { get; set; }

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public AppointmentTypeEnum AppointmentType { get; set; }

    }
}