using LiJunSpace.API.Database;
using LiJunSpace.API.Dtos;
using LiJunSpace.Common.Dtos.Event;
using Microsoft.EntityFrameworkCore;

namespace LiJunSpace.API.Services
{
    public class EventsService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        public EventsService(JunRecordDbContext junRecordDbContext)
        {
            _junRecordDbContext = junRecordDbContext;
        }

        public async Task CreateEventAsync(EventCreationDto eventCreationDto, string publisher)
        {
            var @event = eventCreationDto.ToEventEntity();
            @event.Publisher = publisher;
            await _junRecordDbContext.Events.AddAsync(@event);
            await _junRecordDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDto>> GetMainPageEventsAsync()
        {
            var events =
                await _junRecordDbContext.Events.Include(x => x.Account).Where(x => x.ShowOnMainpage).ToListAsync();

            List<EventDto> eventDtos = new List<EventDto>();
            foreach (var item in events)
            {
                var dto = item.ToDto();
                if (string.IsNullOrEmpty(dto.PublisherAvatar))
                {
                    dto.PublisherAvatar = item.Account.Sex ? "default1.jpg" : "default0.jpg";
                }

                eventDtos.Add(dto);
            }
            return eventDtos;
        }

        public async Task<IEnumerable<EventDto>> GetEventsAsync()
        {
            var events =
                await _junRecordDbContext.Events.Include(x => x.Account).ToListAsync();

            List<EventDto> eventDtos = new List<EventDto>();
            foreach (var item in events)
            {
                var dto = item.ToDto();
                if (string.IsNullOrEmpty(dto.PublisherAvatar))
                {
                    dto.PublisherAvatar = item.Account.Sex ? "default1.jpg" : "default0.jpg";
                }

                eventDtos.Add(dto);
            }

            return eventDtos;
        }

        public async Task DeleteEventAsync(string eventId,string userId)
        {
            var @event =
                await _junRecordDbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId && x.Publisher == userId);

            if (@event != null) 
            {
                _junRecordDbContext.Events.Remove(@event);
                await _junRecordDbContext.SaveChangesAsync();
            }
        }
    }
}
