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
    public class Weapon
    {
        private readonly ApplicationDbContext _context;

        public Weapon(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Weapon> GetWeaponById(int weaponId)
        {
            var result = await _context.Weapons.Where(s => s.Id == weaponId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Models.Weapon>> GetWeapon()
        {
            var result = await _context.Weapons.ToListAsync();

            return result;
        }

        public async Task<Models.Weapon> Add(Models.Weapon weapon)
        {
            await _context.Weapons.AddAsync(weapon);

            var createdWeapon = await _context.Weapons.Where(s => s.Id == weapon.Id).FirstOrDefaultAsync();

            return createdWeapon;
        }

        public async Task<Models.Weapon> Update(Models.Weapon weapon)
        {
            var existingWeapon = await _context.Weapons.Where(s => s.Id == weapon.Id).FirstOrDefaultAsync();
            existingWeapon.Title = weapon.Title;
            existingWeapon.Stock = weapon.Stock;
            existingWeapon.BulletsPS = weapon.BulletsPS;


        await _context.SaveChangesAsync();

            return existingWeapon;
        }

        public async Task Delete(int weaponId)
        {
            var existingWeapon = await _context.Weapons.Where(s => s.Id == weaponId).FirstOrDefaultAsync();

            if (existingWeapon != null)
            {
                _context.Weapons.Remove(existingWeapon);
            }
        }
    }
}
