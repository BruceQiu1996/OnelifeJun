using LiJunSpace.Common.Dtos.Account;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class ProfileComponentViewModel : ComponentBase
    {
        public UserProfileDto Profile { get; set; }
        [Inject]
        HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Profile = new UserProfileDto();
            await base.OnInitializedAsync();
            var resp = await HttpRequest.GetAsync(HttpRequestUrls.profile);
            if (resp != null)
            {
                Profile = JsonSerializer.Deserialize<UserProfileDto>(await resp.Content.ReadAsStringAsync(),
                    HttpRequest._jsonSerializerOptions)!;

                Profile.Avatar = $"{Program.APIEndPoint}/api/avatars/{Profile.Avatar}";
                StateHasChanged();
            }
        }

        public bool Open { get; set; }
        public Anchor Anchor { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public void OpenAvatarDrawer(Anchor anchor)
        {
            Anchor = anchor;
            Open = true;
            Width = "100%";
            Height = "100px";
        }

        public void EditProfile() 
        {
            NavigationManager.NavigateTo("/profile/edit", false);
        }
    }
}
