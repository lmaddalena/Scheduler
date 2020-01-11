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
    public class RemindersController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger _logger;

        // costructor
        public RemindersController(IAppointmentRepository appointmentRepository, ILogger<RemindersController> logger)
        {
            _appointmentRepository = appointmentRepository;    
            _logger = logger;
        }
        
        // GET appointments/reminders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation(0, "Get reminder with id: {0}", id);

            if(!ModelState.IsValid)
                return BadRequest();

            var data = await _appointmentRepository.GetReminderByIdAsync(id);

            if(data == null)
                return this.NotFound();
            else
            {
                // map data to the domain entity
                SchedulerDomainModel.Reminder r = Mappers.DataModelToEntity.MapReminder(data);
               
                return this.Ok(r);
            }
        }

        // POST appointments/reminders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Reminder r)
        {
            _logger.LogInformation(0, "Add new reminder");

            // check the model state
            if(!ModelState.IsValid)
                return BadRequest();
            
            // add reminder to the repository
            var data = _appointmentRepository.Add(
                r.Title, 
                r.Description, 
                r.DateAndTime, 
                SchedulerData.DataModel.AppointmentTypeEnum.Reminder, 
                SchedulerData.DataModel.RecurrencyTypeEnum.One_Off,
                r.IsDone
            );

            // save changes
            await _appointmentRepository.SaveAsync();
            r.ID = data.AppointmentID;

            _logger.LogInformation(0, "Reminder added. ID: ", r.ID);

            // return 
            return CreatedAtAction(nameof(Get), new { ID = r.ID}, r);
        }


    }
}