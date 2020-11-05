using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CustomMapping.Filters
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = false)]
    public class ApiResultFilterAttribute:ResultFilterAttribute
    {
        private readonly Type _destinationType;
        private readonly bool _isEnumerable;

        public ApiResultFilterAttribute(Type destinationType, bool isEnumerable=false)
        {
            _destinationType = destinationType;
            _isEnumerable = isEnumerable;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!(context.Result is ObjectResult))
                await next();

            if (context.Result == null)
            {
                await next();
                return;
            }

            var model = context.Result as ObjectResult;

            if (model == null) { 
                await next();
                return;
            }

            var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();

            if (mapper == null)
                throw new ArgumentNullException("Mapper Not Found");

            if (model.StatusCode < 200 || model.StatusCode >= 300)
            {
                await next();
                return;
            }

            if (!_isEnumerable)
                model.Value = mapper.Map(model.Value, model.Value.GetType(), _destinationType);
            else
            {
                var result = model.Value as IEnumerable<object>;

                model.Value = result.Select(v => mapper.Map(v, v.GetType(), _destinationType));
            }

            await next();
        }
    }
}
