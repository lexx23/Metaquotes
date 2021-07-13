using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Model;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Api
{
    [ApiController]
    [Route("[controller]/locations")]
    public class CityController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public CityController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public IEnumerable<Location> Get([Required] string city)
        {
            return _locationService.Search(city);
        }
    }
}