using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Route53.Classes
{
    public class HostedZone
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ResourceRecordSetCount { get; set; }
    }
}
