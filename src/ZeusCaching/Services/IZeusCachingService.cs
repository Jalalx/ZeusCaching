using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ZeusCaching.Services
{
    public interface IZeusCachingService
    {
        Task ProcessRequestAsync(ZeusCachingContext context, ActionExecutionDelegate next);
    }
}
