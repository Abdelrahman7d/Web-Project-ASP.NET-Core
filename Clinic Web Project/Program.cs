using Core.Exceptions;
using Core.Resources;
using Data;
using Microsoft.EntityFrameworkCore;
using Repository;
using Serilog;
using Service.Mapper;
using Service.Service;

var builder = WebApplication.CreateBuilder(args);

//Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddControllers();
  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var configuration = builder.Configuration; // Access the configuration from the builder

    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {

        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
        ;
    });
});
builder.Services.AddSwaggerGen();

//BaseService and BaseRepo
builder.Services.AddScoped(typeof(BaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(BaseService<,>), typeof(BaseService<,>));

//Custom Services
builder.Services.AddScoped<ClinicService>();
builder.Services.AddScoped<DepartmentService>();

//Resources
builder.Services.AddSingleton<ResourceManagerService<ErrorMessages>>();

//File logger
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true)
);

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
