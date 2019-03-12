using Microsoft.AspNetCore.Mvc;
using CitiesInfo.DataStorage;
using System.Linq;
using CitiesInfo.Models;

namespace CitiesInfo.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities() 
        {
            return Ok(CityDataStorage.Currect.Cities);
        }

        [HttpGet("{cityId}")]
        public IActionResult GetCity(int cityId) 
        {

            CityDto city = CityDataStorage.Currect.Cities.FirstOrDefault(c => c.id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);

        }
    }
}
