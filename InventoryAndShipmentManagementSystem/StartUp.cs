using InventoryBAL.Implementation;
using InventoryBAL.Interface;
using InventoryRepository.Implementation;
using InventoryRepository.Interface;
using InventoryUtility;
using InventoryUtility.Interface;
using LISCareDataAccess.IInventoryDbContext;
using Microsoft.EntityFrameworkCore;

namespace InventoryAndShipmentManagementSystem
{
    public class StartUp
    {
        public IConfiguration Configuration { get; }
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IProduct, ProductBAL>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductLoggers, ProductLoggers>();

            // Configure database connection strings.
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ConstantResources.InventoryDbConnection)));

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(op => op.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API"));
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors("AllowSpecificOrigin"); // This must be placed between UseRouting and UseEndpoints


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
