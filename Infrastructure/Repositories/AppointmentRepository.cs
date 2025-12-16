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
    public class AppointmentRepository : RepositoryBase<Appointment> , IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        private IQueryable<Appointment> GetAppointmentsWithDetails()
        {
            return _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Barber)
                .Include(a => a.Branch)
                .Include(a => a.Treatment)
                .AsQueryable();
        }

        public async Task<Appointment> GetByIdWithDetailsAsync(int id)
        {
            return await GetAppointmentsWithDetails()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAllWithDetailsAsync()
        {
            return await GetAppointmentsWithDetails()
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetFutureAppointmentsByClientIdAsync(int clientId)
        {
            var pendingStatus = new[] { AppointmentStatus.Confirmed };

            return await GetAppointmentsWithDetails()
                .Where(a => a.ClientId == clientId &&
                             pendingStatus.Contains(a.Status) &&
                             a.AppointmentDateTime >= DateTime.UtcNow) 
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByBarberIdAndDateAsync(int barberId, DateTime date)
        {
            var relevantStatus = new[] { AppointmentStatus.Confirmed };
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await GetAppointmentsWithDetails()
                .Where(a => a.BarberId == barberId &&
                             relevantStatus.Contains(a.Status) &&
                             a.AppointmentDateTime >= startOfDay &&
                             a.AppointmentDateTime < endOfDay)
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync();
        }

        /// <summary>
        /// Implementación de la lógica de solapamiento.
        /// </summary>
        public async Task<bool> CheckBarberAvailabilityAsync(int barberId, DateTime requestedStartTime, int durationInMinutes)
        {
            var relevantStatus = new[] { AppointmentStatus.Confirmed };

            DateTime requestedEndTime = requestedStartTime.AddMinutes(durationInMinutes);

            var isOverlapping = await _context.Appointments
                .Where(a => a.BarberId == barberId && relevantStatus.Contains(a.Status))
                .AnyAsync(a =>
                    (a.AppointmentDateTime < requestedEndTime) &&

                    (a.AppointmentDateTime.AddMinutes(durationInMinutes) > requestedStartTime)
                );

            return !isOverlapping;
        }
    }
}
