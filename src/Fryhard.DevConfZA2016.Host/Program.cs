using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Fryhard.DevConfZA2016.Host
{
    class Program
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            try
            {
                _Log.Debug("Starting");
                const string name = ".Fryhard.DevConfZA2016.Host";
                const string description = "Fryhard.DevConfZA2016.Host";
                var host = HostFactory.New(configuration =>
                {
                    configuration.Service<Host>(s =>
                    {
                        s.ConstructUsing(externalHost => new Host());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    configuration.SetDisplayName(name);
                    configuration.SetServiceName(name);
                    configuration.SetDescription(description);
                    configuration.RunAsLocalService();
                });
                host.Run();
            }
            catch (Exception ex)
            {
                _Log.Fatal(string.Format("Fryhard.DevConfZA2016.Host.Program fatal exception: {0}", ex.Message), ex);
            }
        }
    }
}
