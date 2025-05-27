using MudBlazor.Services;
using LiJunSpace.ServerMode.Components;
using LiJunSpace.ServerMode.Helpers;

public class Program
{
    internal static string APIEndPoint;
    internal static string RecordNumber;//±¸°¸ºÅ
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add MudBlazor services
        builder.Services.AddMudServices();
        builder.WebHost.ConfigureKestrel(serverOptions => {
        });
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.VisibleStateDuration = 3000;
        });

        builder.Services.AddScoped<MyHttpRequest>();
        var app = builder.Build();
        APIEndPoint = app.Configuration.GetValue<string>("RemoteAPIHost")!;
        RecordNumber = app.Configuration.GetValue<string>("RecordNumber")!;
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();


        app.UseAntiforgery();

        app.UseStaticFiles();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}