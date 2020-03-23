using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulerData.Repository;
using SchedulerDomainModel;

namespace SchedulerApi.Controllers
{
    [Route("appointments/[controller]")]
    public class MeetingsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAttendeeRepository _attendeeRepository;
        private readonly ILogger _logger;

        // costructor
        public MeetingsController(IAppointmentRepository appointmentRepository, IAttendeeRepository attendeeRepository, ILogger<RemindersController> logger)
        {
            _appointmentRepository = appointmentRepository; 
            _attendeeRepository = attendeeRepository;
            _logger = logger;
        }

        // GET appointments/meetings/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<SchedulerDomainModel.Meeting>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation(0, "Get meeting with id: {0}", id);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataMeeting = await _appointmentRepository.GetMeetingByIdAsync(id);

            if(dataMeeting == null)
                return this.NotFound();
            else
            {
                // map meeting to the domain entity
                SchedulerDomainModel.Meeting m = Mappers.DataModelToEntity.MapMeeting(dataMeeting);

                // get attendees
                var dataAttendees = await _attendeeRepository.GetByAppoinmentIdAsync(id);

                // map attendees to the domain entity
                m.Attendees = Mappers.DataModelToEntity.MapAttendees(dataAttendees);
                
                return this.Ok(m);
            }
        }
        
        // POST appointments/meetings
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Meeting m)
        {
            _logger.LogInformation(0, "Add new meeting");

            // check the model state
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // add reminder to the repository
            var data = _appointmentRepository.Add(
                m.Title, 
                m.Description, 
                m.DateAndTime, 
                SchedulerData.DataModel.AppointmentTypeEnum.Meeting, 
                (m.RecurrencyType == SchedulerDomainModel.RecurrencyTypeEnum.One_Off ? SchedulerData.DataModel.RecurrencyTypeEnum.One_Off : SchedulerData.DataModel.RecurrencyTypeEnum.Weekly),
                false                                
            );

            data.Attendees = new List<SchedulerData.DataModel.Attendee>();

            foreach(var a in m.Attendees)
            {
                data.Attendees.Add(new SchedulerData.DataModel.Attendee(){ AttendeeID = a.AttendeeID, Name = a.Name });
            }

            // save changes
            await _appointmentRepository.SaveAsync();
            m.ID = data.AppointmentID;

            _logger.LogInformation(0, "Meeting added. ID: ", m.ID);

            // return 
            return CreatedAtAction(nameof(Get), new { ID = m.ID}, m);
        }

        // PATCH appointments/meetings/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMeeting(int id, [FromBody] List<Attendee> attendees)
        {
            _logger.LogInformation(0, "Patch attendees in meeting with id: {0}", id);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dataMeeting = await _appointmentRepository.GetMeetingByIdAsync(id);

            if(dataMeeting == null)
                return this.NotFound();
            else
            {
                _attendeeRepository.RemoveByAppoinmentIdAsync(id);
                
                foreach(var a in attendees)
                {
                    dataMeeting.Attendees.Add(new SchedulerData.DataModel.Attendee(){ AttendeeID = a.AttendeeID, Name = a.Name });
                }

                await _appointmentRepository.SaveAsync();

                return this.Ok();
            }

        }

    }
}