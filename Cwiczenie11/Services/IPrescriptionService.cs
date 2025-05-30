using Cwiczenie11.DTOs;

namespace Cwiczenie11.Services;

public interface IPrescriptionService
{
    Task<int> CreateAsync(CreatePrescriptionDto dto);
}