using Microsoft.AspNetCore.Mvc;
using CitiesInfo.Models;
using CitiesInfo.DataStorage;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using CitiesInfo.Services;

namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private LocalMailService _mailService;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, LocalMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {

            try
            {
                CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accesing points of interest.");
                    return NotFound();
                }

                ICollection<PointOfInterestDto> pointsofinterest = city.pointofinterestdto;

                return Ok(pointsofinterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happend while handling your request.");
            }

        }

        [HttpGet("{cityId}/pointsofinterest/{point}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int point)
        {

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            PointOfInterestDto pointsofinterest = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            if (city == null || pointsofinterest == null)
            {
                return NotFound();
            }

            return Ok(pointsofinterest);

        }

        [HttpPost("{cityId}/pointofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInteresForCreationDto pointOfInterest) 
        {
            if (pointOfInterest == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest.name == pointOfInterest.description)
            {
                ModelState.AddModelError("Description", "The provide values should not be equal.");
            }

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            int maxPointsOfInterest = city.pointofinterestdto.Max(p => p.id);

            PointOfInterestDto finalPointOfInterest = new PointOfInterestDto()
            {
                id = ++maxPointsOfInterest,
                name = pointOfInterest.name,
                description = pointOfInterest.description
            };

            city.pointofinterestdto.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, point = finalPointOfInterest.id }, finalPointOfInterest);

        }

        [HttpPut("{cityId}/pointofinterest/{point}")]
        public IActionResult UpdatePointOfInterest(int cityId, int point, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest.name == pointOfInterest.description)
            {
                ModelState.AddModelError("Description", "The provide values should not be equal.");
            }

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            PointOfInterestDto pointToModify = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            if (city == null || pointToModify == null)
            {
                return NotFound();
            }

            pointToModify.name = pointOfInterest.name;
            pointToModify.description = pointOfInterest.description;

            return NoContent();


        }
    
        [HttpPatch("{cityId}/pointofinterest/{point}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int point, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            PointOfInterestDto pointOfInterest = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            if (city == null || pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                name = pointOfInterest.name,
                description = pointOfInterest.description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.name == pointOfInterestToPatch.description)
            {
                ModelState.AddModelError("Description", "The provide values should not be equal.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.name = pointOfInterestToPatch.name;
            pointOfInterest.description = pointOfInterestToPatch.description;

            return NoContent();

        }

        [HttpDelete("{cityId}/pointofinterest/{point}")]
        public IActionResult DeletePointOfInterest(int cityId, int point)
        {
            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            PointOfInterestDto pointOfInterest = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            if (city == null || pointOfInterest == null)
            {
                return NotFound();
            }

            city.pointofinterestdto.Remove(pointOfInterest);

            _mailService.Send("Point of interest has been removed", $"The point of interest {pointOfInterest.name} with id {pointOfInterest.id} has been removed.");

            return NoContent();
        }

    }
}
