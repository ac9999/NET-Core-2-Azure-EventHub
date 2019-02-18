using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleReceive
{
    public class AppPartitionReceiveHandler : IPartitionReceiveHandler
    {
        public AppPartitionReceiveHandler(string partitionId)
        {
            PartitionId = partitionId;
        }

        public int MaxBatchSize { get; set; }

        public string PartitionId { get; }
        
        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(IEnumerable<EventData> eventDatas)
        {
            if (eventDatas != null)
            {
                foreach (var eventData in eventDatas)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    Console.WriteLine($"{dataAsJson} | PartitionId: {PartitionId}" +
                      $" | Offset: {eventData.SystemProperties.Offset}");
                }
            }
            return Task.CompletedTask;
        }

    }
}
