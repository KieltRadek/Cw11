using Cwiczenie11.DTOs;

namespace Cwiczenie11.Services;

public interface IPatientService
{
    Task<PatientWithPrescriptionsDto> GetWithPrescriptionsAsync(int patientId);
}