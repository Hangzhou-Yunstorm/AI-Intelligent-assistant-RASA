using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
//using System.Web.Services;
using System.Web.UI;
//using System.Web.UI.WebControls;

namespace SPKnowledgeManagement.SitePages
{
    public partial class Index : System.Web.UI.Page
    {
        public string json = string.Empty;
        public string folderName = string.Empty;
        public string newUpload = string.Empty;
        public string userName = string.Empty;
        public string integral = string.Empty;
        public string downNumber = string.Empty;
        public string uploadNumber = string.Empty;
        public string userRanking = string.Empty;
        public string docRanking = string.Empty;
        public string imgCarousel = string.Empty;
        public string resGuessYouLike = "[]";
        public string tipNum = string.Empty;
        public string publicFolderId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //test();
            DocShareTip();
            NewGuessYouLike();
            NewGetFolderFiles();
            NewGetDocumentRanking();
            NewCheckShowIndex();
        }

        private void test()
        {
            using (SPSite site = new SPSite(config.siteUrl))
            {
                var rootFolder = Request.QueryString["RootFolder"];
                //var url = rootFolder.Replace("%2F", "/");
                var url = Server.UrlDecode(rootFolder);
                Guid view = new Guid("AF4FD37B-1459-40B4-9426-BDCCCD332FEB");
                SPWeb web = site.OpenWeb();
                SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                var folder = docList.RootFolder.SubFolders[url];
                if (!folder.Item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.ManagePermissions) || !web.UserIsSiteAdmin)
                {
                    int folderId = folder.Item.ID;
                    Response.Redirect("" + config.siteUrl + "SitePages/DocumentList.aspx?FolderID=" + folderId + "");
                }
            }
        }


        /// <summary>
        /// 分享提示
        /// </summary>
        private void DocShareTip()
        {
            string loginAccount = "";
            string loginName = "";
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                loginAccount = web.CurrentUser.LoginName;
                loginName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList shareList = web.Lists.TryGetList(config.docShareList);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                                       "<And>" +
                                                            "<Eq>" +
                                                                "<FieldRef Name='ShareAccount' /><Value Type='Text'>" + loginAccount + "</Value>" +
                                                            "</Eq>" +
                                                            "<Eq>" +
                                                                "<FieldRef Name='IsRead' /><Value Type='Boolean'>0</Value>" +
                                                            "</Eq>" +
                                                        "</And>" +
                                                 "</Where>";
                    SPListItemCollection shareItem = shareList.GetItems(spQuery);
                    if (shareItem.Count > 0)
                    {
                        tipNum = shareItem.Count.ToString();
                    }
                    else
                    {
                        tipNum = "0";
                    }
                }
            });
        }
        /// <summary>
        /// 猜你喜欢
        /// </summary>
        public void NewGuessYouLike()
        {
            //获取登录用户
            string loginAccount = "";
            string loginName = "";
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    using (SPSite site = new SPSite(config.siteUrl))
                    {
                        SPWeb web = site.OpenWeb();
                        SPList downList = web.Lists[config.downList];
                        SPQuery spDown = new SPQuery();
                        spDown.Query = "   <Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                        SPListItemCollection downItems = downList.GetItems(spDown);
                        //没有下载记录 填入最新的7个文档
                        if (downItems.Count == 0)
                        {
                            SPList knowledgeList = web.Lists[config.knowledgeBase];
                            SPQuery spNewUploadKnowledge = new SPQuery();
                            spNewUploadKnowledge.ViewAttributes = "Scope=\"Recursive\"";
                            spNewUploadKnowledge.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                               "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";
                            spNewUploadKnowledge.RowLimit = 7;
                            SPListItemCollection knowNewUploadItems = knowledgeList.GetItems(spNewUploadKnowledge);
                            if (knowNewUploadItems.Count > 0)
                            {
                                resGuessYouLike = "[";
                                for (int i = 0; i < knowNewUploadItems.Count; i++)
                                {
                                    var arryExtension = knowNewUploadItems[i].Name.ToString().Split('.');
                                    string extension = arryExtension[arryExtension.Length - 1];
                                    resGuessYouLike += "{\"Title\":\"" + knowNewUploadItems[i]["Title"] + "\",\"Remark\":\"" + knowNewUploadItems[i]["Remark"] + "\",\"ID\":\"" + knowNewUploadItems[i].ID + "\",\"Extension\":\"" + extension + "\"},";
                                }
                                if (resGuessYouLike.Substring(resGuessYouLike.Length - 1) == ",")
                                {
                                    resGuessYouLike = resGuessYouLike.TrimEnd(',');
                                }
                                resGuessYouLike += "]";
                            }
                        }
                        //判断下载最多的类别
                        else
                        {
                            DataTable dtDownType = new DataTable();
                            dtDownType.Columns.AddRange(new DataColumn[] { new DataColumn("KnowledgeType", typeof(string)) });
                            for (int i = 0; i < downItems.Count; i++)
                            {
                                var arr = downItems[i]["KnowledgeType"].ToString().Split('/');
                                string knowledgeType = arr[1];
                                dtDownType.Rows.Add(new object[] { knowledgeType });
                            }
                            //获取文档库中出去Forms和活动的一级目录并转换成数组
                            SPList knowledgeList = web.Lists[config.knowledgeBase];
                            var folders = knowledgeList.RootFolder.SubFolders;
                            string name = "";
                            for (int i = 0; i < folders.Count; i++)
                            {
                                if (folders[i].Name != "Forms" && folders[i].Name != "活动")
                                {
                                    name += folders[i].Name + "|";
                                }
                            }
                            var arry = name.TrimEnd('|').Split('|');
                            //将每个目录下载次数放入list
                            List<GuessYouLike> guessList = new List<GuessYouLike>();
                            for (int j = 0; j < arry.Length; j++)
                            {
                                GuessYouLike guessYouLike = new GuessYouLike();
                                int count = 0;
                                for (int i = 0; i < dtDownType.Rows.Count; i++)
                                {
                                    if (dtDownType.Rows[i]["KnowledgeType"].ToString() == arry[j])
                                    {
                                        count = count + 1;
                                    }
                                }
                                guessYouLike.Name = arry[j];
                                guessYouLike.number = count;
                                guessList.Add(guessYouLike);
                            }
                            //根据list排序取得下载数最多的类别
                            var sorted = guessList.OrderByDescending(x => x.number);
                            string folderName = sorted.ToList()[0].Name.ToString();
                            SPQuery spQuery = new SPQuery();
                            spQuery.ViewAttributes = "Scope=\"Recursive\"";
                            spQuery.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                         "<OrderBy><FieldRef Name='Grade' Ascending='false' /></OrderBy>";
                            spQuery.RowLimit = 7;
                            for (int i = 0; i < folders.Count; i++)
                            {
                                if (folders[i].Name == folderName)
                                {
                                    spQuery.Folder = knowledgeList.RootFolder.SubFolders[folders[i].ServerRelativeUrl];
                                }
                            }
                            SPListItemCollection mostItems = knowledgeList.GetItems(spQuery);
                            if (mostItems.Count >= 7)
                            {
                                resGuessYouLike = "[";
                                for (int i = 0; i < 7; i++)
                                {
                                    var arryExtension = mostItems[i].Name.ToString().Split('.');
                                    string extension = arryExtension[arryExtension.Length - 1];
                                    resGuessYouLike += "{\"Title\":\"" + mostItems[i]["Title"] + "\",\"Remark\":\"" + mostItems[i]["Remark"] + "\",\"ID\":\"" + mostItems[i].ID + "\",\"Extension\":\"" + extension + "\"},";
                                }
                                if (resGuessYouLike.Substring(resGuessYouLike.Length - 1) == ",")
                                {
                                    resGuessYouLike = resGuessYouLike.TrimEnd(',');
                                }
                                resGuessYouLike += "]";
                            }
                            else
                            {
                                SPQuery spNewUploadKnowledge = new SPQuery();
                                spNewUploadKnowledge.ViewAttributes = "Scope=\"Recursive\"";
                                spNewUploadKnowledge.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                                   "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";
                                spNewUploadKnowledge.RowLimit = 15;
                                SPListItemCollection newUploadItems = knowledgeList.GetItems(spNewUploadKnowledge);
                                resGuessYouLike = "[";
                                for (int i = 0; i < mostItems.Count; i++)
                                {
                                    var arryExtension = mostItems[i].Name.ToString().Split('.');
                                    string extension = arryExtension[arryExtension.Length - 1];
                                    resGuessYouLike += "{\"Title\":\"" + mostItems[i]["Title"] + "\",\"Remark\":\"" + mostItems[i]["Remark"] + "\",\"ID\":\"" + mostItems[i].ID + "\",\"Extension\":\"" + extension + "\"},";
                                }
                                int number = 7 - mostItems.Count;
                                for (int i = 0; i < newUploadItems.Count; i++)
                                {
                                    bool isExist = false;
                                    for (int j = 0; j < mostItems.Count; j++)
                                    {
                                        if (mostItems[j].ID == newUploadItems[i].ID)
                                        {
                                            isExist = true;
                                        }
                                    }
                                    if (isExist == false && number > 0)
                                    {
                                        var arryExtension = newUploadItems[i].Name.ToString().Split('.');
                                        string extension = arryExtension[arryExtension.Length - 1];
                                        resGuessYouLike += "{\"Title\":\"" + newUploadItems[i]["Title"] + "\",\"Remark\":\"" + newUploadItems[i]["Remark"] + "\",\"ID\":\"" + newUploadItems[i].ID + "\",\"Extension\":\"" + extension + "\"},";
                                        number = number - 1;
                                    }
                                }
                                resGuessYouLike = resGuessYouLike.TrimEnd(',');

                                //if (resGuessYouLike.Substring(resGuessYouLike.Length - 1) == ",")
                                //{
                                //    resGuessYouLike = resGuessYouLike.TrimEnd(',');
                                //}
                                resGuessYouLike += "]";
                            }
                        }
                    }
                    resGuessYouLike = resGuessYouLike.Replace("\n", "<br>");
                }
                catch (Exception ex)
                {
                    string mess = ex.Message;
                }
            });
        }

        /// <summary>
        /// 猜你喜欢
        /// </summary>
        private void GuessYouLike1()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
             {
                 using (SPSite spSite = new SPSite(config.siteUrl))
                 {
                     SPWeb web = spSite.OpenWeb();
                     SPList userDown = web.Lists.TryGetList(config.downList);
                     string loginAccount = SPTools.GetLoginUserAccount(web);
                     SPQuery spQuery = new SPQuery();
                     spQuery.ViewAttributes = "Scope=\"Recursive\"";
                     SPListItemCollection downList = userDown.GetItems(spQuery);
                     DataTable dtDown = downList.GetDataTable();
                     DataTable newDt = new DataTable();
                     newDt.Columns.AddRange(new DataColumn[] { new DataColumn("KnowledgeType", typeof(string)) });
                     if (dtDown != null)
                     {
                         for (int i = 0; i < dtDown.Rows.Count; i++)
                         {
                             var arr = dtDown.Rows[i]["KnowledgeType"].ToString().TrimStart('/').Split('/');
                             string knowledgeType = arr[0];
                             //string knowledgeType = arr[1];
                             newDt.Rows.Add(new object[] { knowledgeType });
                         }
                     }
                     //获取一级目录
                     SPList list = web.Lists.TryGetList(config.knowledgeBase);
                     var folders = list.RootFolder.SubFolders;
                     string name = "";
                     for (int i = 0; i < folders.Count; i++)
                     {
                         if (folders[i].Name != "Forms" && folders[i].Name != "活动")
                         {
                             name += folders[i].Name + "|";
                         }
                     }
                     var arry = name.TrimEnd('|').Split('|');
                     List<GuessYouLike> guessList = new List<GuessYouLike>();
                     for (int j = 0; j < arry.Length; j++)
                     {
                         GuessYouLike guessYouLike = new GuessYouLike();
                         int count = 0;
                         for (int i = 0; i < newDt.Rows.Count; i++)
                         {
                             if (newDt.Rows[i]["KnowledgeType"].ToString() == arry[j])
                             {
                                 count = count + 1;
                             }
                         }
                         guessYouLike.Name = arry[j];
                         guessYouLike.number = count;
                         guessList.Add(guessYouLike);
                     }
                     var sorted = guessList.OrderByDescending(x => x.number);
                     string folderName = sorted.ToList()[0].Name.ToString();
                     SPQuery spQuery1 = new SPQuery();
                     spQuery1.ViewAttributes = "Scope=\"Recursive\"";
                     spQuery1.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                  "<OrderBy><FieldRef Name='Grade' Ascending='false' /></OrderBy>";
                     for (int i = 0; i < folders.Count; i++)
                     {
                         if (folders[i].Name == folderName)
                         {
                             spQuery1.Folder = list.RootFolder.SubFolders[folders[i].ServerRelativeUrl];
                         }
                     }
                     SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                     var guess = docList.GetItems(spQuery1);
                     resGuessYouLike = "[";
                     for (int i = 0; i < guess.Count; i++)
                     {
                         resGuessYouLike += "{\"Title\":\"" + guess[i]["Title"] + "\",\"Remark\":\"" + guess[i]["Remark"] + "\",\"ID\":\"" + guess[i].ID + "\"},";
                     }
                     if (resGuessYouLike.Substring(resGuessYouLike.Length - 1) == ",")
                     {
                         resGuessYouLike = resGuessYouLike.TrimEnd(',');
                     }
                     resGuessYouLike += "]";
                     //如果没有下载过任何东西
                     if (resGuessYouLike == "[]")
                     {
                         SPQuery spNewUpload = new SPQuery();
                         spNewUpload.ViewAttributes = "Scope=\"Recursive\"";
                         spNewUpload.RowLimit = Convert.ToUInt32(config.rowLimit2);
                         spNewUpload.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where>" +
                          "<OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy> ";
                         SPListItemCollection newUploadItems = docList.GetItems(spNewUpload);
                         if (newUploadItems.Count > 0)
                         {
                             resGuessYouLike = "[";
                             for (int i = 0; i < newUploadItems.Count; i++)
                             {
                                 resGuessYouLike += "{\"Title\":\"" + newUploadItems[i]["Title"] + "\",\"Remark\":\"" + newUploadItems[i]["Remark"] + "\",\"ID\":\"" + newUploadItems[i].ID + "\"},";
                             }
                             if (resGuessYouLike.Substring(resGuessYouLike.Length - 1) == ",")
                             {
                                 resGuessYouLike = resGuessYouLike.TrimEnd(',');
                             }
                             resGuessYouLike += "]";
                         }
                     }
                 }
             });
        }
        public class GuessYouLike
        {
            public string Name { get; set; }
            public int number { get; set; }
        }
        /// <summary>
        /// 服务端模型数据载入
        /// </summary>
        private void NewGetFolderFiles()
        {
            string loginAccount = "";
            string loginName = "";
            string currentLoginName = "";
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                loginAccount = SPTools.GetLoginUserAccount(web);
                currentLoginName = web.CurrentUser.LoginName;
                loginName = web.CurrentUser.Name;
                userName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList list = web.Lists.TryGetList(config.knowledgeBase);
                    SPQuery spFolder = new SPQuery();
                    spFolder.Query = "<OrderBy><FieldRef Name='Title' Ascending='False' /></OrderBy>";
                    SPListItemCollection folders = list.GetItems(spFolder);
                    //SPFolder folder = list.RootFolder;
                    //var folders = list.RootFolder.SubFolders;
                    json = "[";
                    folderName = "[";
                    //拼接json
                    #region
                    foreach (SPListItem f in folders)
                    {
                        SPQuery spQuery = new SPQuery();
                        int id;
                        if (f.Name == "Forms")
                        {
                            id = 0;
                        }
                        else
                        {
                            id = f.ID;
                        }
                        if (f.Name == "公共文档库")
                            publicFolderId = f.ID.ToString();
                        spQuery.Folder = list.RootFolder.SubFolders[f.Url];
                        spQuery.ViewAttributes = "Scope=\"Recursive\"";
                        spQuery.RowLimit = Convert.ToUInt32(config.rowLimit1);
                        spQuery.Query = @"<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where>
                        <OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                        SPListItemCollection docList = list.GetItems(spQuery);
                        if (f.Name != "Forms" && f.Name != "活动" && f.Name != "公共文档库")
                        {
                            folderName += "\"" + f.Name + "\",";
                            json += "{\"Name\":\"" + f.Name + "\",\"Id\":\"" + id + "\",\"Items\":[";
                            for (int j = 0; j < docList.Count; j++)
                            {
                                json += "{\"DisplayName\":\"" + docList[j].DisplayName + "\",\"Created\":\"" + Convert.ToDateTime(docList[j]["Created"]).ToString("MM-dd") + "\",\"Title\":\"" + docList[j]["Title"] + "\",\"Id\":\"" + docList[j].ID + "\"},";
                            }
                            if (json.Substring(json.Length - 1) == ",")
                            {
                                json = json.TrimEnd(',');
                            }
                            json += "]},";
                        }
                    }
                    #endregion
                    //判断结尾是否为,
                    if (json.Substring(json.Length - 1) == ",")
                    {
                        json = json.TrimEnd(',');
                    }
                    json += "]";
                    if (folderName.Substring(folderName.Length - 1) == ",")
                    {
                        folderName = folderName.Substring(0, folderName.Length - 1);
                    }
                    folderName += "]";
                    //最新上传获取
                    #region
                    SPQuery spQuery1 = new SPQuery();
                    spQuery1.ViewAttributes = "Scope=\"Recursive\"";
                    spQuery1.RowLimit = Convert.ToUInt32(config.rowLimit2);
                    spQuery1.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where>" +
                     "<OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy> ";
                    SPListItemCollection listItems1 = list.GetItems(spQuery1);
                    newUpload = "[";
                    for (int z = 0; z < listItems1.Count; z++)
                    {
                        var knowledgeType = "";
                        if (listItems1[z]["KnowledgeType"].ToString().IndexOf('/') != -1)
                        {
                            knowledgeType = listItems1[z]["KnowledgeType"].ToString().Split('/')[1];
                        }
                        newUpload += "{\"DisplayName\":\"" + listItems1[z].Name + "\",\"Created\":\"" + Convert.ToDateTime(listItems1[z]["Created"]).ToString("yyyy-MM-dd") + "\",\"Title\":\"" + listItems1[z]["Title"] + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + listItems1[z]["Clicks"].ToString() + "\",\"Comments\":\"" + listItems1[z]["Comments"] + "\",\"Id\":\"" + listItems1[z].ID + "\",\"Image\":\"" + listItems1[z]["Image"] + "\",\"Remark\":\"" + listItems1[z]["Remark"] + "\"},";
                    }
                    if (newUpload.Substring(newUpload.Length - 1) == ",")
                    {
                        newUpload = newUpload.Substring(0, newUpload.Length - 1);
                    }
                    newUpload += "]";
                    newUpload = newUpload.Replace("\n", "<br>");
                    #endregion
                    //获取个人中心
                    #region
                    SPList list1 = web.Lists.TryGetList(config.userList);
                    //指定过滤条件
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                    SPListItemCollection userIntegral = list1.GetItems(spQuery2);
                    if (userIntegral.Count > 0)
                    {
                        //userName = userIntegral[0]["UserName"].ToString();
                        integral = userIntegral[0]["Integral"].ToString();
                    }
                    else
                    {
                        //userName = web.CurrentUser.Name;
                        integral = "0";
                    }
                    //下载量
                    SPList listDown = web.Lists.TryGetList(config.downList);
                    SPListItemCollection userDownload = listDown.GetItems(spQuery2);
                    if (userDownload.Count > 0)
                    {
                        downNumber = userDownload.Count.ToString();
                    }
                    else
                    {
                        downNumber = "0";
                    }
                    //上传量
                    SPList listUpload = web.Lists.TryGetList(config.uploadList);
                    SPListItemCollection userUpload = listUpload.GetItems(spQuery2);
                    if (userUpload.Count > 0)
                    {
                        uploadNumber = userUpload.Count.ToString();
                    }
                    else
                    {
                        uploadNumber = "0";
                    }
                    #endregion
                    //用户排行
                    #region
                    SPQuery spQueryUserRanking = new SPQuery();
                    spQueryUserRanking.RowLimit = Convert.ToUInt32(config.rowLimit3);
                    spQueryUserRanking.Query = @"<OrderBy><FieldRef Name='Integral' Ascending='false' /></OrderBy>";
                    SPListItemCollection listItemUserRanking = list1.GetItems(spQueryUserRanking);
                    userRanking = "[";
                    foreach (SPListItem tmpItem in listItemUserRanking)
                    {
                        if (tmpItem["UserName"].ToString() != "系统帐户" && tmpItem["UserName"].ToString() != "系统账户")
                            userRanking += "{\"UserName\":\"" + tmpItem["UserName"] + "\",\"Integral\":\"" + tmpItem["Integral"] + "\"},";
                    }
                    if (userRanking.Substring(userRanking.Length - 1) == ",")
                    {
                        userRanking = userRanking.TrimEnd(',');
                    }
                    userRanking += "]";
                    #endregion
                }
            });
        }
        /// <summary>
        /// 服务端模型获取文档排行
        /// </summary>
        private void NewGetDocumentRanking()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList list = web.Lists.TryGetList(config.knowledgeBase);
                    SPQuery spQueryDocRanking = new SPQuery();
                    spQueryDocRanking.ViewAttributes = "Scope=\"RecursiveAll\"";
                    spQueryDocRanking.RowLimit = Convert.ToUInt32(config.rowLimit3);
                    spQueryDocRanking.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where>" +
                        "<OrderBy><FieldRef Name='Grade' Ascending='false' /></OrderBy> ";
                    SPListItemCollection listItemsDocRanking = list.GetItems(spQueryDocRanking);
                    docRanking = "[";
                    string extension = string.Empty;
                    for (int i = 0; i < listItemsDocRanking.Count; i++)
                    {
                        //extension = listItemsDocRanking[i].File.Name.ToString().Substring(listItemsDocRanking[i].File.Name.ToString().IndexOf('.'), listItemsDocRanking[i].File.Name.ToString().Length - listItemsDocRanking[i].File.Name.ToString().IndexOf('.')).TrimStart('.');
                        var arry = listItemsDocRanking[i].Name.ToString().Split('.');
                        extension = arry[arry.Length - 1];
                        docRanking += "{\"Title\":\"" + listItemsDocRanking[i]["Title"] + "\",\"Extension\":\"" + extension + "\",\"Author\":\"" + listItemsDocRanking[i]["Owner"] + "\",\"ID\":\"" + listItemsDocRanking[i].ID + "\"},";
                    }
                    if (docRanking.Substring(docRanking.Length - 1) == ",")
                    {
                        docRanking = docRanking.TrimEnd(',');
                    }
                    docRanking += "]";
                }
            });
        }
        /// <summary>
        /// 服务端模型图片轮播
        /// </summary>
        private void NewCheckShowIndex()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
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
                    spQueryActivity.Query = "<Where><And><And><Eq><FieldRef Name=\"ShowIndex\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                        "<Eq><FieldRef Name=\"Activity\" /><Value Type=\"Boolean\">1</Value></Eq></And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></And></Where>" +
                                                            "<OrderBy><FieldRef Name=\"Created\" Ascending='false'/></OrderBy>";
                    SPListItemCollection itemActivity = list.GetItems(spQueryActivity);
                    imgCarousel = "[";
                    for (int i = 0; i < itemActivity.Count; i++)
                    {
                        try
                        {
                            var imgUrl = "";
                            if (itemActivity[i]["Image"].ToString().IndexOf(',') != -1)
                            {
                                imgUrl = itemActivity[i]["Image"].ToString().Split(',')[0];
                            }
                            else
                            {
                                imgUrl = itemActivity[i]["Image"].ToString();
                            }
                            imgCarousel += "{\"Title\":\"" + itemActivity[i].File.Title + "\",\"Id\":\"" + itemActivity[i].ID + "\",\"ImageUrl\":\"" + imgUrl + "\"},";
                        }
                        catch
                        {

                            Response.Write("<script>alert(\"ID为" + itemActivity[i].ID + "的轮播文档（活动）缺少图片，请联系管理员解决。\")</script>");
                        }
                    }
                    SPQuery spQueryNotActivity = new SPQuery();
                    spQueryNotActivity.ViewAttributes = "Scope=\"RecursiveAll\"";
                    spQueryNotActivity.RowLimit = Convert.ToUInt32(config.rowLimit4);
                    spQueryNotActivity.Query = "<Where><And><And><Eq><FieldRef Name=\"ShowIndex\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                        "<Eq><FieldRef Name=\"Activity\" /><Value Type=\"Boolean\">0</Value></Eq></And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></And></Where>" +
                        "<OrderBy><FieldRef Name=\"Grade\" Ascending='false' /></OrderBy>";
                    SPListItemCollection itemNotActivity = list.GetItems(spQueryNotActivity);
                    for (int i = 0; i < itemNotActivity.Count; i++)
                    {
                        try
                        {
                            var imgUrl = "";
                            if (itemNotActivity[i]["Image"].ToString().IndexOf(',') != -1)
                            {
                                imgUrl = itemNotActivity[i]["Image"].ToString().Split(',')[0];
                            }
                            else
                            {
                                imgUrl = itemNotActivity[i]["Image"].ToString();
                            }
                            imgCarousel += "{\"Title\":\"" + itemNotActivity[i].File.Title + "\",\"Id\":\"" + itemNotActivity[i].ID + "\",\"ImageUrl\":\"" + imgUrl + "\"},";
                        }
                        catch
                        {
                            Response.Write("<script>alert(\"ID为" + itemNotActivity[i].ID + "的轮播文档（活动）缺少图片，请联系管理员解决。\")</script>");
                        }
                    }
                    if (imgCarousel.Substring(imgCarousel.Length - 1) == ",")
                    {
                        imgCarousel = imgCarousel.TrimEnd(',');
                    }
                    imgCarousel += "]";
                }
            });
        }
    }
}