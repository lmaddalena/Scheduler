using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerData.DataModel;

namespace SchedulerData.Repository
{
    public interface IAppointmentRepository
    {
        Appointment Add(string title, string description, DateTime reminderDateTime, AppointmentTypeEnum appointmentType, RecurrencyTypeEnum recurrencyType, bool isDone);
        Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime appointmentDate);
        Task<IEnumerable<Appointment>> GetAppointmentsByAttendeeIdAndDateAsync(string attendeeId, DateTime appointmentDate);
        Task<Appointment> GetByIdAsync(int id);
        Task<Appointment> GetReminderByIdAsync(int id);
        Task<Appointment> GetMeetingByIdAsync(int id);
        void Save();
        Task<int> SaveAsync();
        
    }

}