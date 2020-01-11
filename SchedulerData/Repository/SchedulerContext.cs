using Microsoft.EntityFrameworkCore;
using SchedulerData.DataModel;

namespace SchedulerData.Repository
{
    public class SchedulerContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Attendee> Attendees { get; set; }

        private string _connectionString = null;

        public SchedulerContext() : base()
        {

        }

        public SchedulerContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public SchedulerContext(DbContextOptions<SchedulerContext> options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(string.IsNullOrEmpty(this._connectionString))
                optionsBuilder.UseSqlite("Data Source=scheduler.db");
            else
                optionsBuilder.UseSqlite(this._connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>()
                .HasKey(a => new { a.AttendeeID, a.AppointmentID });
        }     
    }
}