using Common.Model;

namespace Common.DataProviders
{
    public interface ILocationDataProvider
    {
        Location Get(int index);
        Location[] Search(string city);
    }
}