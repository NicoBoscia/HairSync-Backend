using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var normalizedEmail = email.ToLowerInvariant();
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task<List<User>> GetBarbersByBranchAsync(int branchId)
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Barber && u.BranchId == branchId)
                .ToListAsync();
        }


    }
}
