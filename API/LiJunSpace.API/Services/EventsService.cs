using LiJunSpace.API.Channels;
using LiJunSpace.API.Database;
using LiJunSpace.API.Dtos;
using LiJunSpace.Common.Dtos.Event;
using Microsoft.EntityFrameworkCore;

namespace LiJunSpace.API.Services
{
    public class EventsService : IAppService
    {
        private readonly JunRecordDbContext _junRecordDbContext;
        private readonly SendEmailChannel _sendEmailChannel;
        public EventsService(JunRecordDbContext junRecordDbContext, SendEmailChannel sendEmailChannel)
        {
            _junRecordDbContext = junRecordDbContext;
            _sendEmailChannel = sendEmailChannel;
        }

        public async Task CreateEventAsync(EventCreationDto eventCreationDto, string publisher)
        {
            var @event = eventCreationDto.ToEventEntity();
            @event.Publisher = publisher;
            await _junRecordDbContext.Events.AddAsync(@event);
            await _junRecordDbContext.SaveChangesAsync();

            var objs = (await _junRecordDbContext.Accounts.Where(x => x.OpenEmailNotice
                && !string.IsNullOrEmpty(x.Email) && x.Id != publisher).ToListAsync()).Select(x => new SendEmailObject()
                {
                    Title = "有人创建了新的事件",
                    Content = @event.Title,
                    Target = x.Email
                });

            await _sendEmailChannel.WriteMessageAsync(objs);
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

        public async Task DeleteEventAsync(string eventId, string userId)
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
