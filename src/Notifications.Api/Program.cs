using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application;
using Notifications.Application.Interface;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Infrastructure.Configuration;
using Notifications.Infrastructure.Logging;
using Notifications.Infrastructure.Senders;

var builder = WebApplication.CreateBuilder(args);

// Cada sender registrado individualmente — SRP no DI também
// builder.Services.AddKeyedScoped<INotificationSender, EmailSender>("Email");
// builder.Services.AddKeyedScoped<INotificationSender, SmsSender>("Sms");
// builder.Services.AddKeyedScoped<INotificationSender, PushSender>("push");

builder.Services.AddScoped<INotificationSender, EmailSender>();
builder.Services.AddScoped<INotificationSender, SmsSender>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services
    .AddOptions<SendGridOptions>()
    .Bind(builder.Configuration.GetSection(SendGridOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<NotificationLogger>();
builder.Services.AddApplication();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/notifications", async (INotificationOrchestrator orchestrator, [FromBody] NotificationRequest request,
    CancellationToken ct) =>
{
    var success = await orchestrator.NotifyAsync(request, ct);
    return success ? Results.Ok(new { message = "Enviado." }) : Results.Json(new { message = "Falha no envio." }, statusCode: 502);
});

app.Run();
