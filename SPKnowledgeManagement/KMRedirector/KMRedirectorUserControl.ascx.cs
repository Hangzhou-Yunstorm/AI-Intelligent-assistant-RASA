using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPKnowledgeManagement.KMRedirector
{
    public partial class KMRedirectorUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                if (!web.UserIsSiteAdmin)
                {
                    Response.Redirect("" + config.siteUrl + "SitePages/Index.aspx");
                }
            }
        }
    }
}
