using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        private readonly string _QueueUrl;

        public SqsRepository(IAmazonSQS sqs, IConfiguration configuration, ILogger<SqsRepository> log)
        {
            this.sqs = sqs;
            this.configuration = configuration;
            this.log = log;

            this._QueueUrl = configuration["AWS_QUEUE_URL"] ?? throw new ApplicationException("SqsQueueUrl environment variable is not set");
        }

        public async Task<Order> SendOrderAsync(Order order)
        {
            try
            {
                log.LogInformation($"SENDING order {order.id}");

                await sqs.SendMessageAsync(
                    new SendMessageRequest(_QueueUrl,
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

        public async Task<IEnumerable<Order>> ReceiveOrderAsync()
        {
            var response = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest()
            {
                QueueUrl = _QueueUrl
            });

            var orders = new List<Order>();

            response.Messages.ForEach(async x =>
            {
                //Process the message
                orders.Add(JsonConvert.DeserializeObject<Order>(x.Body));

                //Delete the message
                var deleteMsgReq = new DeleteMessageRequest
                {
                    QueueUrl = _QueueUrl,
                    ReceiptHandle = x.ReceiptHandle
                };
                await sqs.DeleteMessageAsync(deleteMsgReq);

                log.LogInformation($"received, processed and deleted message {x.MessageId}");
            });

            return orders;
        }
    }
}
