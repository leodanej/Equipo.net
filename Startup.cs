using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace bakend
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // requires using Microsoft.AspNet.OData.Extensions;
    
            services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(Configuration.GetConnectionString("DbCon")));
        // Se agrega CORS
        services.AddCors(options =>
                    {
                        options.AddPolicy( name: MyAllowSpecificOrigins,
                                        builder =>
                                        {
                                            builder.WithOrigins("http://localhost"         )
                                                                .AllowAnyOrigin()
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod();
                                        });
                    });
 services.AddSwaggerGen();
        // se agrega Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters{

                                        ValidateIssuer = true,
                                        ValidateAudience = true,
                                        ValidateLifetime = true,
                                        ValidateIssuerSigningKey = true,
                                        ValidIssuer = Configuration["Jwt:Issuer"],
                                        ValidAudience = Configuration["Jwt:Audience"],
                                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                                        ClockSkew = TimeSpan.Zero 
                                    });

               
            
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
               app.UseSwagger();

               app.UseSwaggerUI(c =>
            {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

          //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
