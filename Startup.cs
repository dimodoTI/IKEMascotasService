using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MascotasApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MascotasApi.Helpers;
using MediatR;
using mails;
using Microsoft.Extensions.Logging;



namespace MascotasApi
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

            services.AddMediatR(typeof(Startup));
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddAuthorization();
            services.AddControllers();
            services.AddOData();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var mailToken = appSettings.mailToken;
            var mailVeterinario = appSettings.mailVeterinario;
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            services.AddScoped<IikeMailService, IkeMailService>(x => new IkeMailService(new System.Net.Http.HttpClient(), mailToken));

            services.AddDbContext<MascotasContext>(opt =>
                            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();


            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                            endpoints.Select().Expand().Filter().OrderBy().Count().MaxTop(300);
                            endpoints.MapODataRoute("odata", "odata", GetEdmModel());
                        });
        }
        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Mascotas>("MascotasQuery");
            odataBuilder.EntitySet<Razas>("RazasQuery");
            odataBuilder.EntitySet<MascotasTipo>("MascotasTipoQuery");
            odataBuilder.EntitySet<Vacunas>("VacunasQuery");
            odataBuilder.EntitySet<Calendario>("CalendarioQuery");
            odataBuilder.EntitySet<MascotasVacunas>("MascotasVacunasQuery");
            odataBuilder.EntitySet<Configuracion>("ConfiguracionQuery");
            odataBuilder.EntitySet<Puestos>("PuestosQuery");
            odataBuilder.EntitySet<Tramos>("TramosQuery");
            odataBuilder.EntitySet<Reservas>("ReservasQuery");
            odataBuilder.EntitySet<Atenciones>("AtencionesQuery");
            odataBuilder.EntitySet<Adjuntos>("AdjuntosQuery");

            return odataBuilder.GetEdmModel();
        }
    }
}
