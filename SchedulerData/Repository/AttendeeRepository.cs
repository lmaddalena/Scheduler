using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerData.DataModel;

namespace SchedulerData.Repository
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private SchedulerContext _dataContext;

        public AttendeeRepository(SchedulerContext dataContext)
        {
            _dataContext = dataContext;
        }

        // get all attendees of specified appointment
        public async Task<IEnumerable<Attendee>> GetByAppoinmentIdAsync(int appointmentId)
        {
            var attendees = from a in _dataContext.Attendees
                            where a.AppointmentID == appointmentId
                            orderby a.Name
                            select a;
            
            return await attendees.ToListAsync();

        }

        // remove all Attendees by appoinment id
        public void RemoveByAppoinmentIdAsync(int appointmentId)
        {
            var attendees = from a in _dataContext.Attendees
                            where a.AppointmentID == appointmentId
                            select a;
            
            foreach(var a in attendees)
                _dataContext.Attendees.Remove(a);
        }

        // save changes
        public void Save()
        {
            _dataContext.SaveChanges();
        }

        // save changes asynchronly
        public async Task<int> SaveAsync()
        {
            // EF Core does not support multiple parallel operations being run on the same context instance. 
            // You should always wait for an operation to complete before beginning the next operation. 
            // This is typically done by using the await keyword on each asynchronous operation.

            return await _dataContext.SaveChangesAsync();
        }

    }
}