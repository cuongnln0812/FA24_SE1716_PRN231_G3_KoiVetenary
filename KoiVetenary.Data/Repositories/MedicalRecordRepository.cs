using KoiVetenary.Data.Base;
using KoiVetenary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Data.Repositories
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>
    {
        public MedicalRecordRepository() { }
        public MedicalRecordRepository(FA24_SE1716_PRN231_G3_KoiVetenaryContext context) => _context = context;

        public async Task<List<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords.Include(a => a.Animal).ToListAsync();
        }

        public async Task<MedicalRecord> GetByIdAsync(int id)
        {
            var entity = await _context.MedicalRecords.Include(a => a.Animal).FirstOrDefaultAsync(a => a.RecordId == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }
    }
}
