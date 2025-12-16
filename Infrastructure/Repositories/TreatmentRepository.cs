using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories 
{
    public class TreatmentRepository : RepositoryBase<Treatment> , ITreatmentRepository
    {

        public TreatmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _context.Treatments.AsNoTracking().ToListAsync();
        }

        public async Task<Treatment> GetByIdAsync(int treatmentId)
        {
            return await _context.Treatments.FindAsync(treatmentId);
        }
    }
}
