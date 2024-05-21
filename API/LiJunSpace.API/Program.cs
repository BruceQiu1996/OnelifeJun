using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using LiJunSpace.API.Database;
using LiJunSpace.API.Helpers;
using LiJunSpace.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

namespace LiJunSpace.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>((hcontext, builder) =>
            {
                builder.AddApplicationContainer(Assembly.GetExecutingAssembly());
            });

            builder.Host.ConfigureServices((hostContext, services) =>
            {
                //配置跨域
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin()
                               //允许所有请求方法：Get,Post,Put,Delete
                               .AllowAnyMethod()
                               //允许所有请求头:application/json
                               .AllowAnyHeader();
                    });
                });
                services.AddSingleton<PasswordHelper>();
                services.AddEfCoreContext(hostContext.Configuration);
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "lijun v1",
                        Description = "lijun v1版本接口"
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                        {
                            new OpenApiSecurityScheme{Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "Bearer"}},new string[] { }
                        }
                        });
                });
                //添加认证
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hostContext.Configuration["Jwt:JwtSecret"]!)),
                        ValidateIssuer = true,
                        ValidIssuer = hostContext.Configuration["System:Name"],
                        ValidateAudience = true,
                        ValidAudience = hostContext.Configuration.GetSection("Jwt:Audience").Get<string>(),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(int.Parse(hostContext.Configuration["Jwt:ClockSkew"]!)), //过期时间容错值，解决服务器端时间不同步问题（秒）
                        RequireExpirationTime = true
                    };
                });
            }).UseSerilog((context, logger) =>
            {
                logger.WriteTo.Console();
            });
            builder.Services.AddControllers();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var app = builder.Build();
            app.Urls.Add("http://*:5000");
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(app.Configuration.GetSection("FileStorage:AvatarImagesLocation").Value!),
                RequestPath = new PathString("/api/avatars"),
                EnableDirectoryBrowsing = false
            });
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(app.Configuration.GetSection("FileStorage:RecordImagesLocation").Value!),
                RequestPath = new PathString("/api/record/images"),
                EnableDirectoryBrowsing = false
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            });
            app.MapControllers();

            app.Run();
        }
    }
}
