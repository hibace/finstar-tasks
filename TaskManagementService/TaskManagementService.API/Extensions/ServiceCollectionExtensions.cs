using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.Commands;
using TaskManagementService.Core.Interfaces;
using TaskManagementService.Infrastructure.Data;
using TaskManagementService.Infrastructure.MessageBus;
using TaskManagementService.Infrastructure.Repositories;

namespace TaskManagementService.API.Extensions
{
    /// <summary>
    /// Расширения для конфигурации сервисов
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет сервисы приложения
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация</param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Add Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));

            // Add AutoMapper
            services.AddAutoMapper(typeof(CreateTaskCommand).Assembly);

            // Add FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(CreateTaskCommand).Assembly);

            // Add CAP
            services.AddCap(capOptions =>
            {
                capOptions.UseEntityFramework<ApplicationDbContext>();
                capOptions.UsePostgreSql(connectionString);
                capOptions.UseRabbitMQ(o =>
                {
                    o.HostName = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
                    o.UserName = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
                    o.Password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";
                });

                capOptions.UseDashboard(d =>
                {
                    d.PathMatch = "/cap-dashboard";
                    d.StatsPollingInterval = 5000;
                });

                // Настройка повторных попыток
                capOptions.FailedRetryCount = 5;
                capOptions.FailedRetryInterval = 60;
            });

            // Add Message Bus
            services.AddScoped<IMessageBus, CapMessageBus>();

            return services;
        }
    }
} 