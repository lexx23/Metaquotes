using Common.Model;

namespace Common.Services
{
    public interface ILocationService
    {
        Location[] Search(string city);
    }
}