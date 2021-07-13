using System;
using Common.DataProviders;
using Common.Model;

namespace DAL.Binary.DataProviders
{
    internal class IpRangeDataProvider : IIpRangeDataProvider
    {
        private readonly DatabaseContext _context;

        public IpRangeDataProvider(DatabaseContext context)
        {
            _context = context;
        }

        public IpRange Search(long ip)
        {
            if (ip < 0)
                throw new ArgumentException(nameof(ip));
            
            var index = BinarySearch(ip);
            if (index > -1 && index < _context.IpRanges.Length)
                return  Map(_context.IpRanges[index]);

            return null;
        }

        private int BinarySearch(long ip)
        {
            var result = -1;
            var low = 0;
            var high = _context.IpRanges.Length;
            long sip = 0;

            while (low <= high)
            {
                var middle = (low + high) >> 1;
                sip = _context.IpRanges[middle].ip_from;

                if (ip < sip)
                {
                    high = middle - 1;
                }
                else
                {
                    sip = _context.IpRanges[middle].ip_to;
                    if (ip > sip)
                    {
                        low = middle + 1;
                    }
                    else
                    {
                        result = middle;
                        break;
                    }
                }
            }
            
            return result;
        }

        private IpRange Map(Model.IpRange dbData)
        {
            return new IpRange
            {
                IpFrom = dbData.ip_from,
                IpTo = dbData.ip_to,
                LocationIndex = dbData.location_index
            };

        }
    }
}