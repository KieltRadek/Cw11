using Cw11.DTOs;
namespace Cw11.Interfaces;

public interface IPrescriptionService
{
    Task<int> CreatePrescriptionAsync(NewPrescriptionDto dto);
    Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId);
}