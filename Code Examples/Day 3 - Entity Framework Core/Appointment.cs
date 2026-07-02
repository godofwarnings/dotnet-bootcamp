using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp6
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "";
        public decimal AmountPaid { get; set; }
    }
}
