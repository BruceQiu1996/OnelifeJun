using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
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
        
        }

        public void DeleteRecordAction() 
        {
        
        }
    }
}
