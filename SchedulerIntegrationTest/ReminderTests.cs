using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using SchedulerDomainModel;

namespace SchedulerIntegrationTest
{
    [TestClass]
    public class ReminderTests
    {
        private string _base_url = "https://localhost:5001";

        [TestMethod]
        public void AddReminderTest()
        {
            using (var client = new HttpClient())
            {
                string title = "Integration Test " + DateTime.Now.Ticks;
                string description = "My new Reminder";
                DateTime dateAndTime = DateTime.Now.AddDays(10);
                bool isDone = false;            

                Reminder r = new Reminder()
                {
                    Title = title,
                    Description = description,
                    DateAndTime = dateAndTime,
                    IsDone = isDone
                };

                // json serialization
                string s = JsonConvert.SerializeObject(r);

                // content to be sent
                var content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");

                // Post call and get the response
                HttpResponseMessage res = client.PostAsync(_base_url + "/appointments/reminders", content).Result;

                // assertion
                Assert.IsNotNull(res, "Response is null");
                Assert.IsTrue(res.IsSuccessStatusCode, "Status code: " + res.StatusCode);

                // content of the response
                s = res.Content.ReadAsStringAsync().Result;

                // deserialize the reposne
                Reminder r2 = JsonConvert.DeserializeObject<Reminder>(s);

                // assertion
                Assert.IsNotNull(r2, "Response doesen't contain a reminder object");
                Assert.IsTrue(r2.ID > 0, "The id is not correctly valorized");
            }
        }
    }
}
