using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Route53
{
    [XmlRootAttribute(ElementName = "ChangeResourceRecordSetsResponse", Namespace = "https://route53.amazonaws.com/doc/2013-04-01/")]
    public class ChangeResourceRecordSetsResponse : Response
    {
        public ChangeInfo ChangeInfo { get; set; }
    }
}
