namespace Cw11.DTOs;

public class NewPrescriptionDto
{
    public PatientDto Patient { get; set; }
    public int IdDoctor       { get; set; }
    public DateTime Date      { get; set; }
    public DateTime DueDate   { get; set; }
    public List<NewPrescriptionMedDto> Medicaments { get; set; }
}