using System;
using System.Collections.Generic;

namespace sqs.data
{
    public class Order
    {
        public int id { get; set; }
        public string desc { get; set; }
        public string status { get; set; }

        public ICollection<IItem> items { get; set; } = new List<IItem>();
    }
}
