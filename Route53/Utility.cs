using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Route53
{
    public static class Utility
    {
        public static string GetRoute53Date()
        {
            string url = "https://route53.amazonaws.com/date";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response;
            response = request.GetResponse() as HttpWebResponse;
            return response.Headers["Date"];
        }

        public static string GetAWSR53_SHA1AuthorizationValue(string AWSAccessKeyId,
               string AWSSecretAccessKey, string AmzDate)
        {
            System.Security.Cryptography.HMACSHA1 MySigner =
              new System.Security.Cryptography.HMACSHA1(
              System.Text.Encoding.UTF8.GetBytes(AWSSecretAccessKey));

            string SignatureValue = Convert.ToBase64String(
              MySigner.ComputeHash(System.Text.Encoding.UTF8.GetBytes(AmzDate)));

            string AuthorizationValue = "AWS3-HTTPS AWSAccessKeyId=" +
              System.Uri.EscapeDataString(AWSAccessKeyId) +
              ",Algorithm=HmacSHA1,Signature=" + SignatureValue;

            return AuthorizationValue;
        }

        public static string GetExternalIp()
        {
            string Ip = String.Empty;

            string externalIP;
            externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                         .Matches(externalIP)[0].ToString();

            return externalIP;
        }

        public static T Deserialize<T>(Stream stream) where T : Response
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T response = (T)serializer.Deserialize(stream);

            return response;
        }

        public static string Serialize<T>(T t) where T : Request
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //  Add lib namespace with empty prefix
            ns.Add("", "https://route53.amazonaws.com/doc/2013-04-01/");

            //  Now serialize by passing the XmlSerializerNamespaces object
            //  as a parameter to the Serialize() method
            XmlSerializer serializer = new XmlSerializer(t.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(ms, Encoding.UTF8);
            //xmlTextWriter.Formatting = Formatting.Indented;
            serializer.Serialize(xmlTextWriter, t, ns);
            ms = (MemoryStream)xmlTextWriter.BaseStream;
            string xml = Encoding.UTF8.GetString(ms.ToArray());

            return xml;
        }
    }
}
