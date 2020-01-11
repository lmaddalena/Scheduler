using System;
using System.Collections.Generic;
using System.Linq;
using SchedulerData.DataModel;


namespace SchedulerApi.Controllers.Mappers
{
    public class DataModelToEntity
    {
        public static SchedulerDomainModel.Meeting MapMeeting(Appointment data)
        {
            if(data == null)
                return null;

            SchedulerDomainModel.Meeting m = new SchedulerDomainModel.Meeting() {
                ID = data.AppointmentID,
                Title = data.Title,
                Description = data.Description,
                DateAndTime = data.AppointmentDateTime,
                RecurrencyType = (data.RecurrencyType == SchedulerData.DataModel.RecurrencyTypeEnum.One_Off ? SchedulerDomainModel.RecurrencyTypeEnum.One_Off : SchedulerDomainModel.RecurrencyTypeEnum.Weekly)
            };

            return m;
        }

        public static IEnumerable<SchedulerDomainModel.Meeting> MapMeetings(IEnumerable<Appointment> data)
        {
            if(data == null)
                return null;

            List<SchedulerDomainModel.Meeting> meetings = new List<SchedulerDomainModel.Meeting>();

            foreach(var m in data)
            {
                meetings.Add(MapMeeting(m));
            }

            return meetings;
        }

        public static SchedulerDomainModel.Reminder MapReminder(Appointment data)
        {
            if(data == null)
                return null;

            SchedulerDomainModel.Reminder r = new SchedulerDomainModel.Reminder() {
                ID = data.AppointmentID,
                Title = data.Title,
                Description = data.Description,
                DateAndTime = data.AppointmentDateTime,
                IsDone = data.IsDone
            };
            return r;
        }

        public static IEnumerable<SchedulerDomainModel.Reminder> MapReminder(IEnumerable<Appointment> data)
        {
            if(data == null)
                return null;

            List<SchedulerDomainModel.Reminder> reminders = new List<SchedulerDomainModel.Reminder>();

            foreach(var r in data)
            {
                reminders.Add(MapReminder(r));
            }

            return reminders;

        }

        public static SchedulerDomainModel.Attendee MapAttendee(Attendee data)
        {
            if(data == null)
                return null;

            SchedulerDomainModel.Attendee a = new SchedulerDomainModel.Attendee() {
                AttendeeID = data.AttendeeID, 
                Name = data.Name
            };

            return a;
        }
        public static IEnumerable<SchedulerDomainModel.Attendee> MapAttendees(IEnumerable<Attendee> data)
        {
            if(data == null)
                return null;

            List<SchedulerDomainModel.Attendee> attendees = new List<SchedulerDomainModel.Attendee>();

            foreach(var a in data)
            {
                attendees.Add(MapAttendee(a));
            }

            return attendees;

        }

    }
}