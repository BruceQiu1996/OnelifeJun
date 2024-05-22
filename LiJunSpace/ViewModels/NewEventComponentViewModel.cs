using LiJunSpace.Common.Dtos.Event;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace LiJunSpace.ViewModels
{
    public class NewEventComponentViewModel : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [StringLength(50, ErrorMessage = "标题长度需小于等于50")]
        public string Title { get; set; }
        public DateTime? Time { get; set; } = DateTime.Now;
        public bool Desc { get; set; }
        public bool UseSeconds { get; set; }
        public bool ShowOnMain { get; set; }
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public async Task PublishNewEventAsync() 
        {
            if (string.IsNullOrEmpty(Title) || Title.Length > 50) 
            {
                Snackbar.Add("标题不能为空或者超过50长度",Severity.Info);
                return;
            }

            if (Time == null) 
            {
                Snackbar.Add("时间不能为空", Severity.Info);
                return;
            }

            var resp = await HttpRequest.PostAsync(HttpRequestUrls.event_url, new EventCreationDto()
            {
                Title = Title,
                Time = Time.Value,
                Desc = Desc,
                ShowOnMainpage = ShowOnMain,
            });

            if (resp == null)
            {
                Snackbar.Add("发布错误", Severity.Error);
            }
            else 
            {
                await JSRuntime.InvokeVoidAsync("window.history.back");
            }
        }
    }
}
