using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MiniMartApi.Interfaces;
using MiniMartApi.Repositories;
using Swashbuckle.Swagger;

namespace MiniMartApi
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

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<IStoreRepository, StoreRepository>();
            services.AddSingleton<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IVoucherRepository, VoucherRepository>();
            services.AddSingleton<ICartRepository, CartRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Description = @"
                    The system should:
                    Be able to setup all data from a simple GET
                    • Be able to query available stores at a certain time in the day and return only those that
                    apply
                    • Be able to query all available products, across stores, with their total stock.
                    • Be able to query if a product is available, at a certain store, and return that product's
                    info
                    • Be able to query available products for a particular store
                    • Be able to manage a simple virtual cart(add/remove from it). It cannot allow to add a
                    product that has NO stock
                    • Be able to check the validity of a Voucher code on said virtual cart.Calculate discounts
                    and return both original and discounted prices",Title= "MiniMart Api" });
            }   );
        }






        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
