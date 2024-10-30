using Authentication;
using LibraryApi;
using LibraryApi.Services;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Cors

bool.TryParse(builder.Configuration[Configuration.USE_CORS], out bool useCors);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (useCors)
{
    builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());
}

#endregion

builder.Services.AddDbContextFactory<LibraryShopDbContext>(builder.Configuration.GetConnectionString(Configuration.LIBRARY_DATABASE_CONNECTION_STRING), "LibraryApi");

#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services

builder.Services.AddSingleton<ILibraryEntityService<Book>, BookService>();
builder.Services.AddSingleton<ILibraryEntityService<Author>, AuthorService>();
builder.Services.AddSingleton<ILibraryEntityService<Genre>, LibraryEntityService<Genre>>();
builder.Services.AddSingleton<ILibraryEntityService<Publisher>, LibraryEntityService<Publisher>>();

builder.Services.AddPaginationConfiguration(builder.Configuration);
builder.Services.AddRepositoryPatternWithResilience<LibraryShopDbContext>(builder.Configuration);
#endregion

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.AddMemoryCache();

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<LibraryShopDbContext>(CancellationToken.None);
}

if (useCors)
{
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseIdentity();

app.MapControllers();

app.Run();

public partial class Program { }