using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route53
{
    public class ResourceRecordSet
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string TTL { get; set; }
        public List<ResourceRecord> ResourceRecords { get; set; }
    }
}
