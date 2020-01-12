using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchedulerData.Repository;
using SchedulerDomainModel;

namespace SchedulerApi.Controllers
{
    [Route("[controller]")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly ILogger _logger;

        // costructor
        public AppointmentsController(IAppointmentRepository appointmentRepository, IAttendeeRepository attendeeRepository, ILogger<RemindersController> logger)
        {
            _appointmentRepository = appointmentRepository;    
            _attendeeRepository = attendeeRepository;
            _logger = logger;
        }
        
        // PATCH appointments/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAppointment(int id, [FromBody] DateTime appointmentDateTime)
        {
            _logger.LogInformation(0, "Patch appointment with id: {0}, new appointment date: {1}", id, appointmentDateTime);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _appointmentRepository.GetByIdAsync(id);

            if(data == null)
                return this.NotFound();
            else
            {
                data.AppointmentDateTime = appointmentDateTime;
                await _appointmentRepository.SaveAsync();

                Appointment app = new Appointment() {
                    ID = data.AppointmentID,
                    DateAndTime = data.AppointmentDateTime,
                    Title = data.Title,
                    Description = data.Description
                };

                return this.Ok(app);
            }

        }

        // GET appointments/2020-03-16
        [HttpGet("{appointmentDate}")]
        public async Task<IActionResult> Get(DateTime appointmentDate)
        {
            _logger.LogInformation(0, "Get appointments at: {0}", appointmentDate);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _appointmentRepository.GetAppointmentsByDateAsync(appointmentDate);            
            
            if(data == null)
                return this.NotFound();
            else
            {
                List<SchedulerDomainModel.Reminder> reminders = new List<Reminder>();
                List<SchedulerDomainModel.Meeting> meetings = new List<Meeting>();

                foreach(var a in data)
                {
                    if(a.AppointmentType == SchedulerData.DataModel.AppointmentTypeEnum.Reminder)
                    {
                        reminders.Add(Mappers.DataModelToEntity.MapReminder(a));
                    }
                    else
                    {
                        SchedulerDomainModel.Meeting m = Mappers.DataModelToEntity.MapMeeting(a);
                        m.Attendees = Mappers.DataModelToEntity.MapAttendees(await _attendeeRepository.GetByAppoinmentIdAsync(a.AppointmentID));
                        meetings.Add(m);
                    }


                }
                
                return this.Ok(new {reminders, meetings});
            }
        }

        // GET appointments/jo@gmail.com/2020-03-16
        [HttpGet("{attendeeId}/{appointmentDate}")]
        public async Task<IActionResult> Get(string attendeeId, DateTime appointmentDate)
        {
            _logger.LogInformation(0, "Get appointments for attendee: {0} at: {1}", attendeeId, appointmentDate);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _appointmentRepository.GetAppointmentsByAttendeeIdAndDateAsync(attendeeId, appointmentDate);            
            
            if(data == null)
                return this.NotFound();
            else
            {
                List<SchedulerDomainModel.Reminder> reminders = new List<Reminder>();
                List<SchedulerDomainModel.Meeting> meetings = new List<Meeting>();

                foreach(var a in data)
                {
                    if(a.AppointmentType == SchedulerData.DataModel.AppointmentTypeEnum.Reminder)
                    {
                        reminders.Add(Mappers.DataModelToEntity.MapReminder(a));
                    }
                    else
                    {
                        SchedulerDomainModel.Meeting m = Mappers.DataModelToEntity.MapMeeting(a);
                        m.Attendees = Mappers.DataModelToEntity.MapAttendees(await _attendeeRepository.GetByAppoinmentIdAsync(a.AppointmentID));
                        meetings.Add(m);
                    }


                }
                
                return this.Ok(new {reminders, meetings});
            }
        }

    }
}