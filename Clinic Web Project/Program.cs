using Business;
using Business.Custom;
using Data.Query;
using Core.Core;
using Core.Exceptions;
using Core.Resources;
using Data;
using Microsoft.EntityFrameworkCore;
using Data.Repository;
using Serilog;
using Service.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//DbCotext configration
IConfiguration appSettingsConfiguration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var configuration = builder.Configuration;

string databaseType = appSettingsConfiguration["ClientSettings:DatabaseType"];

if (databaseType.Equals("SqlServer"))
{
   
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"));
    });
}
else if (databaseType.Equals("OracleServer"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseOracle(configuration.GetConnectionString("DefaultOracleConnection"));
    });
}




// Needed Classes and Layers
builder.Services.AddScoped(typeof(IRepositoryBase<,>), typeof(BaseRepository<,>));
builder.Services.AddScoped(typeof(BaseBusiness<,>), typeof(BaseBusiness<,>));
builder.Services.AddScoped(typeof(IQueryBuilder<,>), typeof(QueryBuilder<,>));

//Custom Business Classes
builder.Services.AddScoped<ClinicBusiness>();
builder.Services.AddScoped<DepartmentBusiness>();

//Resources
builder.Services.AddSingleton<IResourceManagerService,ResourceManagerService<ErrorMessages>>();

//File logger
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true)
);

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {

        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
        ;
    });
});

//Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Exception Handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
