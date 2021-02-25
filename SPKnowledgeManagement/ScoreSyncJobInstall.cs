using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Threading.Tasks;
using SPKnowledgeManagement.SitePages;

namespace SPKnowledgeManagement
{
    class ScoreSyncJobInstall : SPFeatureReceiver
    {
        const string ScorePushJobName = "SCOREPUSHJOB";
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWebApplication spWebApp;
                if (properties.Feature.Parent is SPWeb)
                {
                    SPSite site = ((SPWeb)properties.Feature.Parent).Site;
                    spWebApp = site.WebApplication;
                    foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
                    {
                        if (job.Title == ScorePushJobName)
                        {
                            job.Delete();
                        }
                    }
                }
                else if (properties.Feature.Parent is SPSite)
                {
                    SPSite site = (SPSite)properties.Feature.Parent;
                    spWebApp = site.WebApplication;
                    foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
                    {
                        if (job.Title == ScorePushJobName)
                        {
                            job.Delete();
                        }
                    }
                }
                else
                {
                    SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;
                    spWebApp = webApp;
                    foreach (SPJobDefinition job in webApp.JobDefinitions)
                    {
                        if (job.Title == ScorePushJobName)
                        {
                            job.Delete();
                        }
                    }
                }
                ScoreSyncJob scoreSync = new ScoreSyncJob(ScorePushJobName, spWebApp);

                SPDailySchedule dailySchedule = new SPDailySchedule();
                dailySchedule.BeginHour = 1;
                dailySchedule.EndHour = 5;

                scoreSync.Schedule = dailySchedule;
                //SPMinuteSchedule minuteSchedule = new SPMinuteSchedule();
                //minuteSchedule.BeginSecond = 0;
                //minuteSchedule.EndSecond = 59;
                //minuteSchedule.Interval = 1;
                //scoreSync.Schedule = minuteSchedule;
                //SPDailySchedule dailySchedule = new SPDailySchedule();
                //dailySchedule.BeginHour = 8;
                //dailySchedule.EndSecond = 10;
                //knowledgePush.Schedule = dailySchedule;
                scoreSync.Update();
                //base.FeatureActivated(properties);
            }
            catch(Exception ex)
            {
            }
        }
     
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site;
            if (properties.Feature.Parent is SPWeb)
            {
                site = ((SPWeb)properties.Feature.Parent).Site;
            }
            else
            {
                site = (SPSite)properties.Feature.Parent;
            }
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Title == ScorePushJobName)
                {
                    job.Delete();
                }
            }
            //base.FeatureDeactivating(properties);
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            //base.FeatureInstalled(properties);
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            SPSite site;
            if (properties.Feature.Parent is SPWeb)
            {
                site = ((SPWeb)properties.Feature.Parent).Site;
            }
            else
            {
                site = (SPSite)properties.Feature.Parent;
            }
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Title == ScorePushJobName)
                {
                    job.Delete();
                }
            }
            //base.FeatureUninstalling(properties);
        }
    }
}
