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
    //Deler av CRUD koden; get, delete, add og update


    public class Mission
    {
        private readonly ApplicationDbContext _context;

        public Mission(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Mission> GetMissionById(int missionId)
        {
            var result = await _context.Missions.Where(s => s.Id == missionId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Models.Mission>> GetMission()
        {
            var result = await _context.Missions.ToListAsync();

            return result;
        }

        public async Task<Models.Mission> Add(Models.Mission mission)
        {
            await _context.Missions.AddAsync(mission);

            var createdMission = await _context.Missions.Where(s => s.Id == mission.Id).FirstOrDefaultAsync();

            return createdMission;
        }

        public async Task<Models.Mission> Update(Models.Mission mission)
        {
            var existingMission = await _context.Missions.Where(s => s.Id == mission.Id).FirstOrDefaultAsync();
            existingMission.Title = mission.Title;
            existingMission.Country = mission.Country;
            existingMission.Date = mission.Date;
            existingMission.Importance = mission.Importance;

            await _context.SaveChangesAsync();

            return existingMission;
        }



    public async Task Delete(int MissionId)
        {
            var existingMission = await _context.Missions.Where(s => s.Id == MissionId).FirstOrDefaultAsync();

            if (existingMission != null)
            {
                _context.Missions.Remove(existingMission);
            }
        }
    }
}
