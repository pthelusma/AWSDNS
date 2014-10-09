using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route53
{
    public class Change
    {
        public string Action { get; set; }
        public ResourceRecordSet ResourceRecordSet { get; set; }
    }
}
