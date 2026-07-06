using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; // I dont think this is required.

namespace ConsoleApp6{

    public class HospitalDbContext : DbContext{
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}