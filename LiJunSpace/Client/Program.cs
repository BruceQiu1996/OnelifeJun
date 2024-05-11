using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace LiJunSpace
{
    public class Program
    {
        internal static string APIEndPoint = "http://127.0.0.1:5052";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<HttpRequest>();
            await builder.Build().RunAsync();
        }
    }
}
