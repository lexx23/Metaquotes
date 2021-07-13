using Common.Model;

namespace Common.DataProviders
{
    public interface IIpRangeDataProvider
    {
        IpRange Search(long ip);
    }
}