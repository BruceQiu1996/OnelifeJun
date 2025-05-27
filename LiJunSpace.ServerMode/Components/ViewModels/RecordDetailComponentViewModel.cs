using LiJunSpace.Common.Dtos.Record;
using LiJunSpace.ServerMode.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class RecordDetailComponentViewModel : ComponentBase
    {
        [Parameter]
        public string RecordId { get; set; }
        [Inject]
        public MyHttpRequest HttpRequest { get; set; }
        public RecordDtoWithComments Record { get; set; }
        public bool IsDialogVisible { get; set; }
        public string CurrentImage { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }
        public bool CanEdit { get; set; }
        [MaxLength(250)]
        public string Content { get; set; }
        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(RecordId))
                return;

            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.record_detail, RecordId));
            if (resp != null)
            {
                Record = JsonSerializer.Deserialize<RecordDtoWithComments>(await resp.Content.ReadAsStringAsync(), MyHttpRequest._jsonSerializerOptions);
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
            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.record_delete, RecordId));
            if (resp == null)
            {
                Snackbar.Add("删除错误", Severity.Error);
                return;
            }

            await JSRuntime.InvokeVoidAsync("window.history.back");
        }

        public void OnGesture(ServerMode.Components.Shared.GestureComponent.EnumGesture gesture)
        {
            if (Record.Images == null || Record.Images?.Count <= 1)
                return;

            switch (gesture)
            {
                case ServerMode.Components.Shared.GestureComponent.EnumGesture.Left:
                    var index = Record.Images.IndexOf(CurrentImage);
                    if (index == -1 || index == Record.Images.Count - 1)
                    {
                        return;
                    }

                    CurrentImage = Record.Images[index + 1];
                    break;
                case ServerMode.Components.Shared.GestureComponent.EnumGesture.Right:
                    var index1 = Record.Images.IndexOf(CurrentImage);
                    if (index1 == -1 || index1 == 0)
                    {
                        return;
                    }

                    CurrentImage = Record.Images[index1 - 1];
                    break;
                default:
                    break;
            }

            StateHasChanged();
        }

        public async Task PublishNewComment() 
        {
            if (string.IsNullOrEmpty(Content))
                return;

            var resp = await HttpRequest.PostAsync(HttpRequestUrls.record_comment, new CommentCreationDto() 
            {
                RecordId = Record.Id,
                Content = Content
            });
            if (resp == null)
            {
                Snackbar.Add("发布评论异常", Severity.Error);
                return;
            }

            var comment = JsonSerializer
                    .Deserialize<CommentDto>(await resp.Content.ReadAsStringAsync(), MyHttpRequest._jsonSerializerOptions);
            Record.Comments.Insert(0, comment);
            Content = null;

            StateHasChanged();
        }
    }
}
