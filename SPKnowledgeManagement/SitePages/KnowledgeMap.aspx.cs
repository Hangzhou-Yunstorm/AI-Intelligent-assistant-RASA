using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Net;


namespace SPKnowledgeManagement.SitePages
{
    public partial class KnowledgeMap : System.Web.UI.Page
    {
        public string CurrentUserName = SPContext.Current.Web.CurrentUser.LoginName.Split(new char[] { '\\' })[1];

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static string CategoryFolders = string.Empty;

        [WebMethod]
        public static string GetFoldersCategoryValue()
        {
            CategoryFolders = "";
            try
            {
                using (SPSite docSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = docSite.OpenWeb();
                    SPList myList = web.Lists.TryGetList(config.knowledgeBase);

                    SPFolder rootFolder = myList.RootFolder;
                    KnowledgeMapModel kmMapModel = new KnowledgeMapModel();

                    SPQuery spQuery = new SPQuery();
                    spQuery.Folder = rootFolder;
                    spQuery.ViewAttributes = "Scope=\"Recursive\"";
                    var resultCollection = myList.GetItems(spQuery);
                    kmMapModel.topic = config.knowledgeBase + "(" + resultCollection.Count.ToString() + ")";
                    kmMapModel.name = config.knowledgeBase + "(" + resultCollection.Count.ToString() + ")";
                    kmMapModel.value = resultCollection.Count.ToString();
                    kmMapModel.id = "0";
                    List<KnowledgeMapModel> kmModelList = GeneratorSubFolders(rootFolder, myList);
                    //kmModelList = kmModelList.OrderByDescending(kmModel => kmModel.name).ToList();
                    kmMapModel.children = kmModelList;
                    string JsonString = JsonHelper.ObjToJson(kmMapModel);
                    CategoryFolders = "[" + JsonString + "]";
                    //CategoryFolders += "]";
                }
                return CategoryFolders;
            }
            catch (Exception ex)
            {
                return "[]";
            }

        }

        private static List<KnowledgeMapModel> GeneratorSubFolders(SPFolder parentFolder, SPList spList)
        {
            try
            {
                List<KnowledgeMapModel> tmpKMMapModelList = new List<KnowledgeMapModel>();
                if (parentFolder.SubFolders.Count == 0)
                {
                    return new List<KnowledgeMapModel>();
                }
                else
                {
                    int tmpTag = 0;
                    for (int i = 0; i < parentFolder.SubFolders.Count; i++)
                    {
                        if (parentFolder.SubFolders[i].Name != "Forms")
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Folder = parentFolder.SubFolders[i];
                            spQuery.ViewAttributes = "Scope=\"Recursive\"";
                            var resultCollection = spList.GetItems(spQuery);
                            if (tmpTag % 2 == 0)
                                tmpKMMapModelList.Add(new KnowledgeMapModel() { name = parentFolder.SubFolders[i].Name + "(" + resultCollection.Count.ToString() + ")", topic = parentFolder.SubFolders[i].Name + "(" + resultCollection.Count.ToString() + ")", value = resultCollection.Count.ToString(), children = GeneratorSubFolders(parentFolder.SubFolders[i], spList), id = parentFolder.SubFolders[i].Item.ID.ToString(), direction = "left" });
                            else
                                tmpKMMapModelList.Add(new KnowledgeMapModel() { name = parentFolder.SubFolders[i].Name + "(" + resultCollection.Count.ToString() + ")", topic = parentFolder.SubFolders[i].Name + "(" + resultCollection.Count.ToString() + ")", value = resultCollection.Count.ToString(), children = GeneratorSubFolders(parentFolder.SubFolders[i], spList), id = parentFolder.SubFolders[i].Item.ID.ToString(), direction = "right" });
                            tmpTag++;
                            //CategoryFolders += ",{id:'" + childfolder.UniqueId.ToString() + "',pId:'" + parentFolder.UniqueId.ToString() + "',isParent:true,name:'" + childfolder.Name + "'}";
                            //GeneratorSubFolders(childfolder);
                        }
                    }
                }
                tmpKMMapModelList = tmpKMMapModelList.OrderByDescending(kmModel => kmModel.name).ToList();
                return tmpKMMapModelList;
            }
            catch (Exception ex)
            {
                return new List<KnowledgeMapModel>();
            }
        }
    }
}
