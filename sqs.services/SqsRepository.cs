using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

using sqs.data;

namespace sqs.services
{
    public class SqsRepository : ISqsRepository
    {
        public readonly IAmazonSQS sqs;
        public readonly IConfiguration configuration;
        public readonly ILogger log;

        public SqsRepository(IAmazonSQS sqs, IConfiguration configuration, ILogger<SqsRepository> log)
        {
            this.sqs = sqs;
            this.configuration = configuration;
            this.log = log;
        }

        public async Task<Order> SendOrderAsync(Order order)
        {
            try
            {
                log.LogInformation($"SENDING order {order.id}");

                await sqs.SendMessageAsync(new SendMessageRequest(configuration["SqsQueueUrl"],
                    JsonConvert.SerializeObject(order)));

                log.LogInformation($"COMPLETED order {order.id}");

                return order;
            }
            catch (Exception e)
            {
                log.LogError($"ERROR while sending order {order.id} error {e.Message}");

                throw;
            }
        }
    }
}
