using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace ZeusCachingSample
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class WrapResultAttribute : Attribute, IAsyncResultFilter
    {
        public bool WrapResult { get; set; } = true;

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var endpointFeature = context.HttpContext.Features.Get<IEndpointFeature>();
            var wrapResultAttr = endpointFeature.Endpoint.Metadata.GetMetadata<WrapResultAttribute>();
            if (wrapResultAttr?.WrapResult ?? false)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    var result = new WrappedResult(objectResult.Value);
                    context.Result = new ObjectResult(result);
                }
                else if (context.Result is JsonResult jsonResult)
                {
                    var result = new WrappedResult(jsonResult.Value);
                    context.Result = new ObjectResult(result);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class WrappedResult
    {
        public WrappedResult(object result)
        {
            Result = result;
        }

        public object Result { get; }
    }
}
