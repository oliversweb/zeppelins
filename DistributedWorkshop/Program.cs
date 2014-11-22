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
            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (var server = ctx.CreateResponseSocket())
                {
                    server.Bind("tcp://192.168.43.121:5556");

                    using (var client = ctx.CreateRequestSocket())
                    {
                        client.Connect("tcp://192.168.43.23:5556");
                        client.Send("Hello");

                        string m1 = server.ReceiveString();
                        Console.WriteLine("From Client: {0}", m1);
                        server.Send("Hi Back");

                        string m2 = client.ReceiveString();
                        Console.WriteLine("From Server: {0}", m2);
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
