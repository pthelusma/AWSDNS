using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Route53
{
    public static class API
    {
        public static ChangeResourceRecordSetsResponse PostChangeResourceRecordSets(string date, string authValue, string externalIp, string hostedZoneId, string name, string type, string TTL, string action)
        {
            String url = "https://route53.amazonaws.com/2013-04-01" + hostedZoneId + "/rrset";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            WebHeaderCollection headers = (request as HttpWebRequest).Headers;

            headers.Add("x-amz-date", date);
            headers.Add("X-Amzn-Authorization", authValue);

            request.ContentType = "application/xml";

            string xmlRequest = Utility.Serialize<ChangeResourceRecordSetsRequest>(CreateChangeResourceRecordSetsRequest(name, type, TTL, externalIp, action));

            byte[] bytes = Encoding.UTF8.GetBytes(xmlRequest);

            request.ContentLength = bytes.Length;

            using (Stream putStream = request.GetRequestStream())
            {
                putStream.Write(bytes, 0, bytes.Length);
            }

            ChangeResourceRecordSetsResponse changeResourceRecordSetsResponse = new ChangeResourceRecordSetsResponse();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (Stream stream = response.GetResponseStream() as Stream)
            {
                changeResourceRecordSetsResponse = Utility.Deserialize<ChangeResourceRecordSetsResponse>(stream);
            }

            return changeResourceRecordSetsResponse;

        }

        public static ListResourceRecordSetsResponse GetListResourceRecordSets(string date, string authValue, string hostedZoneId)
        {
            String url = "https://route53.amazonaws.com/2013-04-01" + hostedZoneId + "/rrset?";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            WebHeaderCollection headers = (request as HttpWebRequest).Headers;

            ListResourceRecordSetsResponse listResourceRecordSetsResponse = new ListResourceRecordSetsResponse();

            headers.Add("x-amz-date", date);
            headers.Add("X-Amzn-Authorization", authValue);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (Stream stream = response.GetResponseStream() as Stream)
            {
                listResourceRecordSetsResponse = Utility.Deserialize<ListResourceRecordSetsResponse>(stream);
            }

            return listResourceRecordSetsResponse;
        }

        public static ListHostedZonesResponse GetListHostedZones(string date, string authValue)
        {
            String url = "https://route53.amazonaws.com/2013-04-01/hostedzone";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            WebHeaderCollection headers = (request as HttpWebRequest).Headers;

            ListHostedZonesResponse listHostedZonesResponse = new ListHostedZonesResponse();

            // the canonical String is the date String 
            //String httpDate = GetRoute53Date();

            headers.Add("x-amz-date", date);

            // Both the following methods work! 
            //String authenticationSig = GetAWSR53_SHA1AuthorizationValue(awsId, secretId, httpDate);
            headers.Add("X-Amzn-Authorization", authValue);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (Stream stream = response.GetResponseStream() as Stream)
            {
                listHostedZonesResponse = Utility.Deserialize<ListHostedZonesResponse>(stream);
            }

            return listHostedZonesResponse;

        }

        public static ChangeResourceRecordSetsRequest CreateChangeResourceRecordSetsRequest(string Name, string Type, string TTL, string Value, string Action)
        {
            ResourceRecordSet resourceRecordSet = new ResourceRecordSet();
            resourceRecordSet.Name = Name;
            resourceRecordSet.Type = Type;
            resourceRecordSet.TTL = TTL;

            ResourceRecord resourceRecord = new ResourceRecord();
            resourceRecord.Value = Value;

            resourceRecordSet.ResourceRecords = new List<ResourceRecord>();
            resourceRecordSet.ResourceRecords.Add(resourceRecord);

            Change change = new Change();
            change.Action = Action;
            change.ResourceRecordSet = resourceRecordSet;

            ChangeBatch changeBatch = new ChangeBatch();
            changeBatch.Changes = new List<Change>();
            changeBatch.Changes.Add(change);

            ChangeResourceRecordSetsRequest changeResourceRecordSetsRequest = new ChangeResourceRecordSetsRequest();
            changeResourceRecordSetsRequest.ChangeBatch = changeBatch;

            return changeResourceRecordSetsRequest;
        }
    }
}
