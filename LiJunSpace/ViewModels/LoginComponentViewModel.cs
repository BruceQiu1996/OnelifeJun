using LiJunSpace.Common.Dtos.Account;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class LoginComponentViewModel : ComponentBase
    {
        public RegisterAccountForm model;
        public string errorMessage;
        public class RegisterAccountForm
        {
            [Required(ErrorMessage = "用户名必填")]
            [StringLength(16, ErrorMessage = "用户名长度小于16")]
            public string Username { get; set; }

            [Required(ErrorMessage = "密码必填")]
            [StringLength(16, ErrorMessage = "密码长度小于16大于6", MinimumLength = 6)]
            public string Password { get; set; }
        }

        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            model = new RegisterAccountForm();
            HttpRequest.ExcuteWhileBadRequest += (error) =>
            {
                errorMessage = error;
            };
            StateHasChanged();
        }

        public async Task OnValidSubmit(EditContext context)
        {
            errorMessage = null;
            var resp = await HttpRequest.PostAsync(HttpRequestUrls.Login, new UserLoginDto()
            {
                UserName = model.Username,
                Password = model.Password,
            });

            if (resp != null)
            {
                var userInfo = JsonSerializer
                    .Deserialize<UserLoginResponseDto>(await resp.Content.ReadAsStringAsync(), HttpRequest._jsonSerializerOptions);
                await JSRuntime.InvokeVoidAsync("localStorageInterop.setItem", "token", userInfo.AccessToken);
                await JSRuntime.InvokeVoidAsync("localStorageInterop.setItem", "userId", userInfo.Id);
                Navigation.NavigateTo("/", forceLoad: false, replace: true);
            }
        }
    }
}
