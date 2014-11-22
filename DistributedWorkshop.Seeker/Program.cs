using Metrics;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWorkshop.Seeker
{
    class Program
    {
        private static readonly Timer requestTimer = Metric.Timer("Request Time", Unit.Requests);

        static void Main(string[] args)
        {
            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (var client = ctx.CreateDealerSocket())
                {
                    client.Connect("tcp://192.168.1.25:5556");
                    while (true)
                    {
                        using (requestTimer.NewContext())
                        {
                            client.Send("Hello");
                        }
                    }
                }
            }
        }
    }
}
