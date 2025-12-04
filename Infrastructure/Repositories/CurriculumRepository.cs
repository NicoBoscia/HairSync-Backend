using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CurriculumRepository : RepositoryBase<Curriculum>, ICurriculumRepository
    {
        public CurriculumRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Curriculum>> GetAllCurriculumsAsync()
        {
            return await _context.Set<Curriculum>()
                                 .OrderByDescending(c => c.UploadDate)
                                 .ToListAsync();
        }
    }
}
