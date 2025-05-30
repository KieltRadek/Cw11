namespace Cw11.DTOs;

public class PrescriptionDetailDto
{
    public int IdPrescription  { get; set; }
    public DateTime Date       { get; set; }
    public DateTime DueDate    { get; set; }
    public IEnumerable<MedicamentDetailDto> Medicaments { get; set; }
    public DoctorDto Doctor    { get; set; }
}