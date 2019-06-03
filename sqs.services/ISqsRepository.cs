using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Amazon.SQS.Model;

using sqs.data;

namespace sqs.services
{
    public interface ISqsRepository
    {
        Task<Order> SendOrderAsync(Order order);
        Task<IEnumerable<Order>> ReceiveOrderAsync();
    }
}
