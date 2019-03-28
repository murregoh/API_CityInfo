using Microsoft.AspNetCore.Mvc;
using CitiesInfo.Models;
using CitiesInfo.DataStorage;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System;
using CitiesInfo.Interfaces;
using CitiesInfo.Entities;
using AutoMapper;

namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _infoRepository;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, ICityInfoRepository infoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _infoRepository = infoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_infoRepository.CityExist(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accesing points of interest.");
                    return NotFound("City was not found");
                }

                IEnumerable<PointOfInterest> pointOfInterest = _infoRepository.GetPointsOfInterest(cityId);

                return Ok(Mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterest));

                //CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);

                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} wasn't found when accesing points of interest.");
                //    return NotFound();
                //}

                //ICollection<PointOfInterestDto> pointsofinterest = city.pointofinterestdto;

                //return Ok(pointsofinterest);
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
            if (!_infoRepository.CityExist(cityId))
                return NotFound("City was not found.");

            PointOfInterest pointOfInterest = _infoRepository.GetPointOfInterest(cityId, point);

            if (pointOfInterest == null)
                return NotFound("Point of interest was not found");

            return Ok(Mapper.Map<PointOfInterestDto>(pointOfInterest));

            //CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);
            //PointOfInterestDto pointsofinterest = city.pointofinterestdto.FirstOrDefault(p => p.id == point);

            //if (city == null || pointsofinterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointsofinterest);

        }

        [HttpPost("{cityId}/pointofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInteresForCreationDto pointOfInterest) 
        {
            if (pointOfInterest == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (pointOfInterest.name == pointOfInterest.description)
                ModelState.AddModelError("Description", "The provide values should not be equal.");

            if (!_infoRepository.CityExist(cityId))
                return NotFound("City was not found");

            PointOfInterest finalPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);

            _infoRepository.AddPointOfInterest(cityId, finalPointOfInterest);

            if (!_infoRepository.Save())
                return StatusCode(500, "A problem happened while saving your point of interest.");

            PointOfInterest createdPointOfInterestToReturn = Mapper.Map<PointOfInterest>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, point = createdPointOfInterestToReturn.id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointofinterest/{point}")]
        public IActionResult UpdatePointOfInterest(int cityId, int point, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_infoRepository.CityExist(cityId))
                return NotFound("City was not found");

            if (pointOfInterest.name == pointOfInterest.description)
                ModelState.AddModelError("Description", "The provide values should not be equal.");

            PointOfInterest pointOfInterestEntity = _infoRepository.GetPointOfInterest(cityId, point);

            if (pointOfInterestEntity == null)
                return NotFound("Point of interest was not found");

            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_infoRepository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }
    
        [HttpPatch("{cityId}/pointofinterest/{point}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int point, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!_infoRepository.CityExist(cityId))
                return NotFound("City was not found");

            PointOfInterest pointOfInterestEntity = _infoRepository.GetPointOfInterest(cityId, point);

            if (pointOfInterestEntity == null)
                return NotFound("Point of interest was not found");

            PointOfInterestForUpdateDto pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pointOfInterestToPatch.name == pointOfInterestToPatch.description)
                ModelState.AddModelError("Description", "The provide values should not be equal.");

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_infoRepository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();

        }

        [HttpDelete("{cityId}/pointofinterest/{point}")]
        public IActionResult DeletePointOfInterest(int cityId, int point)
        {
            if (_infoRepository.CityExist(cityId))
                return NotFound("City was not found");

            PointOfInterest pointOfInterestEntity = _infoRepository.GetPointOfInterest(cityId, point);

            if (pointOfInterestEntity == null)
                return NotFound("Point of interest was not found.");

            _infoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_infoRepository.Save())
                return StatusCode(500, "A problem happened while handling your request.");
                
            _mailService.Send("Point of interest has been removed", $"The point of interest {pointOfInterestEntity.name} with id {pointOfInterestEntity.id} has been removed.");

            return NoContent();
        }

    }
}
