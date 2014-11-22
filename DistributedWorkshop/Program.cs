using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using Metrics;

namespace DistributedWorkshop
{
    class Program
    {
		private static readonly Timer requestTimer = Metric.Timer("Request Time",Unit.Requests);

        static void Main(string[] args)
        {
			Metric.Config.WithHttpEndpoint ("http://localhost:12345" +
				"/");

			//RunServer();
			RunClient ("tcp://192.168.1.24:5556");
		}

		static void RunClient(string serverUri)
		{
			using (NetMQContext ctx = NetMQContext.Create())
			{
				using (var client = ctx.CreateRequestSocket()) 
				{
					client.Connect(serverUri);
					while (true) 
					{
						using (requestTimer.NewContext ())
						{
							client.Send("Hello", Encoding.UTF8,NetMQ.zmq.SendReceiveOptions.SendMore);
						}
					}
				}
			}
		}

		static void RunServer()
		{
			using (NetMQContext ctx = NetMQContext.Create()) 
			{
				using (var server = ctx.CreateResponseSocket()) 
				{
					server.Bind("tcp://0.0.0.0:5556");

					while (true) 
					{
						using (requestTimer.NewContext ())
						{
							var message = server.ReceiveString(Encoding.UTF8, NetMQ.zmq.SendReceiveOptions.SendMore);
							Console.WriteLine (message);
						}
					}
				}
			}
		}
    }
}
