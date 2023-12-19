using Data;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ClinicRepository: BaseRepository<Clinic>
    {
        private readonly AppDbContext _context;

        public ClinicRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Clinic> GetAllClinics()
        {
            IQueryable<Clinic> query = _context.Clinics;

            query = GetAllInclude(query, ["Department"]);
            return query.ToList();
        }

        public Clinic? GetClinicById(int clinicId)
        {
            return _context.Clinics.Find(clinicId);
        }

        public void AddClinic(Clinic clinic)
        {
            _context.Clinics.Add(clinic);
            _context.SaveChanges();
        }

        public void UpdateClinic(Clinic clinic)
        {
            _context.Clinics.Update(clinic);
            _context.SaveChanges();
        }

        public void DeleteClinic(int clinicId)
        {
            var clinic = _context.Clinics.Find(clinicId);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
                _context.SaveChanges();
            }
        }
    }

}
