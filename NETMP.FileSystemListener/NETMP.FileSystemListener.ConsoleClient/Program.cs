using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Threading;
using NETMP.FileSystemListener.Common;
using NETMP.FileSystemListener.Common.Configuration;

namespace NETMP.FileSystemListener.ConsoleClient
{
    class Program
    {
        private static FileSystemListenerConfig _config;
        private static ILogger _logger;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            _config = (FileSystemListenerConfig) ConfigurationManager.GetSection("listenerSection");
            _logger = new ConsoleLogger();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(_config.Culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_config.Culture);

            var fileSystemListener = new FileSystemListener(_config, _logger);
            fileSystemListener.StartWatching();

            Console.ReadKey();
        }
    }
}
