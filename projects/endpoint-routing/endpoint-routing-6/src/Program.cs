using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace LinkGeneratorSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory logger)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                var linkGenerator = context.RequestServices.GetService<LinkGenerator>();

                var url = linkGenerator.GetPathByAction(
                       controller: "Hello",
                       action: "World",
                       values: new
                       {
                           name = "Annie"
                       }
                   );

                var url2 = linkGenerator.GetPathByAction(
                       controller: "Hello",
                       action: "Goodbye",
                       values: new
                       {
                           age = 55
                       }
                   );

                var url3 = linkGenerator.GetPathByAction(
                       controller: "Hello",
                       action: "CallMe"
                   );

                var url4 = linkGenerator.GetPathByAction(
                       controller: "Greet",
                       action: "Index",
                       values: new
                       {
                           isNice = true
                       }
                       );

                var url5 = linkGenerator.GetPathByAction(
                       controller: "Wave",
                       action: "Away",
                       values: new
                       {
                           danger = "see",
                           ahead = "soon"
                       }
                   );

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($@"Generated Url: 
{url}
{url2}
{url3}
{url4}
{url5}
");
            });

            app.UseMvc();
        }
    }

    [Route("[controller]")]
    public class HelloController
    {
        [HttpGet("{name}")]
        public ActionResult World(string name) => null;

        [HttpGet("Goodbye/{age:int}")]
        public ActionResult Goodbye(int age) => null;

        [HttpGet("[action]/{byYourName?}")]
        public ActionResult CallMe(string byYourName) => null;
    }

    [Route("Greet/{isNice:bool}")]
    public class GreetController
    {
        public ActionResult Index() => null;
    }

    public class WaveController
    {
        [Route("Wave-Away/{danger:required}/{ahead:required}")]
        public ActionResult Away(string danger, string ahead) => null;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}