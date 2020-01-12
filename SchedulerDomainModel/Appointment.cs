using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerDomainModel
{
    public class Appointment 
    {
        public Appointment() { }

        public int ID { get; set; }
        
        [Required]
        public DateTime DateAndTime { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Title { get; set; }
        
        [StringLength(255, ErrorMessage = "{0} length can't be more than {1}.")]
        public string Description { get; set; }

    }
}