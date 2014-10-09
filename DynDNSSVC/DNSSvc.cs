using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Route53;
using Route53.Constants;

namespace DynDNSSVC
{
    public partial class DNSSvc : ServiceBase
    {

        public DNSSvc()
        {
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();

            if (!System.Diagnostics.EventLog.SourceExists(Log.SOURCE))
            {
                System.Diagnostics.EventLog.CreateEventSource(Log.SOURCE, Log.LOG);
            }

            eventLog.Source = Log.SOURCE;
            eventLog.Log = Log.LOG;
        }

        protected override void OnStart(string[] args)
        {

            eventLog.WriteEntry(LogMessaging.SERVICE_STARTED);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry(LogMessaging.SERVICE_STOPPED);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {

            eventLog.WriteEntry(LogMessaging.SERVICE_MONITORING, EventLogEntryType.Information);

            string date = Utility.GetRoute53Date();
            string authValue = Utility.GetAWSR53_SHA1AuthorizationValue(Keys.ACCESS_KEY_ID, Keys.SECRET_KEY, date);

            string externalIp = Utility.GetExternalIp();

            ListHostedZonesResponse listHostedZonesResponse = API.GetListHostedZones(date, authValue);

            var hostedZone = listHostedZonesResponse.HostedZones.Where(x => x.Name == "pierrethelusma.com.").SingleOrDefault();

            ListResourceRecordSetsResponse listResourceRecordSetsResponse = API.GetListResourceRecordSets(date, authValue, hostedZone.Id);

            var resourceRecordSet = listResourceRecordSetsResponse.ResourceRecordSets.Where(x => x.Name == "home.pierrethelusma.com." && x.Type == "A").SingleOrDefault();

            ChangeResourceRecordSetsResponse changeResourceRecordSetsResponse = new ChangeResourceRecordSetsResponse();

            if (resourceRecordSet == null)
            {
                eventLog.WriteEntry(LogMessaging.SERVICE_CREATING_DNS_ENTRY, EventLogEntryType.Information);
                changeResourceRecordSetsResponse = API.PostChangeResourceRecordSets(date, authValue, externalIp, hostedZone.Id, "home.pierrethelusma.com.", "A", "600", "CREATE");
            }
            else
            {
                if (!resourceRecordSet.ResourceRecords.Any(x => x.Value == externalIp))
                {
                    eventLog.WriteEntry(LogMessaging.SERVICE_UPDATING_DNS_ENTRY, EventLogEntryType.Information);
                    changeResourceRecordSetsResponse = API.PostChangeResourceRecordSets(date, authValue, externalIp, hostedZone.Id, "home.pierrethelusma.com.", "A", "600", "UPSERT");
                }
            }
        }
    }
}
