using LiJunSpace.Common.Dtos.Account;
using LiJunSpace.Helpers;
using LiJunSpace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class EditProfileComponentViewModel : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [MaxLength(16, ErrorMessage = "昵称最多16长度")]
        public string DisplayName { get; set; }
        [MaxLength(50, ErrorMessage = "签名最多50长度")]
        public string? Signature { get; set; }
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "邮箱格式不正确")]
        public string? Email { get; set; }
        public bool OpenEmailNotice { get; set; }
        public bool Sex { get; set; }
        public DateTime? Birthday { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var userId = await JSRuntime.InvokeAsync<string>("localStorageInterop.getItem", "userId");
            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.profile, userId));
            if (resp != null)
            {
                var profile = JsonSerializer.Deserialize<UserProfileDto>(await resp.Content.ReadAsStringAsync(),
                    HttpRequest._jsonSerializerOptions)!;

                DisplayName = profile.DisplayName;
                Signature = profile.Signature;
                Email = profile.Email;
                OpenEmailNotice = profile.OpenEmailNotice;
                Birthday = DateTime.Parse(profile.Birthday);
                Sex = profile.Sex;
                StateHasChanged();
            }
        }

        public async void CancelEdit()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var result = DialogService.Show<CancelDialog>("取消编辑", options);

            var data = await result.Result;
            if (!data.Cancelled)
            {
                await JSRuntime.InvokeVoidAsync("window.history.back");
            }
        }

        public async Task SaveAsync()
        {
            var resp = await HttpRequest.PostAsync(HttpRequestUrls.account_edit, new UserProfileUpdateDto()
            {
                Birthday = Birthday.Value,
                DisplayName = DisplayName,
                Signature = Signature,
                OpenEmailNotice = OpenEmailNotice,
                Sex = Sex,
                Email = Email,
            });

            if (resp == null)
            {
                Snackbar.Add("更新信息失败", Severity.Info);
                return;
            }

            await JSRuntime.InvokeVoidAsync("window.history.back");
        }
    }
}
