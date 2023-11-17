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
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void Execute(object? state)
        {
            // We need to await all the async calls, so we need to use Task.Run
            // In other case EF Core will dispose DB context and throw an exception
            Task.Run(async () =>
            {
                using (var scope = _services.CreateScope())
                {
                    var cardPackService =
                            scope.ServiceProvider
                                .GetRequiredService<ICardPackService>();

                    if (cardPackService.GetPacksRemaining(CardPackType.WelcomePack) < MinPacksQuantity)
                    {
                        await cardPackService.CreatePacksAsync(CardPackType.WelcomePack);
                    }
                }
            }
            );
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
