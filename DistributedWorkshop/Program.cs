using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using Metrics;
using Timer = Metrics.Timer;

namespace DistributedWorkshop
{
    class Program
    {
		private static readonly Timer requestTimer = Metric.Timer("Request Time",Unit.Requests);

        static void Main(string[] args)
        {
			Metric.Config.WithHttpEndpoint("http://localhost:12345/");

			//RunServer();
			//RunClient ("tcp://192.168.1.24:5556");
			RunClient ("tcp://127.0.0.1:5556");
		}

		static void RunClient(string serverUri)
		{
			using (NetMQContext ctx = NetMQContext.Create())
			{
				using (var client = ctx.CreateDealerSocket()) 
				{
					client.Connect(serverUri);

					while (true) 
					{
						using (requestTimer.NewContext ())
						{
							client.Send("Hello", Encoding.UTF8,NetMQ.zmq.SendReceiveOptions.SendMore);
						}
					}
                    //client.Send ("Hello");
				}
			}
		}

		static void RunServer()
		{
            Console.WriteLine("Server waiting for connections ...");
            using (NetMQContext ctx = NetMQContext.Create())
			{
                using (var server = ctx.CreateDealerSocket()) 
				{
					server.Bind("tcp://0.0.0.0:5556");

					while (true) 
					{
						using (requestTimer.NewContext())
						{
							//var message = server.ReceiveString(Encoding.UTF8, NetMQ.zmq.SendReceiveOptions.SendMore);
							//Console.WriteLine (message);

							var message = server.ReceiveString ();
							server.Send("Our secret: Zepplins rule!");
						}
					}
				}
			}
		}
    }
}
