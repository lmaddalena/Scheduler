using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using SchedulerData.Repository;
using SchedulerData.DataModel;
using System.Threading.Tasks;

namespace SchedulerUnitTest
{
    [TestClass]
    public class DataLayerUnitTest
    {
        private string _connectionString = "Data Source=..\\..\\..\\scheduler.db";

        private SchedulerContext GetDataContext()
        {
            return new SchedulerContext(_connectionString);
        }

        [TestMethod]
        public void AddAppointmentTest()
        {
            // get the data context
            var dc = this.GetDataContext();

            // create the repository instance
            IAppointmentRepository rep = new AppointmentRepository(dc);

            string title = "Test " + DateTime.Now.Ticks;
            string description = "My new Appointment";
            DateTime appDate = DateTime.Now;
            AppointmentTypeEnum appType = AppointmentTypeEnum.Meeting;
            RecurrencyTypeEnum recType = RecurrencyTypeEnum.Weekly;
            bool isDone = false;            

            // add new appointment in the repository
            Appointment app = rep.Add(
                                    title,
                                    description,
                                    appDate,
                                    appType,
                                    recType,
                                    isDone);


            // save changes
            rep.Save();

            Assert.IsNotNull(app, "Appointment is null");
            Assert.IsFalse(app.AppointmentID == 0, "AppointmentID is 0, expected > 0");            
            Assert.AreEqual(title, app.Title);
            Assert.AreEqual(description, app.Description);
            Assert.AreEqual(appDate, app.AppointmentDateTime);
            Assert.AreEqual(appType, app.AppointmentType);
            Assert.AreEqual(recType, app.RecurrencyType);
            Assert.AreEqual(isDone, app.IsDone);

            // dispose the data context
            dc.Dispose();
        }

        
        
        [TestMethod]
        public void GetRemainderByIdTest()
        {
            // get the data context
            var dc = this.GetDataContext();

            // create the repository instance
            IAppointmentRepository rep = new AppointmentRepository(dc);

            string title = "Test " + DateTime.Now.Ticks;
            string description = "My new Reminder";
            DateTime appDate = DateTime.Now;
            AppointmentTypeEnum appType = AppointmentTypeEnum.Reminder;
            RecurrencyTypeEnum recType = RecurrencyTypeEnum.One_Off;
            bool isDone = false;            

            // add new appointment in the repository
            Appointment app = rep.Add(
                                    title,
                                    description,
                                    appDate,
                                    appType,
                                    recType,
                                    isDone);


            // save changes
            rep.Save();

            var task = rep.GetReminderByIdAsync(app.AppointmentID);

            Assert.IsNotNull(task.Result, "Reminder is null");
            Assert.AreEqual(app.AppointmentID, task.Result.AppointmentID);

            dc.Dispose();

        }
        
    }
}
