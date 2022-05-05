using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Defence22.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Defence22.Services
{

    //vi valgte å bruke services da det skal være en "middleman" mellom models, database
    //og Controllers.Hadde vi ikke brukt services hadde deler av koden her gått inn i controller.
    //Deler av CRUD koden;

    public class Vehicle
    {
        private readonly ApplicationDbContext _context;

        public Vehicle (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Vehicle> GetVehicleById(int vehicleId)
        {
            var result = await _context.Vehicles.Where(s => s.Id == vehicleId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Models.Vehicle>> GetVehicle()
        {
            var result = await _context.Vehicles.ToListAsync();

            return result;
        }

        public async Task<Models.Vehicle> Add(Models.Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);

            var createdVehicle = await _context.Vehicles.Where(s => s.Id == vehicle.Id).FirstOrDefaultAsync();

            return createdVehicle;
        }

        public async Task<Models.Vehicle> Update(Models.Vehicle vehicle)
        {
            var existingVehicle = await _context.Vehicles.Where(s => s.Id == vehicle.Id).FirstOrDefaultAsync();
            existingVehicle.Title = vehicle.Title;
            existingVehicle.Location = vehicle.Location;
            existingVehicle.AmountPeople = vehicle.AmountPeople;
            existingVehicle.Weight = vehicle.Weight;
            existingVehicle.Maintenance = vehicle.Maintenance;

            await _context.SaveChangesAsync();

            return existingVehicle;
        }

        public async Task Delete(int vehicleId)
        {
            var existingVehicle = await _context.Vehicles.Where(s => s.Id == vehicleId).FirstOrDefaultAsync();

            if (existingVehicle != null)
            {
                _context.Vehicles.Remove(existingVehicle);
            }
        }

    }
}
