using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            IQueryable<City> collection = _context.Cities as IQueryable<City>; //for deferred execution

            if (!string.IsNullOrWhiteSpace(name))
            {//filtering
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery) || 
                    (c.Description != null && c.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetaData = new PaginationMetadata(totalItemCount, pageSize, currentPage: pageNumber);

            List<City> collectionToReturn = await collection
                            .OrderBy(c => c.Name) //query is NOT YET executed; we are just building the query
                            .Skip(pageSize * (pageNumber - 1)) //page at the end when the elements are ordered/sorted
                            .Take(pageSize)
                            .ToListAsync(); //query get executed ON DB (not on local machine) when running ToListAsync()

            return (collectionToReturn, paginationMetaData); 
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Cities
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterests
                .Where(p => p.CityId == cityId)
                .ToListAsync();
        }


        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, includePointsOfInterest: false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
