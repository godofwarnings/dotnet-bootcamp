using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp6
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = "";
        public string Specialization { get; set; } = "";
        public decimal ConsultationFee { get; set; }
        public bool IsAvailable { get; set; }
    }
}
