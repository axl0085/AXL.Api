namespace AXL.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    try
        //    {
        //        await _next(httpContext);
        //    }
        //    catch (Exception ex)
        //    {

        //        await HandleExceptionAsync(httpContext, ex);
        //    }
        //}

        //private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        //{
        //    httpContext.Response.ContentType = "application/json";
        //    var res = httpContext.Response;
     

        //}
    }
}
