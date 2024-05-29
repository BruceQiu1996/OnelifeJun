using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace LiJunSpace
{
    public class Program
    {
        internal static string APIEndPoint;
        internal static string RecordNumber;//±¸°¸ºÅ
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddMudServices(config => 
            {
                config.SnackbarConfiguration.VisibleStateDuration = 3000;
            });
            builder.Services.AddSingleton<HttpRequest>();
            var host = builder.Build();
            APIEndPoint = host.Configuration.GetValue<string>("RemoteAPIHost")!;
            RecordNumber = host.Configuration.GetValue<string>("RecordNumber")!;

            await host.RunAsync();
        }
    }
}
