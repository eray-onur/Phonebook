using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Phonebook.Directory.Application.Events;
using Phonebook.Directory.Infrastructure.Kafka;
using Phonebook.Directory.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ProducerService>();

builder.Services.AddDbContext<DirectoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DirectoryDb")), ServiceLifetime.Scoped);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    AppDomain.CurrentDomain.GetAssemblies())
);
builder.Services.AddScoped<INotificationHandler<PersonListGenerateEvent>, PersonListGenerateEventHandler>();

builder.Services.AddHostedService<ConsumerService>();





// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Directory API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Run("http://*:8081");
app.Run();