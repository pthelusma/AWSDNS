using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route53.Classes;
using System.Xml.Serialization;

namespace Route53
{
    [XmlRootAttribute(ElementName = "ListHostedZonesResponse", Namespace = "https://route53.amazonaws.com/doc/2013-04-01/")]
    public class ListHostedZonesResponse : Response
    {
        public List<HostedZone> HostedZones { get; set; }
    }
}
