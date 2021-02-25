using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Threading.Tasks;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using SPKnowledgeManagement.SitePages;

namespace SPKnowledgeManagement
{
    class ScoreSyncJob : SPJobDefinition
    {
        public ScoreSyncJob()
            : base()
        { }


        public ScoreSyncJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "SCOREPUSHJOB";
        }
        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }
        public override void Execute(Guid targetInstanceId)
        {
            System.Diagnostics.Trace.Assert(false);

            try
            {
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                SPContentDatabase contentDb = webApplication.ContentDatabases[targetInstanceId];

                if (config.siteName == "")
                {
                    SPWeb web = contentDb.Sites[0].OpenWeb();

                    SPList userList = web.Lists.TryGetList(config.userList);
                    SPList downList = web.Lists.TryGetList(config.downList);
                    SPQuery spDown = new SPQuery();
                    spDown.Query = "<Where><Eq><FieldRef Name='IsCalculate' /><Value Type='Boolean'>0</Value></Eq></Where>";
                    SPListItemCollection downItem = downList.GetItems(spDown);
                    for (int i = 0; i < downItem.Count; i++)
                    {
                        int docId = int.Parse(downItem[i]["DocumentId"].ToString());

                        string userAccount = downItem[i]["AuthorAccount"].ToString();
                        int downloadIntegral = Convert.ToInt32(downItem[i]["DownloadIntegral"]);
                        SPQuery spUser = new SPQuery();
                        spUser.Query = "<Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + userAccount + "</Value></Eq></Where>";
                        SPListItemCollection userItem = userList.GetItems(spUser);
                        if (userItem.Count > 0)
                        {
                            SPListItem item1 = userItem[0];
                            item1["Integral"] = Convert.ToInt32(userItem[0]["Integral"]) + downloadIntegral;
                            item1.SystemUpdate();
                            SPListItem item2 = downItem[i];
                            item2["IsCalculate"] = 1;
                            item2.SystemUpdate();
                        }
                    }
                }
                else
                {
                    SPWeb web = contentDb.Sites[config.siteName].OpenWeb();
                    SPList userList = web.Lists.TryGetList(config.userList);
                    SPList downList = web.Lists.TryGetList(config.downList);
                    SPQuery spDown = new SPQuery();
                    spDown.Query = "<Where><Eq><FieldRef Name='IsCalculate' /><Value Type='Boolean'>0</Value></Eq></Where>";
                    SPListItemCollection downItem = downList.GetItems(spDown);
                    for (int i = 0; i < downItem.Count; i++)
                    {
                        int docId = int.Parse(downItem[i]["DocumentId"].ToString());

                        string userAccount = downItem[i]["AuthorAccount"].ToString();
                        int downloadIntegral = Convert.ToInt32(downItem[i]["DownloadIntegral"]);
                        SPQuery spUser = new SPQuery();
                        spUser.Query = "<Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + userAccount + "</Value></Eq></Where>";
                        SPListItemCollection userItem = userList.GetItems(spUser);
                        if (userItem.Count > 0)
                        {
                            SPListItem item1 = userItem[0];
                            item1["Integral"] = Convert.ToInt32(userItem[0]["Integral"]) + downloadIntegral;
                            item1.SystemUpdate();
                            SPListItem item2 = downItem[i];
                            item2["IsCalculate"] = 1;
                            item2.SystemUpdate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}