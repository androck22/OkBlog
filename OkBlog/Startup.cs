using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OkBlog.Data;
using OkBlog.Data.FileManager;
using OkBlog.Data.Repository;
using OkBlog.Models.Db;
using OkBlog.Validation;
using System.Net;

namespace OkBlog
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDbContext>(options => options.UseSqlite(connection));

            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
            {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
          .AddEntityFrameworkStores<BlogDbContext>()
          .AddDefaultTokenProviders();

            services.AddTransient<IRepository, Repository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IFileManager, FileManager>();

            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var logger = appBuilder.ApplicationServices.GetRequiredService<ILogger<Startup>>();
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature.Error is not null)
                    {
                        logger.LogError(feature.Error, "Exception Here!");
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        //context.Request.Path = "/Home/500.cshtml";

                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                        {
                            error = "Something went wrong!",
                            detail = feature.Error.Message
                        }));
                    }
                });
            });

            env.EnvironmentName = "Production";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                //app.UseExceptionHandler("/Error");
            }

            //app.Run(async (context) =>
            //{
            //    int zero = 0;
            //    int result = 4 / zero;
            //    await context.Response.WriteAsync($"Page not found");
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Posts}/{action=Index}/{id?}");
            });
        }
    }
}
