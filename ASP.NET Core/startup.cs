using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LesonAsp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //service тут
            services.AddMvc();
             services.AddScoped<Iperson, Person>(); // один экземпляр сервиса создается на весь запрос
             services.AddTransient<Iperson, Person>(); //каждый запрос будет создаватся новый экземпляр
             services.AddSingleton<Iperson, Person>(); // один создается на все время жизни обькта
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage(); //
            app.UseStatusCodePages(); //использовать отображает коды страниц
            app.UseMvcWithDefaultRoute(); // если страницы нету то отображать начальный путь
            app.UseStaticFiles();// использовать статичиские файлы

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //режим разработки
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!"); // выводит техт на странице
            });
        }
    }
}
