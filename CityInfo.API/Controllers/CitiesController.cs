using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController] //Automatic HTTP 400 responses; Problem details for error status codes etc.
    //[Authorize] //disable temporary
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            [FromQuery] string? /*filterValue*/ name,
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            (IEnumerable<Entities.City> cityEntities, PaginationMetadata paginationMetaData) = 
                await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

            var result = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(result);
        }

        /// <summary>
        /// Get a city ba id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>An IActonResult</returns>
        /// <response code="200">Returns the requested city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));

            //var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == id);
            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}
            //return Ok(cityToReturn);
        }
    }
}
