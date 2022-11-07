using Azure.Identity;
using ElevatorApi;
using ElevatorApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault"]), new DefaultAzureCredential());


builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration["SqlConnectionString"]));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();


if (builder.Environment.IsDevelopment())
    app.UseSwagger();
else
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();