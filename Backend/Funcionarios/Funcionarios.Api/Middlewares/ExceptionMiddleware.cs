using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Funcionarios.Api.Middlewares
{
	public class ExceptionMiddleware(RequestDelegate next)
	{
		private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				await HandleValidationExceptionAsync(context, ex);
			}
		}

		private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			var errors = exception.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
			var response = JsonSerializer.Serialize(new { errors });

			return context.Response.WriteAsync(response);
		}
	}
}
