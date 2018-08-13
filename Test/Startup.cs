using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Configuration;

namespace Test
{
    public class Startup
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * 对于Token签名需要一对公钥和私钥，
             * 不过IdentityServer为开发者提供了一个AddDeveloperSigningCredential()方法，它会帮我们搞定这个事，并默认存到硬盘中。
             * 当切换到生产环境时，还是得使用正儿八经的证书，更换为使用AddSigningCredential()方法
             */

            // var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var basePath = AppContext.BaseDirectory;

            InMemoryConfiguration.Configuration = this.Configuration;

            services.AddIdentityServer()
                // .AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                Configuration["Certificates:CerPath"]),
                Configuration["Certificates:Password"]))
                .AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources());

            // for QuickStart-UI
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // UseIdentityServer
            app.UseIdentityServer();

            // for QuickStart-UI
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            /*
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */
        }
    }
}
