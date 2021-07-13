using System;

namespace Common.Services
{
    public interface ICacheService<TRes, Tkey>
    {
        TRes Cache(Tkey key ,Func<TRes> execute);
    }
}