using Pairs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pairs.ConsoleClient
{
    class ConsoleClientProgram
    {
        static void Main(string[] args)
        {
            using (var channelFactory = new ChannelFactory<IPairsGameService>("PairsGameEndpoint"))
            {
                IPairsGameService client = channelFactory.CreateChannel();

                Console.WriteLine(client.GetColumnCount());
                Thread.Sleep(1000);
                Console.WriteLine(client.GetColumnCount());
                Thread.Sleep(1000);
                Console.WriteLine(client.GetColumnCount());
                Console.ReadLine();
            }

        }
    }
}
