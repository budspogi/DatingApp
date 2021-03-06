using System.Net;
using System.Text;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
          //  services.AddDbContext<Data.DataContext> (x=>
          // x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
           
            services.AddDbContext<DataContext>(x => 
            {
                x.UseLazyLoadingProxies();
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
                
            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {

           // services.AddDbContext<Data.DataContext> (x=>
           //x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
           //  x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<DataContext>(x => 
            {
                x.UseLazyLoadingProxies();
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
           });

            ConfigureServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {



           // this should be in by order 25-05-2020 salvador
               
         //  services.AddDbContext<Data.DataContext> (x=>
         //  x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
           //services.AddDbContext<DataContextt> (x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
           services.AddControllers().AddNewtonsoftJson(opt => 
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }

           );
          //  services.AddMvc(option => option.EnableEndpointRouting = false);
        //   services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
           services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
             // added from lesson 78 28-05-2020
           services.AddAutoMapper(typeof(DatingRepository).Assembly);
           // end lesson 78
           services.AddScoped<IAuthRepository, AuthRepository>();
           services.AddScoped<IDatingRepository,DatingRepository>();
           services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
               AddJwtBearer(options => {
                   options.TokenValidationParameters=new TokenValidationParameters 
                   {
                     ValidateIssuerSigningKey =true, 
                     IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII
                     .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                     ValidateIssuer=false,
                     ValidateAudience=false
                   };
               });
           services.AddScoped<LogUserActivity>();
           //services.AddControllers();
          //MvcOptions.EnableEndpointRouting = 'false';
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
               app.UseExceptionHandler(builder => {
                  builder.Run(async context => {
                      context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null) 
                      {
                         context.Response.AddApplicationError(error.Error.Message);
                         await context.Response.WriteAsync(error.Error.Message); 
                      }
                  });
               }); 
            }

            //app.UseHttpsRedirection();
            
            //24-05-2020 - added salvador

            //24-05-2020 - salvador commented
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            
            app.UseDefaultFiles();
            app.UseStaticFiles();

           //  app.UseMvc();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index","Fallback");
            });
        }
    }
}
