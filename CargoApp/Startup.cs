using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpsPolicy;
using CargoApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CargoApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            {
                /*
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.Companies.Count() == 0)
                    {
                        Company c1 = new Company { Inn = "123123123123", Name = "TestCompany" };
                        Company c2 = new Company { Name = "Apple" };
                        db.Companies.AddRange(c1, c2);

                        //UserRegData u1 = new UserRegData { Login = "l1", Name = "l1", Password = "123", Salt = "0" };
                        Logistician l1 = new Logistician
                        {
                            Company = c1,
                            Login = "l1",
                            RegData = new UserRegData { Login = "l1", Name = "l1", Password = "123", Salt = "0" }
                        };
                        Logistician l2 = new Logistician
                        {
                            Company = c2,
                            Login = "l2",
                            RegData = new UserRegData { Login = "l2", Name = "l2", Password = "123", Salt = "0" }
                        };
                        Logistician l3 = new Logistician
                        {
                            Company = c1,
                            Login = "l3",
                            RegData = new UserRegData { Login = "l3", Name = "l3", Password = "123", Salt = "0" }
                        };

                        db.Logisticians.AddRange(l1, l2, l3);

                        Client client1 = new Client
                        {
                            RegData = new UserRegData { Login = "client", Name = "123", Password = "123", Salt = "0" }
                        };
                        Client client2 = new Client
                        {
                            RegData = new UserRegData { Login = "client2", Name = "123", Password = "123", Salt = "0" }
                        };
                        db.Clients.AddRange(client1, client2);

                        db.SaveChanges();
                    }

                    var clients = (from clientss in db.Clients.Include(c => c.RegData.Login)
                                  select clientss).ToList();

                    foreach (var client in clients)
                        Console.WriteLine($"{client.RegData.Name} ({client.Login})");
                }
                */
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DefaultConnection");
            // добавляем контекст MobileContext в качестве сервиса в приложение
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            //services.AddControllersWithViews();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
         /*   else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            */
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                /*endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");*/
            });
        }
    }
}
