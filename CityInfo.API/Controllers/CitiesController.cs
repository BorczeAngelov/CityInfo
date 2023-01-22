using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] //Automatic HTTP 400 responses; Problem details for error status codes etc.
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            var result = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(result);
            
            //var result = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var city in cityEntities)
            //{
            //    result.Add(new CityWithoutPointsOfInterestDto()
            //    {
            //        Id = city.Id,
            //        Description = city.Description,
            //        Name = city.Name,
            //    });
            //}
            //return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == id);

            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);
            return Ok();
        }
    }
}
