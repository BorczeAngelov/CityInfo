using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointOfInterestsForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestsForCity));

            //try
            //{
            //    var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
            //        return NotFound();
            //    }

            //    return Ok(city.PointsOfInterest);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical($"Exception while getting points of interest for wity with id {cityId}", ex);
            //    return StatusCode(500, "A problem happened while handling your request.");
            //}
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId,
            int pointofinterestid)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

            //var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterestDto = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointofinterestid);
            //if (pointOfInterestDto == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterestDto);
        }

        //[HttpPost]
        //public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        //    int cityId,
        //    [FromBody] PointOfInterestForCreationDto pointOfInterest)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    // demo purpose - not ideal; to be improved
        //    var maxPointOfInterestId = _citiesDataStore.Cities
        //        .SelectMany(c => c.PointsOfInterest)
        //        .Max(c => c.Id);


        //    var finalPointOfInterest = new PointOfInterestDto()
        //    {
        //        Id = ++maxPointOfInterestId,
        //        Name = pointOfInterest.Name,
        //        Description = pointOfInterest.Description,
        //    };

        //    city.PointsOfInterest.Add(finalPointOfInterest);

        //    return CreatedAtRoute("GetPointOfInterest",
        //        new
        //        {
        //            cityId = cityId,
        //            pointOfInterestId = finalPointOfInterest.Id
        //        },
        //        finalPointOfInterest);
        //}


        //[HttpPut("{pointofinterestid}")]
        //public ActionResult<PointOfInterestDto> UpdatePointOfInterest(
        //    int cityId,
        //    int pointOfInterestId,
        //    [FromBody] PointOfInterestForCreationDto pointOfInterest)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    pointOfInterestFromStore.Name = pointOfInterest.Name;
        //    pointOfInterestFromStore.Description = pointOfInterest.Description;

        //    return NoContent();
        //}

        //[HttpPatch("{pointofinterestid}")]
        //public ActionResult PartiallyUpdatePointOfInterest(
        //    int cityId,
        //    int pointOfInterestId,
        //    JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        //    {
        //        Name = pointOfInterestFromStore.Name,
        //        Description = pointOfInterestFromStore.Description,
        //    };


        //    patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!TryValidateModel(pointOfInterestToPatch)) //check if name prop is invalid for example
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        //    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        //    return NoContent();
        //}


        //[HttpDelete("{pointofinterestid}")]
        //public ActionResult DeletePointOfInterest(
        //    int cityId,
        //    int pointofinterestid)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(i => i.Id == cityId);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointofinterestid);
        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    city.PointsOfInterest.Remove(pointOfInterestFromStore);
        //    _mailService.Send(
        //        subject: "Point of interest deleted.",
        //        message: $"Point of interest {pointOfInterestFromStore.Name} with is {pointOfInterestFromStore.Id} is deleted");

        //    return NoContent();
        //}
    }
}
