using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route53.Constants
{
    public class Log
    {
        public const string SOURCE = "AWS Dynamic DNS";
        public const string LOG = "AWS Dynamic DNS Log";
    }

    public class LogMessaging
    {
        public const string SERVICE_STARTED = "AWS Dynamic DNS Service Started";
        public const string SERVICE_STOPPED = "AWS Dynamic DNS Service Stopped";
        public const string SERVICE_MONITORING = "AWS Dynamic DNS Service Monitoring";
        public const string SERVICE_CREATING_DNS_ENTRY = "AWS Dynamic DNS Service Creating DNS Entry";
        public const string SERVICE_UPDATING_DNS_ENTRY = "AWS Dynamic DNS Service Updating DNS Entry";
    }

}
