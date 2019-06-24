using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioAPI.Controllers;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InventarioAPI
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
            //Para declarar DTO's (cada uno)
            services.AddAutoMapper(options =>
            {
                options.CreateMap<CategoriaCreacionDTO, Categoria>();
                options.CreateMap<TipoEmpaqueCreacionDTO, TipoEmpaque>();
                options.CreateMap<ClienteCreacionDTO, Cliente>();
                options.CreateMap<ProveedorCreacionDTO, Proveedor>();
                options.CreateMap<InventarioCreacionDTO, Inventario>();
                options.CreateMap<ProductoCreacionDTO, Producto>();
                options.CreateMap<DetalleFacturaCreacionDTO, DetalleFactura>();
                options.CreateMap<DetalleCompraCreacionDTO, DetalleCompra>();
                options.CreateMap<EmailProveedorCreacionDTO, EmailProveedor>();
                options.CreateMap<CompraCreacionDTO, Compra>();
                
            });
            //Enlazar contexto con el nombre de la cadena de conexión
            services.AddDbContext<InventarioDBContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                    options.SerializerSettings.ReferenceLoopHandling
                      = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
