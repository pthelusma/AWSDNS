using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Route53
{
    [XmlRootAttribute(ElementName = "ChangeResourceRecordSetsRequest", Namespace = "https://route53.amazonaws.com/doc/2013-04-01/")]
    public class ChangeResourceRecordSetsRequest : Request
    {
        public ChangeBatch ChangeBatch { get; set; }
    }
}
