using System;
using System.Collections.Generic;
using System.Linq;
// using Microsoft.EntityFrameworkCore;

namespace ConsoleApp6;

// static class because class is the object itself.
public static class HospitalService
{
    
    public static void DisplayAllDoctors()
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Doctor> doctors = context.Doctors.ToList();
            foreach (Doctor doctor in doctors) { PrintDoctor(doctor); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while displaying all doctors: {ex.Message}");
        }
    }

    public static void DisplayAllPatients()
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Patient> patients = context.Patients.ToList();
            foreach (Patient patient in patients) { PrintPatient(patient); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while displaying all patients: {ex.Message}");
        }
    }

    public static void DisplayAllAppointments()
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Appointment> appointments = context.Appointments.ToList();
            foreach (Appointment appointment in appointments) { PrintAppointment(appointment); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while displaying all appointments: {ex.Message}");
        }
    }

    public static void DisplayAvailableDoctors()
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Doctor> availableDoctors = context.Doctors.Where(doc => doc.IsAvailable == true).ToList();
            foreach (Doctor doctor in availableDoctors) { PrintDoctor(doctor); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for available doctors: {ex.Message}");
        }
    }

    public static void DisplayDoctorsBySpecialization(string specialization)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Doctor> doctors = context.Doctors.Where(doc => doc.Specialization == specialization).ToList();
            foreach (Doctor doctor in doctors) { PrintDoctor(doctor); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for doctors specialized in {specialization}: {ex.Message}");
        }
    }

    public static void DisplayPatientByCity(string city)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            List<Patient> patients = context.Patients.Where(p => p.City == city).ToList();
            foreach (Patient patient in patients) { PrintPatient(patient); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while displaying patients in the city of {city}: {ex.Message}");
        }
    }

    public static void DisplayDoctorById(int id)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.DoctorId == id);
            if (doctor != null) { Console.WriteLine(); Console.WriteLine("Doctor Found!"); PrintDoctor(doctor); }
            else { Console.WriteLine("Doctor with ID {0} not found", id); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for doctors with id {id}: {ex.Message}");
        }
    }

    public static void InsertDoctor(Doctor doctor)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            context.Doctors.Add(doctor);
            context.SaveChanges();
            Console.WriteLine("Doctor inserted Successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not insert a new doctor: {0}", ex.Message);
        }
    }

    public static void InsertPatient(Patient patient)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            context.Patients.Add(patient);
            context.SaveChanges();
            Console.WriteLine("Patient inserted Successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not insert a new patient: {0}", ex.Message);
        }
    }

    public static void UpdateDoctorFee(int id, decimal fee)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.DoctorId == id);
            if (doctor != null) { Console.WriteLine(); Console.WriteLine("Doctor Found!"); doctor.ConsultationFee = fee; context.SaveChanges(); PrintDoctor(doctor); }
            else { Console.WriteLine("Doctor with ID {0} not found", id); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for doctors with id {id}: {ex.Message}");
        }
    }

    public static void UpdateDoctorToUnavailable(int id)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.DoctorId == id);
            if (doctor != null) { Console.WriteLine(); Console.WriteLine("Doctor Found!"); if (doctor.IsAvailable == false) { Console.WriteLine("Doctor is already unavailable."); } else { doctor.IsAvailable = false; context.SaveChanges(); PrintDoctor(doctor); } }
            else { Console.WriteLine("Doctor with ID {0} not found", id); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for doctors with id {id}: {ex.Message}");
        }
    }

    public static void UpdatePatientCity(int id, string city)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            Patient? patient = context.Patients.FirstOrDefault(p => p.PatientId == id);
            if (patient != null) { Console.WriteLine(); Console.WriteLine("Patient Found!"); patient.City = city; context.SaveChanges(); PrintPatient(patient); }
            else { Console.WriteLine("Patient with ID {0} not found", id); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while checking for patients with id {id}: {ex.Message}");
        }
    }

    public static void DeletePatient(int id)
    {
        try
        {
            using HospitalDbContext context = new HospitalDbContext();
            Patient? patient = context.Patients.Find(id);
            if (patient == null) { Console.WriteLine("Patient not found."); return; }
            bool hasScheduledAppointment = context.Appointments.Any(a => a.PatientId == id && a.Status == "Scheduled");
            if (hasScheduledAppointment) { Console.WriteLine("Patient has a scheduled appointment and cannot be deleted."); return; }
            context.Patients.Remove(patient);
            context.SaveChanges();
            Console.WriteLine("Patient removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting patient with id {id}: {ex.Message}");
        }
    }

    private static void PrintDoctor(Doctor doctor)
    {
        Console.WriteLine($"{doctor.DoctorId} - {doctor.DoctorName} - {doctor.Specialization} - {doctor.ConsultationFee} - Available: {doctor.IsAvailable}");
    }

    private static void PrintPatient(Patient patient)
    {
        Console.WriteLine($"{patient.PatientId} - {patient.PatientName} - Age: {patient.Age} - {patient.Gender} - {patient.City} - {patient.PhoneNumber}");
    }

    private static void PrintAppointment(Appointment appointment)
    {
        Console.WriteLine($"{appointment.AppointmentId} - Patient ID: {appointment.PatientId} - Doctor ID: {appointment.DoctorId} - Date: {appointment.AppointmentDate} - Status: {appointment.Status} - Paid: {appointment.AmountPaid}");
    }
}
