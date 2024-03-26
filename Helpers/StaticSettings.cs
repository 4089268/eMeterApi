using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeter.Helpers
{
    internal static class StaticSettings
    {

        internal static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}