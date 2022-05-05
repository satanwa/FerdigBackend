using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Defence22.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Defence22.DataAccess;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Defence22.Controller
{

    //Her er en delen av CRUD kommandoene gjort med Http (andre del ligger i service)
    //Funksjonene skal gjøre forskjellige ting om man kaller på de i vue, slette (og ID), legge til, oppdater etc.

    [ApiController]
    [Route("api/weapon")]
    public class WeaponController : ControllerBase
    {
        private Services.Weapon _weaponService;

        public WeaponController(Services.Weapon weaponService)
        {
            _weaponService = weaponService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> GetWeapon()
        {
            List<Weapon> weapons = await _weaponService.GetWeapon();
            if (weapons != null)
            {
                return Ok(weapons);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Weapon>> GetWeaponById(int weaponId)
        {
            try
            {
                var weapon = await _weaponService.GetWeaponById(weaponId);

                if (weapon == null)
                {
                    return NotFound();
                }

                return weapon;
            }
            catch
            {
                return StatusCode(500, "Not able to retrieve data from database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Weapon>> CreateWeapon(Weapon weapon)
        {
            try
            {
                if (weapon == null)
                {
                    return BadRequest();
                }

                var existingWeapon = _weaponService.GetWeaponById(weapon.Id);

                if (existingWeapon != null)
                {
                    ModelState.AddModelError(weapon.Id.ToString(), "That weapon is already in database");
                    return BadRequest(ModelState);
                }

                var createdWeapon = await _weaponService.Add(weapon);

                return CreatedAtAction(nameof(GetWeapon), new { id = createdWeapon.Id }, createdWeapon);
            }
            catch
            {
                return StatusCode(500, "unable to retrieve data from database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Weapon>> UpdateWeapon(Weapon weapon)
        {
            try
            {
                var weaponToUpdate = await _weaponService.GetWeaponById(weapon.Id);

                if (weaponToUpdate != null)
                {
                    return await _weaponService.Update(weapon);
                }
                else
                {
                    return StatusCode(500, "Weapon does not exist");
                }
            }
            catch
            {
                return StatusCode(500, "error updating weapon database");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteWeapon(int id)
        {
            try
            {
                var weaponToDelete = await _weaponService.GetWeaponById(id);

                if (weaponToDelete == null)
                {
                    return NotFound($"Weapon with Id = {id} not found");
                }

                await _weaponService.Delete(weaponToDelete.Id);

                return Ok();
            }
            catch
            {
                return StatusCode(500, "error deleting data");
            }

        }
    }
}