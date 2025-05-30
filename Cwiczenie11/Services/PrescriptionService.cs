using Cwiczenie11.Data;
using Cwiczenie11.DTOs;
using Cwiczenie11.Models;
using Microsoft.EntityFrameworkCore;


namespace Cwiczenie11.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly PharmacyContext _ctx;
        public PrescriptionService(PharmacyContext ctx) 
            => _ctx = ctx;

        public async Task<int> CreateAsync(CreatePrescriptionDto dto)
        {
            // 1) Walidacje wejścia
            if (dto.Medicaments is null || !dto.Medicaments.Any())
                throw new ArgumentException("Prescription must contain at least one medicament.");
            
            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("No more than 10 medicaments allowed.");

            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate must be >= Date.");

            // 2) Sprawdź czy lekarz istnieje
            var doctorExists = await _ctx.Doctors
                .AsNoTracking()
                .AnyAsync(d => d.IdDoctor == dto.DoctorId);
            if (!doctorExists)
                throw new ArgumentException($"Doctor with id {dto.DoctorId} not found.");

            // 3) Upsert pacjenta
            Patient patient;
            if (dto.Patient.IdPatient.HasValue)
            {
                patient = await _ctx.Patients
                    .FirstOrDefaultAsync(p => p.IdPatient == dto.Patient.IdPatient.Value);

                if (patient == null)
                {
                    patient = MapToNewPatient(dto.Patient);
                    _ctx.Patients.Add(patient);
                }
                else
                {
                    UpdateExistingPatient(patient, dto.Patient);
                }
            }
            else
            {
                patient = MapToNewPatient(dto.Patient);
                _ctx.Patients.Add(patient);
            }

            // 4) Walidacja istnienia leków
            var medIds = dto.Medicaments
                .Select(m => m.IdMedicament)
                .Distinct()
                .ToList();

            var existingMedIds = await _ctx.Medicaments
                .Where(m => medIds.Contains(m.IdMedicament))
                .Select(m => m.IdMedicament)
                .ToListAsync();

            var missing = medIds.Except(existingMedIds).ToList();
            if (missing.Any())
                throw new ArgumentException($"Medicaments not found: {string.Join(", ", missing)}.");

            // 5) Budowa encji recepty
            var prescription = new Prescription
            {
                Date       = dto.Date,
                DueDate    = dto.DueDate,
                IdDoctor   = dto.DoctorId,
                Patient    = patient
            };

            foreach (var medDto in dto.Medicaments)
            {
                prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = medDto.IdMedicament,
                    Dose         = medDto.Dose,
                    Details      = medDto.Details
                });
            }

            // 6) Persist i zwrócenie klucza
            _ctx.Prescriptions.Add(prescription);
            await _ctx.SaveChangesAsync();
            return prescription.IdPrescription;
        }

        // Pomocnicze
        private static Patient MapToNewPatient(PatientDto dto)
            => new Patient
            {
                FirstName = dto.FirstName,
                LastName  = dto.LastName,
                Email     = dto.Email,
                Birthdate = dto.Birthdate
            };

        private static void UpdateExistingPatient(Patient patient, PatientDto dto)
        {
            patient.FirstName = dto.FirstName;
            patient.LastName  = dto.LastName;
            patient.Email     = dto.Email;
            patient.Birthdate = dto.Birthdate;
            // EF Core sam śledzi zmiany
        }
    }
}