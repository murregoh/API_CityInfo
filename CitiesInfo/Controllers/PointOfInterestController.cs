using Microsoft.AspNetCore.Mvc;
using CitiesInfo.Models;
using CitiesInfo.DataStorage;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {

            ICollection<PointOfInterestDto> pointsofinterest = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId).pointofinterestdto;

            return Ok(pointsofinterest);

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

            return NoContent();
        }

    }
}
