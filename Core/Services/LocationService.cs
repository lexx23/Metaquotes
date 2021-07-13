using Common.DataProviders;
using Common.Model;
using Common.Services;

namespace Services
{
    public class LocationService : ILocationService
    {
        private readonly ICacheService<Location[], string> _cacheService;
        private readonly ILocationDataProvider _locationDataProvider;
        
        public LocationService(ILocationDataProvider locationDataProvider, ICacheService<Location[],string> cacheService)
        {
            _cacheService = cacheService;
            _locationDataProvider = locationDataProvider;
        }

        public Location[] Search(string city)
        {
            return _cacheService.Cache(city,() => _locationDataProvider.Search(city));
        }
    }
}