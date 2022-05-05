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
    [Route("api/mission")]
    public class MissionController : ControllerBase
    {
        private Services.Mission _missionService;

        public MissionController(Services.Mission missionService)
        {
            _missionService = missionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Mission>>> GetMission()
        {
            List<Mission> missions = await _missionService.GetMission();
            if (missions != null)
            {
                return Ok(missions);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Mission>> GetMissionById(int missionId)
        {
            try
            {
                var mission = await _missionService.GetMissionById(missionId);
                
                if (mission == null)
                {
                    return NotFound();
                }

                return mission;
            }
            catch
            {
                return StatusCode(500, "Not able to retrieve data from database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Mission>> CreateMission(Mission mission)
        {
            try
            {
                if (mission == null)
                {
                    return BadRequest();
                }

                var existingMission = _missionService.GetMissionById(mission.Id);

                if (existingMission != null)
                {
                    ModelState.AddModelError(mission.Id.ToString(), "That mission is already in database");
                    return BadRequest(ModelState);
                }

                var createdMission = await _missionService.Add(mission);

                return CreatedAtAction(nameof(GetMission), new { id = createdMission.Id }, createdMission);
            }
            catch
            {
                return StatusCode(500, "unable to retrieve data from database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Mission>> UpdateMission(Mission mission)
        {
            try
            {
                var missionToUpdate = await _missionService.GetMissionById(mission.Id);

                if (missionToUpdate != null)
                {
                    return await _missionService.Update(mission);
                }
                else
                {
                    return StatusCode(500, "Mission does not exist");
                }
            }
            catch
            {
                return StatusCode(500, "error updating mission database");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMission(int id)
        {
            try
            {
                var missionToDelete = await _missionService.GetMissionById(id);

                if (missionToDelete == null)
                {
                    return NotFound($"Mission with Id = {id} not found");
                }

                await _missionService.Delete(missionToDelete.Id);

                return Ok();
            }
            catch
            {
                return StatusCode(500, "error deleting data");
            }

        }
    }
}