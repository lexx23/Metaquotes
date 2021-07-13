using System;
using Common.DataProviders;
using Common.Model;
using Common.Services;

namespace Services
{
    public class IpRangeService : IIpRangeService
    {
        private readonly ILocationDataProvider _locationDataProvider;
        private readonly IIpRangeDataProvider _ipRangeDataProvider;
        private readonly ICacheService<IpLocation, string> _cacheService;

        public IpRangeService(IIpRangeDataProvider ipRangeDataProvider, ILocationDataProvider locationDataProvider, ICacheService<IpLocation, string> cacheService)
        {
            _ipRangeDataProvider = ipRangeDataProvider;
            _locationDataProvider = locationDataProvider;
            _cacheService = cacheService;
        }

        public IpLocation Search(string ip)
        {
            return _cacheService.Cache(ip, () =>
            {
                var ipRecord = _ipRangeDataProvider.Search(Ip2Long(ip));
                if (ipRecord == null)
                    return null;
                var location = _locationDataProvider.Get((int) ipRecord.LocationIndex);

                return new IpLocation
                {
                    IpRange = ipRecord,
                    Location = location
                };
            });
        }

        private long Ip2Long(string ip)
        {
            var p = ip.Split('.');
            if (p.Length != 4)
                throw new ArgumentException(nameof(ip));

            foreach (var pp in p)
            {
                if (pp.Length > 3)
                    throw new ArgumentException(nameof(ip));
                if (!int.TryParse(pp, out var value) || value > 255)
                {
                    throw new ArgumentException(nameof(ip));
                }
            }

            var bip1 = long.TryParse(p[0], out var ip1);
            var bip2 = long.TryParse(p[1], out var ip2);
            var bip3 = long.TryParse(p[2], out var ip3);
            var bip4 = long.TryParse(p[3], out var ip4);

            if (!bip1 || !bip2 || !bip3 || !bip4
                || ip4 > 255 || ip1 > 255 || ip2 > 255 || ip3 > 255
                || ip4 < 0 || ip1 < 0 || ip2 < 0 || ip3 < 0)
            {
                throw new ArgumentException(nameof(ip));
            }

            var p1 = ((ip1 << 24) & 0xFF000000);
            var p2 = ((ip2 << 16) & 0x00FF0000);
            var p3 = ((ip3 << 8) & 0x0000FF00);
            var p4 = ((ip4 << 0) & 0x000000FF);
            return ((p1 | p2 | p3 | p4) & 0xFFFFFFFFL);
        }
    }
}