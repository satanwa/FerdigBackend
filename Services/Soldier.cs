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
    //Deler av CRUD koden og resterende ligger i Controller
    public class Soldier
    {
        private readonly ApplicationDbContext _context;

        public Soldier(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Soldier> GetSoldierById(int soldierId)
        {
            var result = await _context.Soldiers.Where(s => s.Id == soldierId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Models.Soldier>> GetSoldiers()
        {
            var result = await _context.Soldiers.ToListAsync();

            return result;
        }

        public async Task<Models.Soldier> Add(Models.Soldier soldier)
        {
            await _context.Soldiers.AddAsync(soldier);

            var createdSoldier = await _context.Soldiers.Where(s => s.Id == soldier.Id).FirstOrDefaultAsync();

            return createdSoldier;
        }

        public async Task<Models.Soldier> Update(Models.Soldier soldier)
        {
            var existingSoldier = await _context.Soldiers.Where(s => s.Id == soldier.Id).FirstOrDefaultAsync();
            existingSoldier.ImagePath = soldier.ImagePath;
            existingSoldier.Rank = soldier.Rank;
            existingSoldier.WholeName = soldier.WholeName;

            await _context.SaveChangesAsync();

            return existingSoldier;
        }

        public async Task Delete(int soldierId)
        {
            var existingSoldier = await _context.Soldiers.Where(s => s.Id == soldierId).FirstOrDefaultAsync();

            if (existingSoldier != null)
            {
                _context.Soldiers.Remove(existingSoldier);
            }
        }
    }
}
