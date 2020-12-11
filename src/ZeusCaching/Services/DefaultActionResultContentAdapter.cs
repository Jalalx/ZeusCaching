using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZeusCaching.Services
{
    public class DefaultActionResultContentAdapter : IActionResultContentAdapter
    {
        public IActionResult CreateResult(object content)
        {
            return new Utf8ContentResult(content);
        }

        public bool TryGetContent(IActionResult result, out object content)
        {
            if (result is ObjectResult objectResult)
            {
                content = objectResult.Value;
                return true;
            }
            else if (result is JsonResult jsonResult)
            {
                content = jsonResult.Value;
                return true;
            }
            else if (result is ContentResult contentResult)
            {
                content = contentResult.Content;
                return true;
            }
            else
            {
                content = null;
                return false;
            }
        }


        private sealed class Utf8ContentResult : ContentResult
        {
            public Utf8ContentResult(object content)
            {
                Content = content?.ToString();
                StatusCode = StatusCodes.Status200OK;
                ContentType = "application/json; charset=utf-8";
            }
        }
    }
}
