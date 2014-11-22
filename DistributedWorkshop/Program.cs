using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;

namespace DistributedWorkshop
{
    class Program
    {
        static void Main(string[] args)
        {
			RunServer();
			//RunClient ("tcp://192.168.43.45:5556");
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
						client.Send ("Hello");
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
					server.Bind("tcp://192.168.43.45:5556");

					while (true) 
					{
						var message = server.ReceiveString();
						Console.WriteLine(message);
					}
				}
			}
		}
    }
}
