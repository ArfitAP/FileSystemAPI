using FileSystemAPI.Persistence;
using FileSystemAPI.Application;
using FileSystemAPI.Infrastructure;
using Serilog;

namespace FileSystemAPI.Api
{
    public static class StartupExtensions
    {
        // Add services to the container.
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);           

            builder.Services.AddControllers();
          
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEasyCaching(options =>
            {
                //use memory cache
                options.UseInMemory(builder.Configuration, "default", "easycaching:inmemory");
            });

            builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

            return builder.Build();
        }

        // Configure the HTTP request pipeline.
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            return app;
        }
    }
}
