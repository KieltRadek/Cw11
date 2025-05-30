namespace Cw11.DTOs;

public class PatientDetailsDto
{
    public int IdPatient   { get; set; }
    public string FirstName{ get; set; }
    public string LastName { get; set; }
    public IEnumerable<PrescriptionDetailDto> Prescriptions { get; set; }
}