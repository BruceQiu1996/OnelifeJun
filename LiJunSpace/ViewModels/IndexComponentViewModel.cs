using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;

namespace LiJunSpace.ViewModels
{
    public class IndexComponentViewModel : ComponentBase
    {
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }

        public void NewRecord() 
        {
            Navigation.NavigateTo("/newRecord", forceLoad: false, replace: true);
        }
    }
}
