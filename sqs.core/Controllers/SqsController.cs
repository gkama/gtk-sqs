using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;

using sqs.data;
using sqs.services;

namespace sqs.core.Controllers
{
    [Route("api")]
    [ApiController]
    public class SqsController : ControllerBase
    {
        public readonly ISqsRepository repo;

        public SqsController(ISqsRepository repo)
        {
            this.repo = repo;
        }

        [Route("send/order")]
        [HttpPost]
        public async Task<IActionResult> SendOrderAsync([FromBody]Order order)
        {
            return new JsonResult(await repo.SendOrderAsync(order));
        }

        [Route("receive/orders")]
        [HttpGet]
        public async Task<IActionResult> ReceiveMessagesAsync()
        {
            return new JsonResult(await repo.ReceiveOrderAsync());
        }
    }
}
