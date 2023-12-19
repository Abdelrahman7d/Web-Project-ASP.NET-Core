using Data;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Mapper;
using Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

//Department
builder.Services.AddScoped(typeof(BaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(BaseService<,>), typeof(BaseService<,>));

builder.Services.AddScoped<ClinicService>();
builder.Services.AddScoped<DepartmentService>();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
