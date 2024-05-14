using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class IndexComponentViewModel : ComponentBase
    {
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        public List<RecordDto> Records { get; set; } = new List<RecordDto>();
        public int PageCount { get; set; }
        public bool IsDialogVisible { get; set; } = false;
        public string CurrentImage { get; set; }
        [Parameter]
        public int Page { get; set; }
        public MudPagination Pagination { get; set; }

        public void OpenImageDialog(string image)
        {
            CurrentImage = image;
            IsDialogVisible = true;
            StateHasChanged();
        }

        public void NewRecord()
        {
            Navigation.NavigateTo("/newRecord", forceLoad: false, replace: false);
        }

        protected async override Task OnInitializedAsync()
        {
            await QueryRecordsByPageAsync(Page);
        }

        protected async override Task OnParametersSetAsync()
        {
            await QueryRecordsByPageAsync(Page);
            if (Pagination != null)
            {
                Pagination.Selected = Page;
            }
        }

        public async Task OnSelectPageAsync(int page)
        {
            Navigation.NavigateTo($"/records/{page}", forceLoad: false, replace: false);
        }

        private async Task QueryRecordsByPageAsync(int page) 
        {
            Records.Clear();
            IsDialogVisible = false;
            var resp = await HttpRequest.GetAsync($"{HttpRequestUrls.record}/{page}");
            if (resp != null)
            {
                var result = JsonSerializer
                    .Deserialize<RecordQueryResultDto>(await resp.Content.ReadAsStringAsync(), HttpRequest._jsonSerializerOptions);

                PageCount = result.AllCount / 10;
                if (result.AllCount % 10 != 0)
                {
                    PageCount++;
                }
                if (result.Records != null)
                    Records = result.Records;

                Records.ForEach(record =>
                {
                    for (int i = 0; i < record.Images.Count; i++)
                    {
                        record.Images[i] = $"{Program.APIEndPoint}/api/record/images/{record.Images[i]}";
                    }

                    record.publisherAvatar = $"{Program.APIEndPoint}/api/avatars/{record.publisherAvatar}";
                    if (!string.IsNullOrEmpty(record.Content))
                    {
                        record.Content = record.Content.Length > 50 ? record.Content.Substring(50) : record.Content;
                    }
                });

                StateHasChanged();
            }
        }
    }
}
