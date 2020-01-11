using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerData.DataModel;

namespace SchedulerData.Repository
{
    public interface IAttendeeRepository
    {
        Task<IEnumerable<Attendee>> GetByAppoinmentIdAsync(int appointmentId);
        void RemoveByAppoinmentIdAsync(int appointmentId);
        void Save();
        Task<int> SaveAsync();
        
    }

}