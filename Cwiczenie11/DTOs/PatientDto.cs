namespace Cw11.DTOs;

public class PatientDto
{
    public int IdPatient   { get; set; }
    public string FirstName{ get; set; }
    public string LastName { get; set; }
    public string Email    { get; set; }
    public DateTime Birthdate { get; set; }
}