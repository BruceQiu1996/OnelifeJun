using LiJunSpace.Common.Dtos.Event;
using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.ServerMode.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class IndexComponentViewModel : ComponentBase
    {
        [Inject]
        public MyHttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        public List<RecordDto> Records { get; set; } = new List<RecordDto>();
        public List<EventDto> Events { get; set; } = new List<EventDto>();
        public int PageCount { get; set; }
        public bool IsDialogVisible { get; set; } = false;
        public string CurrentImage { get; set; }
        public MudPagination Pagination { get; set; }
        public int CurrentPage { get; set; } = 1;
        public bool IsLoading { get; set; }
        public bool HasMoreData { get; set; } = true;

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

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) 
            {
                await QueryTopEvents();
                await QueryRecordsByPageAsync(CurrentPage);
            }
        }

        public async Task OnLoadAsync()
        {
            int page  = CurrentPage + 1;
            await QueryRecordsByPageAsync(page);
        }

        public async Task JumpToTop() 
        {
            CurrentPage = 1;
            Records.Clear();
            await QueryRecordsByPageAsync(CurrentPage);
        }

        public void OpenUserDetail(string id)
        {
            Navigation.NavigateTo($"/profile/{id}", forceLoad: false, replace: false);
        }

        private async Task QueryTopEvents()
        {
            Events.Clear();
            IsDialogVisible = false;
            var resp = await HttpRequest.GetAsync($"{HttpRequestUrls.event_url_main}");
            if (resp != null)
            {
                Events = JsonSerializer
                    .Deserialize<List<EventDto>>(await resp.Content.ReadAsStringAsync(), MyHttpRequest._jsonSerializerOptions);

                StateHasChanged();
            }
        }

        private async Task QueryRecordsByPageAsync(int page)
        {
            if (IsLoading)
                return;

            IsDialogVisible = false;
            IsLoading = true;
            try
            {
                var resp = await HttpRequest.GetAsync($"{HttpRequestUrls.record}/{page}");
                if (resp != null)
                {
                    var result = JsonSerializer
                        .Deserialize<RecordQueryResultDto>(await resp.Content.ReadAsStringAsync(), MyHttpRequest._jsonSerializerOptions);

                    PageCount = result.AllCount / 10;
                    if (result.AllCount % 10 != 0)
                    {
                        PageCount++;
                    }
                    if (result.Records != null && result.Records.Count > 0)
                    {
                        Records.AddRange(result.Records);
                        CurrentPage = page;
                    }
                    else
                    {
                        HasMoreData = false;
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {

            }
            finally 
            {
                IsLoading = false;
            }
        }

        public void OpenRecordDetail(RecordDto record)
        {
            Navigation.NavigateTo($"/records/detail/{record.Id}", false);
        }

        public ElementReference scrollContainer;

        public async Task OnScroll()
        {
            var scrollHeight = await JSRuntime.InvokeAsync<double>("getScrollHeight", scrollContainer);
            var scrollTop = await JSRuntime.InvokeAsync<double>("getScrollTop", scrollContainer);
            var clientHeight = await JSRuntime.InvokeAsync<double>("getClientHeight", scrollContainer);

            if (scrollHeight - scrollTop <= clientHeight + 100) // 接近底部100px时加载
            {
                await OnLoadAsync();
            }
        }
    }
}
