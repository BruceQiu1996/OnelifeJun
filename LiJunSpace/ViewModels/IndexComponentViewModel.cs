using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class IndexComponentViewModel : ComponentBase
    {
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        public List<RecordDto> Records { get; set; } = new List<RecordDto>();
        public int PageCount { get; set; }
        public void NewRecord()
        {
            Navigation.NavigateTo("/newRecord", forceLoad: false, replace: true);
        }

        protected async override Task OnInitializedAsync()
        {
            Records.Clear();
            var resp = await HttpRequest.GetAsync(HttpRequestUrls.record);
            if (resp != null)
            {
                var result = JsonSerializer
                    .Deserialize<RecordQueryResultDto>(await resp.Content.ReadAsStringAsync());

                PageCount = result.AllCount / 10;
                if (result.AllCount % 10 != 0) 
                {
                    PageCount++;
                }
                if (result.Records != null)
                    Records = result.Records;

                StateHasChanged();
            }
        }
    }
}
