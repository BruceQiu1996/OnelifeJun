using LiJunSpace.Common.Dtos.Account;
using LiJunSpace.Helpers;
using LiJunSpace.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class ProfileComponentViewModel : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public UserProfileDto Profile { get; set; }
        [Inject]
        HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string Id { get; set; }
        public bool CanEdit { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            Profile = new UserProfileDto();
            var resp = await HttpRequest.GetAsync(string.Format(HttpRequestUrls.profile, Id));
            if (resp != null)
            {
                Profile = JsonSerializer.Deserialize<UserProfileDto>(await resp.Content.ReadAsStringAsync(),
                    HttpRequest._jsonSerializerOptions)!;

                Profile.Avatar = $"{Program.APIEndPoint}/api/avatars/{Profile.Avatar}";
                CanEdit = Id == await JSRuntime.InvokeAsync<string>("localStorageInterop.getItem", "userId");
                StateHasChanged();
            }
        }

        public bool Open { get; set; }
        public Anchor Anchor { get; set; }
        public void OpenAvatarDrawer(Anchor anchor)
        {
            Anchor = anchor;
            Open = true;
            IsDialogVisible = false;
        }

        public void EditProfile()
        {
            NavigationManager.NavigateTo("/profile/edit", false);
        }

        public bool IsDialogVisible { get; set; }
        public void OpenImageDialog()
        {
            Open = false;
            IsDialogVisible = true;
            StateHasChanged();
        }

        public async void OnSelectedAvatar(IBrowserFile browserFile)
        {
            const string format = "image/jpeg";
            var imageFile = await browserFile.RequestImageFileAsync(format, 1200, 675);
            await using var fileStream = imageFile.OpenReadStream();
            await using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var data = memoryStream.ToArray();

            var resp = await HttpRequest.PostImageAsync(HttpRequestUrls.upload_account_avatar, imageFile.Name, data);
            if (resp == null)
            {
                Snackbar.Add("上传错误", Severity.Error);
                return;
            }

            var filename = await resp.Content.ReadAsStringAsync();
            Profile.Avatar = $"{Program.APIEndPoint}/api/avatars/{filename}";
            StateHasChanged();
        }

        public async Task CheckInAsync()
        {
            var resp = await HttpRequest.PostAsync(HttpRequestUrls.account_checkin, null);
            if (resp != null)
            {
                Profile.TodayIsCheckIn = true;
                Snackbar.Add("今日已签到", Severity.Success);
                StateHasChanged();
            }
        }
    }
}
