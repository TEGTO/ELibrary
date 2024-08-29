using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Middlewares;

namespace Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCustomInvalidModelStateResponseControllers(this IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(x => x.Value.Errors.Select(e => new ValidationFailure(x.Key, e.ErrorMessage)))
                        .ToList();
                    throw new ValidationException(errors);
                };
            });
            return services;
        }
        public static IServiceCollection AddSharedFluentValidation(this IServiceCollection services, Type type)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ExceptionMiddleware>();
            services.AddValidatorsFromAssemblyContaining(type);
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            return services;
        }
        public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var allowedOrigins = configuration.GetSection(Configuration.ALLOWED_CORS_ORIGINS).Get<string[]>() ?? [];

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
                    if (isDevelopment)
                    {
                        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                    }
                });
            });
            return services;
        }
    }
}
