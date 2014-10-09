using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;
using System.Xml.Serialization;

namespace Route53
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessKeyId = "AKIAIJFROTM7CQWW46UQ";
            string secretAccessKey = "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";

            string date = Utility.GetRoute53Date();
            string authValue = Utility.GetAWSR53_SHA1AuthorizationValue(accessKeyId, secretAccessKey, date);

            string externalIp = Utility.GetExternalIp();

            ListHostedZonesResponse listHostedZonesResponse = API.GetListHostedZones(date, authValue);

            var hostedZone = listHostedZonesResponse.HostedZones.Where(x => x.Name == "pierrethelusma.com.").SingleOrDefault();

            ListResourceRecordSetsResponse listResourceRecordSetsResponse = API.GetListResourceRecordSets(date, authValue, hostedZone.Id);

            var resourceRecordSet = listResourceRecordSetsResponse.ResourceRecordSets.Where(x => x.Name == "home.pierrethelusma.com." && x.Type == "A").SingleOrDefault();

            ChangeResourceRecordSetsResponse changeResourceRecordSetsResponse = new ChangeResourceRecordSetsResponse();

            if(resourceRecordSet == null)
            {
                changeResourceRecordSetsResponse = API.PostChangeResourceRecordSets(date, authValue, externalIp, hostedZone.Id, "home.pierrethelusma.com.", "A", "600", "CREATE");
            }
            else
            {
                if(!resourceRecordSet.ResourceRecords.Any(x => x.Value == externalIp))
                {
                    changeResourceRecordSetsResponse = API.PostChangeResourceRecordSets(date, authValue, externalIp, hostedZone.Id, "home.pierrethelusma.com.", "A", "600", "UPSERT");
                }
            }
        }


    }
}
