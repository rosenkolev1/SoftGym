﻿namespace SoftGym.Services.CronJobs
{
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class DeletePastAppointments
    {
        private readonly IDeletableEntityRepository<Appointment> appointmentsRepository;

        public DeletePastAppointments(IDeletableEntityRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task Work()
        {
            var allAppointments = await this.appointmentsRepository
                .All()
                .ToListAsync();

           var expiredAppointments =
                allAppointments
                .Where(x => x.StartTime.Subtract(DateTime.UtcNow).Days < 0)
                .ToList();

            foreach (var appointment in expiredAppointments)
            {
                this.appointmentsRepository.Delete(appointment);
            }

            await this.appointmentsRepository.SaveChangesAsync();
        }
    }
}
