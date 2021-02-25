using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace SPKnowledgeManagement.SitePages
{
    public partial class DocumentUpload : System.Web.UI.Page
    {
        public string CurrentUserName = "";
        public SPUser currentUser;
        //public bool IsPostback = ;
        public static string siteUrl = config.siteUrl;//ConfigurationManager.AppSettings["kbsite"].ToString();
        public static string kbTitle = config.knowledgeBase;
        public int MaxFileLength = config.maxFileSize;
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SPSite userSite = new SPSite(config.siteUrl))
            {
                SPWeb web = userSite.OpenWeb();
                currentUser = web.CurrentUser;
                CurrentUserName = SPTools.GetLoginUserAccount(web);
            }
        }
        public static string CategoryFolders = string.Empty;

        [WebMethod]
        public static string GetFoldersCategory()
        {
            CategoryFolders = "";
            using (SPSite docSite = new SPSite(siteUrl))
            {
                SPWeb web = docSite.OpenWeb();
                SPList myList = web.Lists.TryGetList(kbTitle);
                SPFolder rootFolder = myList.RootFolder;
                CategoryFolders += "[";
                CategoryFolders += "{'id':'" + rootFolder.UniqueId.ToString() + "','pId':0,'chkDisabled':true, 'open':true,'isParent':true,'name':'" + kbTitle + "'}";
                GeneratorSubFolders(rootFolder);
                CategoryFolders += "]";
            }
            return CategoryFolders;
        }

        private static void GeneratorSubFolders(SPFolder parentFolder)
        {
            if (parentFolder.SubFolders.Count == 0 || (parentFolder.Item != null && !parentFolder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.AddListItems)))
            {
                return;
            }
            else
            {
                foreach (SPFolder childfolder in parentFolder.SubFolders)
                {
                    if (childfolder.Name != "Forms" && (childfolder.Item != null && childfolder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.AddListItems)))
                    {
                        CategoryFolders += ",{'id':'" + childfolder.UniqueId.ToString() + "','pId':'" + parentFolder.UniqueId.ToString() + "','isParent':true,'name':'" + childfolder.Name + "'}";
                        GeneratorSubFolders(childfolder);
                    }
                }
            }
        }



        [WebMethod]
        public static string UploadKonwledge(string title, string category, string remark, object file, bool isActivity, string destiFolderId)
        {
            using (SPSite docSite = new SPSite(siteUrl))
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                //System.Text.StringBuilder strMsg = new System.Text.StringBuilder();
                //System.IO.Stream stm = ;
                SPWeb web = docSite.OpenWeb();
                SPList myList = web.Lists.TryGetList(kbTitle);
                //Control myControl1 = FindControl(file);
                SPListItem rootFolder = myList.GetItemByUniqueId(Guid.Parse(destiFolderId));
                //if()
            }
            return "";
        }
    }
}