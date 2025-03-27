using Sample.Commons.Abstracts;
using System.Net;
using System.Text;

namespace Sample.Host.Middlewares
{
    public class CustomExceptionMiddleware
    {
        readonly RequestDelegate next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BusinessException businessException)
            {
                await HandleExceptionAsync(context, businessException);
            }
            catch (AggregateException aggregateException)
            {
                await HandleAggregateExceptionAsync(context, aggregateException);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }

        }

        Task HandleExceptionAsync(HttpContext context, BusinessException exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.Conflict;

            return context.Response.WriteAsync(exception.Message);
        }

        Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception.InnerException == null)
            {
                context.Response.ContentType = "application/json";

                string result = string.Empty;

                var statusCode = context.Response.StatusCode != (int)HttpStatusCode.Redirect ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.Redirect;

                result = context.Response.StatusCode != (int)HttpStatusCode.Redirect ? "Runtime Error" : exception.Message;

                context.Response.StatusCode = statusCode;

                return context.Response.WriteAsync(result);
            }
            else
            {
                return HandleExceptionAsync(context, exception.InnerException);
            }
        }

        Task HandleAggregateExceptionAsync(HttpContext context, AggregateException aggregateException)
        {
            StringBuilder result = new();

            string nextLine = " <\br> ";

            var IsBusinessException = true;

            foreach (var exception in aggregateException.InnerExceptions)
            {
                if (exception.GetType() != typeof(BusinessException))
                {
                    IsBusinessException = false;
                }

                result.Append(exception.Message);

                result.Append(nextLine);
            }

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = IsBusinessException ? (int)HttpStatusCode.Conflict : (int)HttpStatusCode.Accepted;

            return context.Response.WriteAsync(result.ToString());
        }

    }
}
