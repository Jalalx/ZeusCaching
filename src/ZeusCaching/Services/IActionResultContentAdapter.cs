using Microsoft.AspNetCore.Mvc;

namespace ZeusCaching.Services
{
    public interface IActionResultContentAdapter
    {
        bool TryGetContent(IActionResult actionResult, out object content);

        IActionResult CreateResult(object content);
    }
}
