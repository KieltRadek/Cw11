using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cwiczenie11.Models;

[Table("Patient")]
public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName  { get; set; }
    public string LastName   { get; set; }
    public string Email      { get; set; }
    public DateTime Birthdate{ get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
        = new List<Prescription>();
}