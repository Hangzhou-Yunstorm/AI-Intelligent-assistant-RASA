using Microsoft.SharePoint;
using SPKnowledgeManagement.SitePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPKnowledgeManagement.MasterPages
{
    public partial class head : System.Web.UI.MasterPage
    {
        public string CurrentUserName = "";
        public string CurrentUserAccount = "";
        public string CurrentLoginAccount = "";
        public SPUser KBCurrentUser;
        public int IsCurrentUserSiteAdmin = 0;
        public string ApproveUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUser();
        }

        protected void CheckUser()
        {
            using (SPSite userSite = new SPSite(config.siteUrl))
            {
                SPWeb web = userSite.OpenWeb();
                CurrentUserName = web.CurrentUser.Name;
                CurrentLoginAccount = web.CurrentUser.LoginName;
                KBCurrentUser = web.CurrentUser;
                CurrentUserAccount = SPTools.GetLoginUserAccount(web);
                if (web.CurrentUser.IsSiteAdmin)
                    IsCurrentUserSiteAdmin = 1;

            }
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite spSite = new SPSite(config.siteUrl))
                        {
                            SPWeb web = spSite.OpenWeb();

                            SPList mappingList = web.Lists.TryGetList(config.approverList);

                            foreach (SPListItem approverItem in mappingList.Items)
                            {
                                SPFieldUserValueCollection userValues = approverItem["FolderAdmin"] as SPFieldUserValueCollection;
                                foreach (SPFieldUserValue userValue in userValues)
                                {
                                    if (null != userValue.User && SPTools.GetLoginUserAccount(userValue.User.LoginName).Equals(CurrentUserAccount))
                                    {
                                        string path = approverItem["FolderPath"].ToString();
                                        string folderPrefix = config.previewSiteName;
                                        if (folderPrefix == "")
                                            folderPrefix = "/";
                                        ApproveUrl = config.managementSiteUrl + "?RootFolder=" + Server.UrlEncode(folderPrefix + config.adminManagement + "/" + path);
                                        break;
                                    }
                                }
                            }

                            //SPQuery spApprover = new SPQuery();
                            //spApprover.Query = "<Where><Contains><FieldRef Name='ApproverAccount' /><Value Type='Text'>" + CurrentUserAccount + "</Value></Contains></Where>";
                            //SPListItemCollection approverItems = mappingList.GetItems(spApprover);
                            //if (approverItems.Count > 0)
                            //{
                            //    string path = approverItems[0]["FolderPath"].ToString();
                            //    string folderPrefix = config.previewSiteName;
                            //    if (folderPrefix == "")
                            //        folderPrefix = "/";
                            //    ApproveUrl = config.managementSiteUrl + "?RootFolder=" + Server.UrlEncode(folderPrefix + config.adminManagement + "/" + path);
                            //}

                            SPList userList = web.Lists.TryGetList(config.userList);


                            SPQuery spUser = new SPQuery();
                            spUser.Query = "   <Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + CurrentUserAccount + "</Value></Eq></Where>";
                            SPListItemCollection userItem = userList.GetItems(spUser);
                            if (userItem.Count == 0)
                            {
                                SPSecurity.RunWithElevatedPrivileges(delegate()
                                {
                                    web.AllowUnsafeUpdates = true;
                                    SPListItem newItem = userList.Items.Add();
                                    newItem["UserAccount"] = CurrentUserAccount;
                                    newItem["UserName"] = CurrentUserName;
                                    //newItem["Integral"] = Convert.ToInt32(config.defaultIntegral);
                                    newItem.Update();
                                    web.AllowUnsafeUpdates = false;
                                });
                            }
                        }
                    });
            }
            catch (Exception ex)
            {

            }
        }
    }
}