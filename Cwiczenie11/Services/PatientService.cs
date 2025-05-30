using Cwiczenie11.DTOs;
using Cwiczenie11.Data;


namespace Cwiczenie11.Services;

public class PatientService : IPatientService
    {
        private readonly PharmacyContext _ctx;

        public PatientService(PharmacyContext ctx) 
            => _ctx = ctx;

        public async Task<PatientWithPrescriptionsDto> GetWithPrescriptionsAsync(int patientId)
        {
            var patient = await _ctx.Patients
                .AsNoTracking()
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == patientId);

            if (patient == null)
                throw new KeyNotFoundException($"Patient with Id {patientId} not found.");

            // Mapowanie na DTO
            var result = new PatientWithPrescriptionsDto
            {
                IdPatient  = patient.IdPatient,
                FirstName  = patient.FirstName,
                LastName   = patient.LastName,
                Email      = patient.Email,
                Birthdate  = patient.Birthdate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDto
                    {
                        IdPrescription = pr.IdPrescription,
                        Date           = pr.Date,
                        DueDate        = pr.DueDate,
                        Doctor         = new DoctorDto
                        {
                            IdDoctor  = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName  = pr.Doctor.LastName
                        },
                        Medicaments   = pr.PrescriptionMedicaments
                            .Select(pm => new MedicamentOnPrescriptionDto
                            {
                                IdMedicament = pm.IdMedicament,
                                Name         = pm.Medicament.Name,
                                Dose         = pm.Dose,
                                Details      = pm.Details
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return result;
        }
    }