using Microsoft.Extensions.DependencyInjection;
using System;
using Unplugged.Core.Synchronization;

namespace JeffFerguson.Gepsio
{
    internal static class SecResourceControl
    {
        private static readonly IServiceProvider serviceProvider;

        static SecResourceControl()
        {
            var services = new ServiceCollection();
            services.AddUnpluggedSynchronization();
            serviceProvider = services.BuildServiceProvider();
        }

        public static void Wait(string url)
        {
            if (url is null || !url.Contains("sec.gov", StringComparison.InvariantCultureIgnoreCase))
                return;

            var machineRateGate = serviceProvider.GetRequiredService<IMachineRateGate>();
            var rateLimit = new NamedRateLimit("sec.gov", 1, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
            machineRateGate.WaitBlocking(rateLimit);
        }
    }
}
