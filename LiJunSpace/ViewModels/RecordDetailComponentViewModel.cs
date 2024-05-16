using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class RecordDetailComponentViewModel : ComponentBase
    {
        [Parameter]
        public string RecordId { get; set; }
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        public RecordDto Record { get; set; }
        public bool IsDialogVisible { get; set; }
        public string CurrentImage { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }
        public bool CanEdit { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(RecordId))
                return;

            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.record_detail, RecordId));
            if (resp != null)
            {
                Record = JsonSerializer.Deserialize<RecordDto>(await resp.Content.ReadAsStringAsync(), HttpRequest._jsonSerializerOptions);
                StateHasChanged();
            }
            var user = await JSRuntime.InvokeAsync<string>("localStorageInterop.getItem", "userId");

            CanEdit = Record.PublisherId == user;
        }

        public void OpenImageDialog(string image)
        {
            IsDialogVisible = true;
            CurrentImage = image;
            StateHasChanged();
        }

        public bool Open { get; set; }
        public void OpenRecordDrawer()
        {
            IsDialogVisible = false;
            Open = true;
            StateHasChanged();
        }

        public void EditRecordAction() 
        {
            Navigation.NavigateTo($"/records/edit/{RecordId}", false);
        }

        public async Task DeleteRecordAction() 
        {
            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.record_delete,RecordId));
            if (resp == null)
            {
                Snackbar.Add("删除错误", Severity.Error);
                return;
            }

            await JSRuntime.InvokeVoidAsync("window.history.back");
        }
    }
}
