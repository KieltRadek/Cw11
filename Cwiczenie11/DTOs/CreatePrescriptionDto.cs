namespace Cwiczenie11.DTOs;

public class CreatePrescriptionDto
{
    public PatientDto Patient { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentForPrescriptionDto> Medicaments { get; set; }
}