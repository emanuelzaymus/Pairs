using System;
using System.ServiceModel;

namespace Pairs.Server
{
    class ServerProgram
    {
        static void Main()
        {
            ServiceHost selfHost = new ServiceHost(typeof(PairsGameService));
            try
            {
                selfHost.Open();
                Console.WriteLine("The service is running. Press Enter to terminate.");
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine(ce.Message);
                selfHost.Abort();
            }
        }

    }
}
