using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using Phonebook.Report.Application.Events;
using Phonebook.Report.Infrastructure.Kafka;
using Phonebook.Report.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ProducerService>();
builder.Services.AddHostedService<ConsumerService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    AppDomain.CurrentDomain.GetAssemblies())
);

builder.Services.AddScoped<INotificationHandler<PersonListGenerateEvent>, PersonListGenerateEventHandler>();

builder.Services.AddDbContext<PhonebookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PhonebookConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Report API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
