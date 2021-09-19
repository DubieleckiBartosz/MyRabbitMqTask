using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiConsumer.Filters
{
    public class GetMessagesValidation : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var msqQuery = context.ActionArguments["query"] as SearchMessages;

            if (msqQuery is null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (msqQuery.ContentLength != default)
            {
                if (msqQuery.ContentLength < 5 || msqQuery.ContentLength > 6)
                {
                    context.Result = new BadRequestObjectResult("Nothing will be returned because content is between 5 and 6");
                    return;
                }
            }
            if (msqQuery.FromDate > msqQuery.ToDate)
            {
                context.Result = new BadRequestObjectResult("Date from cannot be greater than the date to");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
