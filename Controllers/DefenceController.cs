using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Defence22.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Defence22.DataAccess;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Defence22.Controller //denne skulle selvsagt hete soldiercontroller, men her har vi glemt oss bort
{
    //Her er en delen av CRUD kommandoene gjort med Http (andre del ligger i service)
    //Funksjonene skal gjøre forskjellige ting om man kaller på de i vue, slette (og ID), legge til, oppdater etc.
    [ApiController]
    [Route("api/defence")]
    public class DefenceController : ControllerBase
    {
        private Services.Soldier _soldierService;

        public DefenceController(Services.Soldier soldierService)
        {
            _soldierService = soldierService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Soldier>>> GetSoldier()
        {
            List<Soldier> soldiers = await _soldierService.GetSoldiers();
            if (soldiers != null)
            {
                return Ok(soldiers);
            }
            else
            {
                return NotFound();
            }
        }

        //Denne koden skal hente etter Id i databasen
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Soldier>> GetSoldierById(int soldierId)
        {
            try
            {
                var soldier = await _soldierService.GetSoldierById(soldierId);

                if (soldier == null)
                {
                    return NotFound();
                }

                return soldier;
            }
            catch
            {
                return StatusCode(500, "Not able to retrieve data from database");
            }
        }

        //Denne skal lage ny soldat i databasen
        [HttpPost]
        public async Task<ActionResult<Soldier>> CreateSoldier(Soldier soldier)
        {
            try
            {
                if (soldier == null)
                {
                    return BadRequest();
                }

                var existingSoldier = _soldierService.GetSoldierById(soldier.Id);

                if (existingSoldier != null)
                {
                    ModelState.AddModelError(soldier.Id.ToString(), "That soldier is already in database");
                    return BadRequest(ModelState);
                }

                var createdSoldier = await _soldierService.Add(soldier);

                return CreatedAtAction(nameof(GetSoldier), new { id = createdSoldier.Id }, createdSoldier);
            }
            catch
            {
                return StatusCode(500, "unable to retrieve data from database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Soldier>> UpdateSoldier(Soldier soldier)
        {
            try
            {
                var soldierToUpdate = await _soldierService.GetSoldierById(soldier.Id);

                if(soldierToUpdate != null)
                {
                    return await _soldierService.Update(soldier);   
                }
                else
                {
                    return StatusCode(500, "Soldier does not exist");
                }
            }
            catch
            {
                return StatusCode(500, "error updating soldier database");
            }
        }

        //Denne skal slette etter id i databsen
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSoldier(int id)
        {
            try
            {
                var soldierToDelete = await _soldierService.GetSoldierById(id);

                if (soldierToDelete == null) 
                {
                    return NotFound($"Soldier with Id = {id} not found");
                }

                await _soldierService.Delete(soldierToDelete.Id);

                return Ok();
            }
            catch
            {
                return StatusCode(500, "error deleting data");
            }
         
        }
    }
}