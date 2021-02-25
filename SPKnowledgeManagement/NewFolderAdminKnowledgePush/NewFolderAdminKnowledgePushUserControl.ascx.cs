using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPKnowledgeManagement.NewFolderAdminKnowledgePush
{
    public partial class NewFolderAdminKnowledgePushUserControl : UserControl
    {
        public int IsButtonVisible = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            OpenOutlook();
        }

        public class PushItemModel
        {
            public string ItemId { get; set; }
            public string ItemTitle { get; set; }
        }

        protected void OpenOutlook()
        {

            try
            {
                System.Collections.Generic.List<PushItemModel> pushItems = new System.Collections.Generic.List<PushItemModel>();
                string essenceId = "";
                //获取推送的ID
                #region
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    var rootFolder = Request.QueryString["RootFolder"];
                    if (rootFolder != "" && rootFolder != null)
                    {
                        //var url = rootFolder.Replace("%2F", "/");
                        var url = Server.UrlDecode(rootFolder);
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        //var folder = docList.RootFolder.SubFolders[url];
                        var folder = web.GetFolder(url);
                        //Response.Write("<alert>" + folder.Name + "</alert>");
                        if (!folder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ManagePermissions) && !web.UserIsSiteAdmin && !folder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ApproveItems))
                        {
                            IsButtonVisible = 0;
                        }
                    }
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
                    spQueryActivity.Query = "<Where><And><Eq><FieldRef Name=\"ShowIndex\" /><Value Type=\"Boolean\">1</Value></Eq><Eq><FieldRef Name=\"Activity\" /><Value Type=\"Boolean\">1</Value></Eq></And></Where>" +
                                                            "<OrderBy><FieldRef Name=\"Created\"  Ascending=\"false\" /></OrderBy>";
                    SPListItemCollection itemActivity = list.GetItems(spQueryActivity);
                    for (int i = 0; i < itemActivity.Count; i++)
                    {
                        //essenceId += itemActivity[i].ID + ",";
                        pushItems.Add(new PushItemModel() { ItemId = itemActivity[i].ID.ToString(), ItemTitle = itemActivity[i].File.Name });
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
                        pushItems.Add(new PushItemModel() { ItemId = itemNotActivity[i].ID.ToString(), ItemTitle = itemNotActivity[i].File.Name });
                        //essenceId += itemNotActivity[i].ID + ",";
                    }
                    //if (essenceId.Substring(essenceId.Length - 1) == ",")
                    //{
                    //    essenceId = essenceId.TrimEnd(',');
                    //}
                }
                #endregion
                //var pushId = essenceId.Split(',');
                var formattedBody = "";
                if (pushItems.Count > 0)
                {
                    for (int i = 0; i < pushItems.Count; i++)
                    {
                        formattedBody += "文档名：" + pushItems[i].ItemTitle + ",链接：" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + pushItems[i].ItemId + "%0D";
                    }
                    hidMailUrl.Value = "mailto:" + config.AllMemberMail + "?subject=精华推送&body=" + formattedBody;
                    //System.Diagnostics.Process.Start(mailUrl);
                }
                else
                {
                    hidMailUrl.Value = "";
                    //System.Web.HttpContext.Current.Response.Write("<script>alert(\"请先添加需要推送的文档！\")</script>");
                }
            }
            catch (Exception ex)
            {
                hidMailUrl.Value = "";
            }
        }
    }
}
