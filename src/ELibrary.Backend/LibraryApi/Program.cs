using Authentication;
using LibraryShopEntities;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using Shared.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());

#endregion

builder.Services.AddDbContextFactory<LibraryShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(Configuration.LIBRARY_DATABASE_CONNECTION_STRING),
        b => b.MigrationsAssembly("LibraryApi")));

#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services

builder.Services.AddSingleton<ILibraryEntityService<Book>, BookService>();
builder.Services.AddSingleton<ILibraryEntityService<Author>, LibraryEntityService<Author>>();
builder.Services.AddSingleton<ILibraryEntityService<Genre>, LibraryEntityService<Genre>>();
builder.Services.AddSingleton<ILibraryEntityService<Publisher>, LibraryEntityService<Publisher>>();
builder.Services.AddSingleton<ILibraryEntityService<CoverType>, LibraryEntityService<CoverType>>();
builder.Services.AddSingleton<IDatabaseRepository<LibraryShopDbContext>, DatabaseRepository<LibraryShopDbContext>>();

#endregion

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<LibraryShopDbContext>(CancellationToken.None);
}

app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
