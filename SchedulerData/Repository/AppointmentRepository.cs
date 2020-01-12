using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerData.DataModel;

namespace SchedulerData.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        // private variables
        private SchedulerContext _dataContext;

        // costructor
        public AppointmentRepository(SchedulerContext dataContext)
        {
            _dataContext = dataContext;
        }

        // add new appointment in the repository
        public Appointment Add(string title, string description, DateTime reminderDateTime, AppointmentTypeEnum appointmentType, RecurrencyTypeEnum recurrencyType, bool isDone)
        {
            Appointment app = new Appointment() {
                Title = title,
                Description = description,
                AppointmentDateTime = reminderDateTime,
                AppointmentType = appointmentType,
                RecurrencyType = recurrencyType,
                IsDone = isDone
            };

            _dataContext.Appointments.Add(app);
            return app;
        }

        // get an appointment by id
        public async Task<Appointment> GetByIdAsync(int id)
        {
            var app = from a in _dataContext.Appointments
                        where a.AppointmentID == id
                        select a;

            return await app.SingleOrDefaultAsync();

        }

        // get appointments by date
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime appointmentDate)
        {
            var app = from a in _dataContext.Appointments
                        where a.AppointmentDateTime.Date == appointmentDate.Date ||
                        (
                            a.RecurrencyType == RecurrencyTypeEnum.Weekly &&
                            appointmentDate.Date > a.AppointmentDateTime.Date &&
                            appointmentDate.Date.DayOfWeek == a.AppointmentDateTime.Date.DayOfWeek
                        )
                        select a;

            return await app.ToListAsync();

        }

        // get appointments by attendeeId and date
        public async Task<IEnumerable<Appointment>> GetAppointmentsByAttendeeIdAndDateAsync(string attendeeId, DateTime appointmentDate)
        {
            var appointmentIds = from att in _dataContext.Attendees
                                 where att.AttendeeID == attendeeId
                                 select att.AppointmentID;
            
            var appointments =  from a in _dataContext.Appointments
                                where a.AppointmentDateTime.Date == appointmentDate.Date ||
                                (
                                    a.RecurrencyType == RecurrencyTypeEnum.Weekly &&
                                    appointmentDate.Date > a.AppointmentDateTime.Date &&
                                    appointmentDate.Date.DayOfWeek == a.AppointmentDateTime.Date.DayOfWeek
                                ) &&
                                appointmentIds.Contains(a.AppointmentID)
                                select a;

            return await appointments.ToListAsync();
        }

        // get a reminder by id
        public async Task<Appointment> GetReminderByIdAsync(int id)
        {
            var app = from a in _dataContext.Appointments
                        where a.AppointmentID == id && a.AppointmentType == AppointmentTypeEnum.Reminder
                        select a;

            return await app.SingleOrDefaultAsync();

        }

        // get a meeting by id
        public async Task<Appointment> GetMeetingByIdAsync(int id)
        {
            var app = from a in _dataContext.Appointments
                      where a.AppointmentID == id && a.AppointmentType == AppointmentTypeEnum.Meeting
                      select a;

            return await app.SingleOrDefaultAsync();

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