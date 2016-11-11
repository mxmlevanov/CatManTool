using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ActionConstraints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MLevanov_CMTool.Models;
using MLevanov_CMTool.ViewModels;
using Newtonsoft.Json.Serialization;


namespace MLevanov_CMTool
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public static IConfigurationRoot Configuration;
        public Startup(IApplicationEnvironment appEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnvironment.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)

        {
            
            services.AddMvc().AddJsonOptions(opp=>opp.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddEntityFramework().AddSqlServer().AddDbContext<CmToolContext>();
            services.AddTransient<CmToolSeedData>();
            services.AddScoped<ICmToolRepository, CmToolRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, CmToolSeedData seeder)
        {
           // app.UseDefaultFiles();
            app.UseStaticFiles();
            Mapper.Initialize(config =>
            {
                config.CreateMap<Good, GoodViewModel>().ReverseMap();
                config.CreateMap<GoodsClass, GoodsClassViewModel>().ReverseMap();
                config.CreateMap<ShelvesViewModel, Shelve>()
                    .ForMember(s => s.ShelveGoodsClass, opt => opt.ResolveUsing(mod => new GoodsClass() {Category = mod.ShelveGoodsClass}))
                    .ForMember(s => s.ShelveStore, opt => opt.ResolveUsing(mod => new Store() {StoreCode = mod.StoreCode,StoreName = mod.ShelveStore}));
                config.CreateMap<ProductViewModel, Good>()
                    .ForMember(s => s.GoodsClass, opt => opt.ResolveUsing(mod => new GoodsClass() {Category = mod.GoodsClass}));
                config.CreateMap<SaleViewModel, Sale>().ReverseMap();
                
            });  

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" });

            });
           // app.UseIISPlatformHandler();
          //  seeder.EnsureSeedingData();

        }

        // Entry point for the application.
       public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
