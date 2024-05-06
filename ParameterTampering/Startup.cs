using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParameterTampering.Entities;

namespace ParameterTampering
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
            services.AddEntityFrameworkSqlite().AddDbContext<StoreDBContext>();

            services.AddAntiforgery(options =>
            {
                options.Cookie.HttpOnly = true; // is set by default
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict; // is set by default
                options.SuppressXFrameOptionsHeader = false;
            });

            // ignores methods like GET and HEAD
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add(
            //        "Content-Security-Policy",
            //        "default-src 'self'; " +
            //        "img-src 'self'; " +
            //        "font-src 'self'; " +
            //        "style-src 'self'; " +
            //        "script-src 'self' 'nonce-S0pIQkVGa3V5YnJneXVic2xiZnlvOEdCT1lVJDNlZA=='; " +
            //        "frame-src 'self'; " +
            //        "form-action 'self'; " +
            //        "frame-ancestors 'none'; " +
            //        "connect-src 'self';");
            //    await next();
            //});

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            //    context.Response.Headers.Add("X-Frame-Options", "DENY");
            //    context.Response.Headers.Add("Referrer-Policy", "same-origin");
            //    await next();
            //});

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
