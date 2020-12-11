using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public interface IZeusCachingProfileResolver
    {
        ZeusCachingProfileOptions GetDefaultOptions() => GetOptions(string.Empty);
        ZeusCachingProfileOptions GetOptions(string profileName);
    }
}
