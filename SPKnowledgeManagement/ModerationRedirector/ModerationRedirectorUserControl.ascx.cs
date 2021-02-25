using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPKnowledgeManagement.ModerationRedirector
{
    public partial class ModerationRedirectorUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    var rootFolder = Request.QueryString["RootFolder"];
                    if (rootFolder != "")
                    {
                        //var url = rootFolder.Replace("%2F", "/");
                        var url = Server.UrlDecode(rootFolder);
                        SPWeb web = site.OpenWeb();
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        //var folder = docList.RootFolder.SubFolders[url];
                        var folder = web.GetFolder(url);
                        Response.Write("<alert>" + folder.Name + "</alert>");
                        if (!folder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ManagePermissions) && !web.UserIsSiteAdmin && !folder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ApproveItems))
                        {
                            int folderId = folder.Item.ID;
                            Response.Redirect("" + config.siteUrl + "SitePages/DocumentList.aspx?FolderID=" + folderId + "", false);
                        }
                    }
                    else
                    {
                        SPWeb web = site.OpenWeb();
                        if (!web.UserIsSiteAdmin)
                        {
                            Response.Redirect("" + config.siteUrl + "SitePages/Index.aspx", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string xx = ex.Message;
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    SPWeb web = site.OpenWeb();
                    if (!web.UserIsSiteAdmin)
                    {
                        Response.Redirect("" + config.siteUrl + "SitePages/Index.aspx", false);
                    }
                }
            }
        }
    }
}
