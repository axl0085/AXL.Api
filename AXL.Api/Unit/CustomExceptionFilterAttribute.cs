using AXL.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AXL.Api.Utiy
{
    public class CustomExceptionFilterAttribute:ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> logger;
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> Logger)
        {
            logger = Logger;
        }
        public override void OnException(ExceptionContext context)
        {
            Exception  exception = context.Exception;
            JsonResult result;
            if (exception  is BusinessException)
            {
                result = new JsonResult(exception.Message) {
                    StatusCode = 400
                };
            }
            else
            {
                result = new JsonResult("未知异常错误！")
                {
                    StatusCode = 500
                };
                logger.LogError(exception,"服务器处理出错");
            }
            context.Result = result;
        }
    }
}
