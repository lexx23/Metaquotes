using Common.Model;

namespace Common.Services
{
    public interface IIpRangeService
    {
        IpLocation Search(string ip);
    }
}