using LiJunSpace.Common.Dtos.Event;
using LiJunSpace.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace LiJunSpace.ViewModels
{
    public class EventsComponentViewModel : ComponentBase
    {
        [Inject]
        public HttpRequest HttpRequest { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        public List<EventDto>? Events { get; set; } = new List<EventDto>();
        private EventDto currentEvent;
        public bool  Open { get; set; }
        public void NewEvent()
        {
            Navigation.NavigateTo("/newEvent", forceLoad: false, replace: false);
        }

        protected async override Task OnInitializedAsync()
        {
            Events.Clear();
            var resp = await HttpRequest.GetAsync(HttpRequestUrls.event_url);
            if (resp != null)
            {
                Events = JsonSerializer
                   .Deserialize<List<EventDto>>(await resp.Content.ReadAsStringAsync(), HttpRequest._jsonSerializerOptions);

                StateHasChanged();
            }
        }

        public void OpenUserDetail(string id)
        {
            Navigation.NavigateTo($"/profile/{id}", forceLoad: false, replace: false);
        }

        public async void OpenAvatar(EventDto eventDto) 
        {
            currentEvent = null;
            var userId = await JSRuntime.InvokeAsync<string>("localStorageInterop.getItem", "userId");
            if (eventDto.Publisher != userId) 
            {
                return;
            }
            Open = true;
            currentEvent = eventDto;
            StateHasChanged();
        }

        public async Task DeleteEventAction() 
        {
            Open = false;
            if (currentEvent == null)
                return;

            var resp = await 
                HttpRequest.GetAsync(string.Format(HttpRequestUrls.event_url_delete, currentEvent.Id));
            if (resp != null) 
            {
                await OnInitializedAsync();
            }
        }
    }
}
