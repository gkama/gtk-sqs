using System;

using Microsoft.Extensions.Logging;

using Amazon.SQS;

using sqs.data;

namespace sqs.services
{
    public class SqsRepository : ISqsRepository
    {
        public readonly IAmazonSQS sqs;
        public readonly ILogger log;

        public SqsRepository(IAmazonSQS sqs, ILogger<SqsRepository> log)
        {
            this.sqs = sqs;
            this.log = log;
        }
    }
}
