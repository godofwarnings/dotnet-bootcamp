using ConsoleApp6;
using System;
using static ConsoleApp6.HospitalService;

public class Program
{
    public static void Main()
    {
        try
        {
            Doctor newDoctor = new Doctor
            {
                DoctorName = "Dr. Sam Smith",
                Specialization = "Cardiology",
                ConsultationFee = 15000,
                IsAvailable = true
            };

            Patient newPatient = new Patient
            {
                PatientName = "Amit Mishra",
                Age = 18,
                Gender = "Male",
                City = "Mumbai",
                PhoneNumber = "9000000007"
            };

            //DisplayAllDoctors();
            //DisplayAllPatients();
            //DisplayAllAppointments();
            //DisplayAvailableDoctors();
            //DisplayDoctorsBySpecialization("Cardiology");
            //DisplayPatientByCity("Hyderabad");
            //DisplayDoctorById(4);
            //InsertDoctor(newDoctor);
            //InsertPatient(newPatient);
            //UpdateDoctorFee(6, 1500);
            //UpdatePatientCity(6, "Gujarat");
            //UpdateDoctorToUnavailable(6);
            DeletePatient(3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
