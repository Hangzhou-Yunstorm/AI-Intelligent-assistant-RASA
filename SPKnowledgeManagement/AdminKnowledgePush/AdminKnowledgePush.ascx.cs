using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace SPKnowledgeManagement.AdminKnowledgePush
{
    [ToolboxItemAttribute(false)]
    public partial class AdminKnowledgePush : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public AdminKnowledgePush()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            OpenOutlook();
        }

        protected void OpenOutlook()
        {
            string essenceId = "";
            //获取推送的ID
            #region
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
                    essenceId += itemNotActivity[i].ID + ",";
                }
                if (essenceId.Substring(essenceId.Length - 1) == ",")
                {
                    essenceId = essenceId.TrimEnd(',');
                }
            }
            #endregion
            var pushId = essenceId.Split(',');
            var formattedBody = "";
            if (pushId.Length > 0)
            {
                for (int i = 0; i < pushId.Length;i++)
                {
                    formattedBody += config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + pushId[i] + "%0D";
                }
                hidMailUrl.Value = "mailto:foo@bar.com?subject=精华推送&body=" + formattedBody + "";
                //System.Diagnostics.Process.Start(mailUrl);
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert(\"请先添加需要推送的文档！\")</script>");
            }
        }
    }
}
