using LPMessengerAPI.Authorization;
using LPMessengerAPI.DtoModel;
using LPMessengerAPI.Model;
using LPMessengerAPI.Service;
using LPMessengerAPI.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Options;

namespace LPMessengerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddDbContext<lpmessengerdbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))));
            
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LPMessengerAPI", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
                //c.AddSecurityDefinition("basic", new bas { Type = "basic" });
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>() { { "basic", new string[] { } } });
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "LPMessengerAPI", Version = "v1" });
            });

            //services.AddCors(o => o.AddPolicy(MyAllowSpecificOrigins,
            //            builder =>
            //            {
            //                builder.WithOrigins("https://apiui.lp-its.com")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader();
            //            }));
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
           // services.AddControllers().AddNewtonsoftJson(); 

            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMailerService, MailerService>();
            services.AddTransient<IGroupMasterService, GroupMasterService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<ISmsSendService, SmsSendService>();
            services.AddTransient<IServiceAvailableService, ServiceAvailableService>();
            services.AddTransient<ISenderConfigService, SenderConfigService>();
            services.AddTransient<IExternalService, ExternalService>();
            services.AddTransient<IWhatsAppSendService, WhatsAppSendService>();
            services.AddTransient<IWhatsAppService, WhatsAppService>();
            services.AddScoped<IUserService, UserService>(); 
            services.AddScoped<IApiDocumentationService, ApiDocumentationService>();
            services.AddScoped<ISmsTransactionMessageStatusService, SmsTransactionMessageStatusService>();
            services.AddScoped<IFileAttachmentService, FileAttachmentService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LPMessengerAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
