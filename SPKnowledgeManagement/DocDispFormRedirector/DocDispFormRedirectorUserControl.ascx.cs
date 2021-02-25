using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPKnowledgeManagement.DocDispFormRedirector
{
    public partial class DocDispFormRedirectorUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string itemID = Request.QueryString["ID"];
            if (itemID != null && !string.IsNullOrEmpty(itemID))
            {
                int dispItemId = int.Parse(itemID);
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    SPWeb web = site.OpenWeb();
                    SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                    var dispItem = docList.Items.GetItemById(dispItemId);
                    //Response.Write("<alert>" + folder.Name + "</alert>");
                    if (dispItem.ModerationInformation.Status == SPModerationStatusType.Approved)
                    {
                        Response.Redirect("/" + config.previewSiteName + "SitePages/DocumentDown.aspx?FileID=" + dispItemId.ToString(), false);
                    }
                    else
                    {
                        if (!dispItem.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ManagePermissions) && !web.UserIsSiteAdmin && !dispItem.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ApproveItems))
                        {
                            Response.Redirect("/" + config.previewSiteName + "SitePages/DocumentList.aspx?FolderID=" + dispItemId.ToString(), false);
                        }
                    }
                }
            }
        }
    }
}
