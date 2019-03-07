using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Easy.Tall.UserCenter.DocApi
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940</param>
        /// <returns>DI容器</returns>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiDocServer", new Info
                {
                    Title = "Api文档服务器",
                    Version = "v1",
                    Description = "提供中心化的API文档查看工具，以下列出本服务的接口。"
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c => { c.RouteTemplate = "docs/{documentName}/swagger.json"; });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.

            app.UseStaticFiles();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                var paths = Directory.GetFileSystemEntries(env.WebRootPath + "/apiDocs");
                foreach (var path in paths)
                {
                    var filename = Path.GetFileName(path);
                    c.SwaggerEndpoint($"/apiDocs/{filename}", filename);
                }

                //c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("ApiDocServer/swagger.json", "ApiDocServer");
            });

            app.UseMvc();
        }
    }
}