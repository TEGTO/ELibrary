using Authentication;
using LibraryApi;
using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using Shared.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());

#endregion

builder.Services.AddDbContextFactory<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(Configuration.LIBRARY_DATABASE_CONNECTION_STRING)));

#region Project Services

builder.Services.AddSingleton<ILibraryEntityService<Author>, LibraryEntityService<Author>>();
builder.Services.AddSingleton<ILibraryEntityService<Genre>, LibraryEntityService<Genre>>();
builder.Services.AddSingleton<ILibraryEntityService<Book>, BookService>();
builder.Services.AddSingleton<IDatabaseRepository<LibraryDbContext>, DatabaseRepository<LibraryDbContext>>();

#endregion

builder.Services.ConfigureIdentityServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<LibraryDbContext>(CancellationToken.None);
}

app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
