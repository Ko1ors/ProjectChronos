using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Services
{
    public class PackCreationHostedService : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        private readonly IServiceProvider _services;

        private const int MinPacksQuantity = 1;

        public PackCreationHostedService(IServiceProvider services)
        {
            _services = services;
        }


            public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(Execute, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(30));

            return Task.CompletedTask;
        }

        private void Execute(object? state)
        {
            using (var scope = _services.CreateScope())
            {
                var cardPackService =
                    scope.ServiceProvider
                        .GetRequiredService<ICardPackService>();

                if (cardPackService.GetPacksRemaining(CardPackType.WelcomePack) < MinPacksQuantity)
                {

                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
