using System;
using System.Collections.Generic;
using System.Linq;
using Common.DataProviders;
using Common.Model;

namespace DAL.Binary.DataProviders
{
    internal class LocationDataProvider : ILocationDataProvider
    {
        private readonly DatabaseContext _context;

        public LocationDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public Location Get(int index)
        {
            if (index < 0 || index > _context.LocationsInfo.Locations.Length)
                throw new ArgumentException(nameof(index));

            return Map(_context.LocationsInfo.Locations[index]);
        }

        public Location[] Search(string city)
        {
            if (string.IsNullOrEmpty(city))
                throw new ArgumentNullException(nameof(city));

            var indexedLocations = BinarySearch(city);
            var result = new Location[indexedLocations.Length];
            for (var i = 0; i < indexedLocations.Length; i++)
            {
                var dbLocation = _context.LocationsInfo.Locations[_context.LocationsInfo.SortedIndex[indexedLocations[i]]];
                result[i] = Map(dbLocation);
            }

            return result;
        }

        // Binary search algorithm
        private unsafe int[] BinarySearch(string city)
        {
            IList<int> result = new List<int>();

            var low = 0; // 0 is always going to be the first element
            var high = _context.LocationsInfo.SortedIndex.Length - 1; // Find highest element
            var middle = (low + high + 1) >> 1; // Find middle element
            var location = -1; // Return value -1 if not found

            do // Search for element
            {
                var compResult = 0;
                fixed(sbyte* city2 = _context.LocationsInfo.Locations[_context.LocationsInfo.SortedIndex[middle]].city)
                {
                    compResult = string.CompareOrdinal(city, new string(city2));
                }
                // if element is found at middle
                if (compResult == 0)
                    location = middle; // location is current middle

                // middle element is too high
                else if (compResult < 0)
                    high = middle - 1; // eliminate lower half
                else // middle element is too low
                    low = middle + 1; // eleminate lower half

                middle = (low + high + 1) >> 1; // recalculate the middle  
            } while ((low <= high) && (location == -1));

            if (location < 0)
                return new int[0];

            result.Add(location);
            
            //check forward closest elements
            for (var i = location+1; i < _context.LocationsInfo.SortedIndex.Length; i++)
            {
                var compResult = 0;
                fixed(sbyte* city2 = _context.LocationsInfo.Locations[_context.LocationsInfo.SortedIndex[i]].city)
                {
                    compResult = string.CompareOrdinal(city, new string(city2));
                }
                if (compResult == 0)
                    result.Add(i);
                else
                    break;
            }
            
            // check backward closet elements
            for (var i = location-1; i > -1; i--)
            {
                var compResult = 0;
                fixed(sbyte* city2 = _context.LocationsInfo.Locations[_context.LocationsInfo.SortedIndex[i]].city)
                {
                    compResult = string.CompareOrdinal(city, new string(city2));
                }
                if (compResult == 0)
                    result.Add(i);
                else
                    break;
            }
            
            return result.ToArray();
        }

        private unsafe Location Map(DAL.Binary.Model.Location dbLocation)
        {
            return new Location
            {
                Organization = new string(dbLocation.organization),
                City = new string(dbLocation.city),
                Country = new string(dbLocation.country),
                Postal = new string(dbLocation.postal),
                Region = new string(dbLocation.region),
                Latitude = dbLocation.latitude,
                Longitude = dbLocation.longitude
            };
        }
    }
}