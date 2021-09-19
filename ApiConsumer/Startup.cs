using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Background;
using ApiConsumer.DapperContext;
using ApiConsumer.Interfaces;
using ApiConsumer.Services;
using ApiConsumer.Services.Cache;
using ApiConsumer.Settings;
using ApiConsumer.Filters;
using ApiConsumer.Middleware;

namespace ApiConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitSettings>(Configuration.GetSection("RabbitSettings"));
            services.AddMemoryCache();
            services.AddSingleton<CacheService>();
            services.AddSingleton<AppDbContext>();
            services.AddHostedService<RabbitBackground>();
            services.AddHostedService<CacheBackground>();
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<GetMessagesValidation>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiConsumer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiConsumer v1"));
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
