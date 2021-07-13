using System.ComponentModel.DataAnnotations;
using Common.Model;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Api
{
    [ApiController]
    [Route("[controller]/location")]
    public class IpController : ControllerBase
    {
        private readonly IIpRangeService _ipRangeService;

        public IpController(IIpRangeService ipRangeService)
        {
            _ipRangeService = ipRangeService;
        }

        [HttpGet]
        public IpLocation Get([Required] string ip)
        {
            return _ipRangeService.Search(ip);
        }
    }
}