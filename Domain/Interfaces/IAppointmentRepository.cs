using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        Task<Appointment> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Appointment>> GetAllWithDetailsAsync();

        Task<IEnumerable<Appointment>> GetFutureAppointmentsByClientIdAsync(int clientId);

        Task<IEnumerable<Appointment>> GetByBarberIdAndDateAsync(int barberId, DateTime date);

        Task<bool> CheckBarberAvailabilityAsync(int barberId, DateTime requestedStartTime, int durationInMinutes);
    }
}


