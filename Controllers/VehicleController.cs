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
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
        private Services.Vehicle _vehicleService;

        public VehicleController(Services.Vehicle vehicleService)
        {
            _vehicleService = vehicleService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Vehicle>>> GetVehicle()
        {
            List<Vehicle> vehicles = await _vehicleService.GetVehicle();
            if (vehicles != null)
            {
                return Ok(vehicles);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Vehicle>> GetVehicleById(int vehicleId)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleById(vehicleId);

                if (vehicle == null)
                {
                    return NotFound();
                }

                return vehicle;
            }
            catch
            {
                return StatusCode(500, "Not able to retrieve data from database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle(Vehicle vehicle)
        {
            try
            {
                if (vehicle == null)
                {
                    return BadRequest();
                }

                var existingVehicle = _vehicleService.GetVehicleById(vehicle.Id);

                if (existingVehicle != null)
                {
                    ModelState.AddModelError(vehicle.Id.ToString(), "That vehicle is already in database");
                    return BadRequest(ModelState);
                }

                var createdVehicle = await _vehicleService.Add(vehicle);

                return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicle.Id }, createdVehicle);
            }
            catch
            {
                return StatusCode(500, "unable to retrieve data from database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                var vehicleToUpdate = await _vehicleService.GetVehicleById(vehicle.Id);

                if (vehicleToUpdate != null)
                {
                    return await _vehicleService.Update(vehicle);
                }
                else
                {
                    return StatusCode(500, "Vehicle does not exist");
                }
            }
            catch
            {
                return StatusCode(500, "error updating vehicle database");
            }
        }

       

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            try
            {
                var vehicleToDelete = await _vehicleService.GetVehicleById(id);

                if (vehicleToDelete == null)
                {
                    return NotFound($"Vehicle with Id = {id} not found");
                }

                await _vehicleService.Delete(vehicleToDelete.Id);

                return Ok();
            }
            catch
            {
                return StatusCode(500, "error deleting data");
            }

        }
    }
}