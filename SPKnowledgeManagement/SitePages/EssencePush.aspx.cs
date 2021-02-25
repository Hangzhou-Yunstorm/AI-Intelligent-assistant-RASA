using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace SPKnowledgeManagement.SitePages
{
    public partial class EssencePush : System.Web.UI.Page
    {
        public string essenceId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetEssenceId();
        }
        /// <summary>
        /// 获取需要推送的ID
        /// </summary>
        protected void GetEssenceId()
        {
            using (SPSite spSite = new SPSite(config.siteUrl))
            {
                SPWeb web = spSite.OpenWeb();
                SPList list = web.Lists.TryGetList(config.knowledgeBase);
                var folders = list.RootFolder.SubFolders;
                SPQuery spQueryActivity = new SPQuery();
                for (int i = 0; i < folders.Count; i++)
                {
                    if (folders[i].Name == "活动")
                    {
                        spQueryActivity.Folder = list.RootFolder.SubFolders[folders[i].ServerRelativeUrl];
                    }
                }
                spQueryActivity.ViewAttributes = "Scope=\"RecursiveAll\"";
                spQueryActivity.RowLimit = 3;
                spQueryActivity.Query = "<Where><Eq><FieldRef Name=\"ShowIndex\" /><Value Type=\"Boolean\">1</Value></Eq></Where>" +
                                                        "<OrderBy><FieldRef Name=\"Created\" /></OrderBy>";
                SPListItemCollection itemActivity = list.GetItems(spQueryActivity);
                for (int i = 0; i < itemActivity.Count; i++)
                {
                    essenceId += itemActivity[i].ID + ",";
                }
                SPQuery spQueryNotActivity = new SPQuery();
                spQueryNotActivity.ViewAttributes = "Scope=\"RecursiveAll\"";
                spQueryNotActivity.RowLimit = Convert.ToUInt32(config.rowLimit4);
                spQueryNotActivity.Query = "<Where><And><Eq><FieldRef Name=\"ShowIndex\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                    "<Eq><FieldRef Name=\"Activity\" /><Value Type=\"Boolean\">0</Value></Eq></And></Where>" +
                    "<OrderBy><FieldRef Name=\"Grade\" Ascending='false' /></OrderBy>";
                SPListItemCollection itemNotActivity = list.GetItems(spQueryNotActivity);
                for (int i = 0; i < itemNotActivity.Count; i++)
                {
                    essenceId += itemActivity[i].ID + ",";
                }
                if (essenceId.Substring(essenceId.Length - 1) == ",")
                {
                    essenceId = essenceId.TrimEnd(',');
                }
            }
        }

        [WebMethod]

        public static void OpenOutlook(string pushId)
        {
            var essenceID = pushId.Split(',');
            var body = "<a href=\"www.baidu.com\">1111111111111</a>";
            var url = "mailto:foo@bar.com?subject=Test&body=" + body + "";
            System.Diagnostics.Process.Start(url);
        }
    }
}