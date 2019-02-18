using Microsoft.Azure.EventHubs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleReceive
{
    public class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "<endpoint_address>";
        private const string ConsumerGroupName = "$Default";

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Connecting to the Event Hub...");
            eventHubClient =
              EventHubClient.CreateFromConnectionString(EventHubConnectionString);
            var runtimeInformation = await eventHubClient.GetRuntimeInformationAsync();
            var partitionReceivers = runtimeInformation.PartitionIds.Select(partitionId =>
                eventHubClient.CreateReceiver(ConsumerGroupName,
                partitionId, EventPosition.FromEnd())).ToList();

            Console.WriteLine("Waiting for incoming events...");
             
            foreach (var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(
                  new AppPartitionReceiveHandler(partitionReceiver.PartitionId));
            }

            Console.WriteLine("Press any key to shutdown");
            Console.ReadLine();
            await eventHubClient.CloseAsync();
        }
    }
}
