using Cw11.DTOs;
using Cw11.Interfaces;
using Cwiczenie11.Data;
using Cwiczenie11.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw11.Services;

public class PrescriptionService : IPrescriptionService
    {
        private readonly PharmacyContext _ctx;
        public PrescriptionService(PharmacyContext ctx) => _ctx = ctx;

        public async Task<int> CreatePrescriptionAsync(NewPrescriptionDto dto)
        {
            if (dto.Medicaments.Count > 10)
                throw new ArgumentException("Max 10 medicaments.");
            if (dto.DueDate < dto.Date)
                throw new ArgumentException("DueDate < Date.");

            var doctor = await _ctx.Doctors.FindAsync(dto.IdDoctor)
                  ?? throw new KeyNotFoundException($"Doctor {dto.IdDoctor} not found.");

            // patient
            Patient patient = null;
            if (dto.Patient.IdPatient > 0)
                patient = await _ctx.Patients.FindAsync(dto.Patient.IdPatient);
            if (patient == null)
            {
                patient = new Patient {
                    FirstName = dto.Patient.FirstName,
                    LastName  = dto.Patient.LastName,
                    Email     = dto.Patient.Email,
                    Birthdate = dto.Patient.Birthdate
                };
                _ctx.Patients.Add(patient);
            }

            // meds exist?
            var ids = dto.Medicaments.Select(m => m.IdMedicament).ToList();
            var meds = await _ctx.Medicaments.Where(m => ids.Contains(m.IdMedicament)).ToListAsync();
            if (meds.Count != ids.Count)
                throw new KeyNotFoundException("Some medicaments not found.");

            // create presc
            var presc = new Prescription {
                Date    = dto.Date,
                DueDate = dto.DueDate,
                Patient = patient,
                Doctor  = doctor
            };
            _ctx.Prescriptions.Add(presc);

            foreach (var m in dto.Medicaments)
            {
                presc.PrescriptionMedicaments.Add(new PrescriptionMedicament {
                    IdMedicament = m.IdMedicament,
                    Dose         = m.Dose,
                    Details      = m.Description
                });
            }

            await _ctx.SaveChangesAsync();
            return presc.IdPrescription;
        }

        public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId)
        {
            var p = await _ctx.Patients
                .Include(x => x.Prescriptions)
                  .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
                .Include(x => x.Prescriptions)
                  .ThenInclude(pr => pr.Doctor)
                .SingleOrDefaultAsync(x => x.IdPatient == patientId);

            if (p == null)
                throw new KeyNotFoundException($"Patient {patientId} not found.");

            return new PatientDetailsDto {
                IdPatient   = p.IdPatient,
                FirstName   = p.FirstName,
                LastName    = p.LastName,
                Prescriptions = p.Prescriptions
                  .OrderBy(pr => pr.DueDate)
                  .Select(pr => new PrescriptionDetailDto {
                    IdPrescription = pr.IdPrescription,
                    Date           = pr.Date,
                    DueDate        = pr.DueDate,
                    Doctor = new DoctorDto {
                      IdDoctor  = pr.Doctor.IdDoctor,
                      FirstName = pr.Doctor.FirstName,
                      LastName  = pr.Doctor.LastName
                    },
                    Medicaments = pr.PrescriptionMedicaments
                      .Select(pm => new MedicamentDetailDto {
                        IdMedicament = pm.IdMedicament,
                        Name         = pm.Medicament.Name,
                        Dose         = pm.Dose,
                        Description  = pm.Details
                      })
                  })
            };
        }
    }