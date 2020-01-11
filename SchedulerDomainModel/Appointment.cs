using System;

namespace SchedulerDomainModel
{
    public class Appointment 
    {
        public Appointment() { }

        public int ID { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}