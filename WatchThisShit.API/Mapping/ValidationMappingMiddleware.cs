using FluentValidation;
using WatchThisShit.Contracts.Responses;

namespace WatchThisShit.API.Mapping;

public class ValidationMappingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(error => new ValidationResponse
                {
                    Message = error.ErrorMessage,
                    PropertyName = error.PropertyName
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}