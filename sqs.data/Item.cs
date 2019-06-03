using System;
using System.Collections.Generic;
using System.Text;

namespace sqs.data
{
    public class Item : IItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
    }
}
