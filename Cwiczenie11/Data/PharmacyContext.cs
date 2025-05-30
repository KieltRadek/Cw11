using Cwiczenie11.Data;
using Cwiczenie11.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenie11.Data
{
    public class PharmacyContext : DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> opts)
            : base(opts) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor>  Doctors  { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Patient>().HasKey(p => p.IdPatient);
            mb.Entity<Doctor>().HasKey(d => d.IdDoctor);
            mb.Entity<Medicament>().HasKey(m => m.IdMedicament);
            mb.Entity<Prescription>().HasKey(p => p.IdPrescription);

            mb.Entity<PrescriptionMedicament>()
                .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

            mb.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.IdPatient);

            mb.Entity<Prescription>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.IdDoctor);

            mb.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Prescription)
                .WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdPrescription);

            mb.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdMedicament);

            mb.Entity<Patient>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}