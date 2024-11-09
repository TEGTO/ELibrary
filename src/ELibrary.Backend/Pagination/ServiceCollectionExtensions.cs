using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pagination
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPagination(this IServiceCollection services, IConfiguration configuration)
        {
            var paginationConf = new PaginationOptions(int.Parse(configuration[PaginationConfiguration.MAX_PAGINATION_PAGE_SIZE] ?? "0"));
            services.AddSingleton(paginationConf);

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<PaginationOptions>();
            ValidatorOptions.Global.LanguageManager.Enabled = false;

            return services;
        }
    }
}
