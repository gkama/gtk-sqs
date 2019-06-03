using System;
using System.Collections.Generic;
using System.Text;

namespace sqs.data
{
    public interface IItem
    {
        int id { get; set; }
        string desc { get; set; }
    }
}
