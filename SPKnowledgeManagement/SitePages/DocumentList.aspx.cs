using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Data;
//using System.Web.Services;
using System.Web.UI;
//using System.Web.UI.WebControls;

namespace SPKnowledgeManagement.SitePages
{
    public partial class DocumentList : System.Web.UI.Page
    {
        public string documentList = string.Empty;
        public string requestID = string.Empty;
        public string listName = string.Empty;
        public string navTitle = string.Empty;
        public string parentName = string.Empty;
        public string parentId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            requestID = Request.QueryString["FolderID"];
            int listId = -1;
            if (SPTools.IsNumeric(requestID))
                listId = int.Parse(requestID);
            if (!SPTools.IsNumeric(requestID) && listId<0 && requestID != "youLike" && requestID != "newUpload" && requestID != "docRanking")
            {
                Response.Write("<script language=javascript>if(confirm('参数异常，将返回前一页面')){history.go(-1);}</script>");
                return;
            }
            GetDocList();
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void GetDocList()
        {

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
                if (requestID == "youLike" || requestID == "newUpload" || requestID == "docRanking")
                {
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {
                        SPWeb web = spSite.OpenWeb();
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        switch (requestID)
                        {
                            #region
                            case "newUpload":
                                SPQuery spQueryNewUpload = new SPQuery();
                                spQueryNewUpload.ViewAttributes = "Scope=\"Recursive\"";
                                spQueryNewUpload.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                    "<OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                                spQueryNewUpload.RowLimit = 20;
                                SPListItemCollection newUploadList = docList.GetItems(spQueryNewUpload);
                                if (newUploadList.Count > 0)
                                {
                                    documentList = "[";
                                    for (int i = 0; i < newUploadList.Count; i++)
                                    {
                                        var arryNew = newUploadList[i].Name.ToString().Split('.');
                                        string extension = arryNew[arryNew.Length - 1];
                                        string uploadTime = Convert.ToDateTime(newUploadList[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                        string knowledgeType = "";
                                        if (newUploadList[i]["KnowledgeType"].ToString().IndexOf('/') != -1)
                                        {
                                            knowledgeType = newUploadList[i]["KnowledgeType"].ToString().Split('/')[1];
                                        }
                                        documentList += "{\"Id\":\"" + newUploadList[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + newUploadList[i].File.Title + "\",\"Author\":\"" + newUploadList[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + newUploadList[i]["Clicks"] + "\",\"Downloads\":\"" + newUploadList[i]["Downloads"] + "\",\"Comments\":\"" + newUploadList[i]["Comments"] + "\",\"Grade\":\"" + newUploadList[i]["Grade"] + "\",\"DownloadIntegral\":\"" + newUploadList[i]["DownloadIntegral"] + "\"},";
                                    }
                                    if (documentList.Substring(documentList.Length - 1) == ",")
                                    {
                                        documentList = documentList.TrimEnd(',');
                                    }
                                    documentList += "]";
                                }
                                break;
                            #endregion
                            #region
                            case "docRanking":
                                SPQuery spQueryDocRanking = new SPQuery();
                                spQueryDocRanking.ViewAttributes = "Scope=\"Recursive\"";
                                spQueryDocRanking.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                    "<OrderBy><FieldRef Name='Grade' Ascending='false' /></OrderBy>";
                                spQueryDocRanking.RowLimit = 30;
                                SPListItemCollection docRankingList = docList.GetItems(spQueryDocRanking);
                                if (docRankingList.Count > 0)
                                {
                                    documentList = "[";
                                    for (int i = 0; i < docRankingList.Count; i++)
                                    {
                                        var arryNew = docRankingList[i].Name.ToString().Split('.');
                                        string extension = arryNew[arryNew.Length - 1];
                                        string uploadTime = Convert.ToDateTime(docRankingList[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                        string knowledgeType = "";
                                        if (docRankingList[i]["KnowledgeType"].ToString().IndexOf('/') != -1)
                                        {
                                            knowledgeType = docRankingList[i]["KnowledgeType"].ToString().Split('/')[1];
                                        }
                                        documentList += "{\"Id\":\"" + docRankingList[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + docRankingList[i].File.Title + "\",\"Author\":\"" + docRankingList[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + docRankingList[i]["Clicks"] + "\",\"Downloads\":\"" + docRankingList[i]["Downloads"] + "\",\"Comments\":\"" + docRankingList[i]["Comments"] + "\",\"Grade\":\"" + docRankingList[i]["Grade"] + "\",\"DownloadIntegral\":\"" + docRankingList[i]["DownloadIntegral"] + "\"},";
                                    }
                                    if (documentList.Substring(documentList.Length - 1) == ",")
                                    {
                                        documentList = documentList.TrimEnd(',');
                                    }
                                    documentList += "]";
                                }
                                break;
                            #endregion
                            #region
                            case "youLike":
                                SPList downList = web.Lists[config.downList];
                                SPQuery spDown = new SPQuery();
                                spDown.Query = "   <Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                                SPListItemCollection downItems = downList.GetItems(spDown);
                                //没有下载记录 填入最新的10个文档
                                if (downItems.Count == 0)
                                {
                                    SPList knowledgeList = web.Lists[config.knowledgeBase];
                                    SPQuery spNewUploadKnowledge = new SPQuery();
                                    spNewUploadKnowledge.ViewAttributes = "Scope=\"Recursive\"";
                                    spNewUploadKnowledge.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                                       "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";
                                    spNewUploadKnowledge.RowLimit = 10;
                                    SPListItemCollection knowNewUploadItems = knowledgeList.GetItems(spNewUploadKnowledge);
                                    if (knowNewUploadItems.Count > 0)
                                    {
                                        documentList = "[";
                                        for (int i = 0; i < knowNewUploadItems.Count; i++)
                                        {
                                            var arryNew = knowNewUploadItems[i].Name.ToString().Split('.');
                                            string extension = arryNew[arryNew.Length - 1];
                                            string uploadTime = Convert.ToDateTime(knowNewUploadItems[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            documentList += "{\"Id\":\"" + knowNewUploadItems[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + knowNewUploadItems[i].File.Title + "\",\"Author\":\"" + knowNewUploadItems[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowNewUploadItems[i]["KnowledgeType"] + "\",\"Clicks\":\"" + knowNewUploadItems[i]["Clicks"] + "\",\"Downloads\":\"" + knowNewUploadItems[i]["Downloads"] + "\",\"Comments\":\"" + knowNewUploadItems[i]["Comments"] + "\",\"Grade\":\"" + knowNewUploadItems[i]["Grade"] + "\",\"DownloadIntegral\":\"" + knowNewUploadItems[i]["DownloadIntegral"] + "\"},";
                                        }
                                        if (documentList.Substring(documentList.Length - 1) == ",")
                                        {
                                            documentList = documentList.TrimEnd(',');
                                        }
                                        documentList += "]";
                                    }
                                }
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
                                    spQuery.RowLimit = 10;
                                    for (int i = 0; i < folders.Count; i++)
                                    {
                                        if (folders[i].Name == folderName)
                                        {
                                            spQuery.Folder = knowledgeList.RootFolder.SubFolders[folders[i].ServerRelativeUrl];
                                        }
                                    }
                                    SPListItemCollection mostItems = knowledgeList.GetItems(spQuery);
                                    if (mostItems.Count == 10)
                                    {
                                        documentList = "[";
                                        for (int i = 0; i < mostItems.Count; i++)
                                        {
                                            var arryNew = mostItems[i].Name.ToString().Split('.');
                                            string extension = arryNew[arryNew.Length - 1];
                                            string uploadTime = Convert.ToDateTime(mostItems[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            documentList += "{\"Id\":\"" + mostItems[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + mostItems[i].File.Title + "\",\"Author\":\"" + mostItems[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + mostItems[i]["KnowledgeType"] + "\",\"Clicks\":\"" + mostItems[i]["Clicks"] + "\",\"Downloads\":\"" + mostItems[i]["Downloads"] + "\",\"Comments\":\"" + mostItems[i]["Comments"] + "\",\"Grade\":\"" + mostItems[i]["Grade"] + "\",\"DownloadIntegral\":\"" + mostItems[i]["DownloadIntegral"] + "\"},";
                                        }
                                        if (documentList.Substring(documentList.Length - 1) == ",")
                                        {
                                            documentList = documentList.TrimEnd(',');
                                        }
                                        documentList += "]";
                                    }
                                    else
                                    {
                                        SPQuery spNewUploadKnowledge = new SPQuery();
                                        spNewUploadKnowledge.ViewAttributes = "Scope=\"Recursive\"";
                                        spNewUploadKnowledge.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                                                           "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";
                                        spNewUploadKnowledge.RowLimit = 20 - Convert.ToUInt32(mostItems.Count);
                                        SPListItemCollection newUploadItems = knowledgeList.GetItems(spNewUploadKnowledge);
                                        documentList = "[";
                                        for (int i = 0; i < mostItems.Count; i++)
                                        {
                                            var arryNew = mostItems[i].Name.ToString().Split('.');
                                            string extension = arryNew[arryNew.Length - 1];
                                            string uploadTime = Convert.ToDateTime(mostItems[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            documentList += "{\"Id\":\"" + mostItems[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + mostItems[i].File.Title + "\",\"Author\":\"" + mostItems[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + mostItems[i]["KnowledgeType"] + "\",\"Clicks\":\"" + mostItems[i]["Clicks"] + "\",\"Downloads\":\"" + mostItems[i]["Downloads"] + "\",\"Comments\":\"" + mostItems[i]["Comments"] + "\",\"Grade\":\"" + mostItems[i]["Grade"] + "\",\"DownloadIntegral\":\"" + mostItems[i]["DownloadIntegral"] + "\"},";
                                        }
                                        int number = 10 - mostItems.Count;
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
                                                var arryNew = newUploadItems[i].Name.ToString().Split('.');
                                                string extension = arryNew[arryNew.Length - 1];
                                                string uploadTime = Convert.ToDateTime(newUploadItems[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                documentList += "{\"Id\":\"" + newUploadItems[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + newUploadItems[i].File.Title + "\",\"Author\":\"" + newUploadItems[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + newUploadItems[i]["KnowledgeType"] + "\",\"Clicks\":\"" + newUploadItems[i]["Clicks"] + "\",\"Downloads\":\"" + newUploadItems[i]["Downloads"] + "\",\"Comments\":\"" + newUploadItems[i]["Comments"] + "\",\"Grade\":\"" + newUploadItems[i]["Grade"] + "\",\"DownloadIntegral\":\"" + newUploadItems[i]["DownloadIntegral"] + "\"},";
                                                number = number - 1;
                                            }
                                        }
                                        if (documentList.Substring(documentList.Length - 1) == ",")
                                        {
                                            documentList = documentList.TrimEnd(',');
                                        }
                                        documentList += "]";
                                    }
                                }
                                //SPList userDown = web.Lists.TryGetList(config.downList);
                                //SPQuery spQuery = new SPQuery();
                                //spQuery.ViewAttributes = "Scope=\"Recursive\"";
                                //SPListItemCollection downList = userDown.GetItems(spQuery);
                                //DataTable dtDown = downList.GetDataTable();
                                //DataTable newDt = new DataTable();
                                //if (dtDown != null)
                                //{
                                //    newDt.Columns.AddRange(new DataColumn[] { new DataColumn("KnowledgeType", typeof(string)) });
                                //    for (int i = 0; i < dtDown.Rows.Count; i++)
                                //    {
                                //        string knowledgeType = "";

                                //        if (dtDown.Rows[i]["KnowledgeType"].ToString().IndexOf('/') != -1)
                                //        {
                                //            knowledgeType = dtDown.Rows[i]["KnowledgeType"].ToString().Split('/')[1];
                                //        }
                                //        //string knowledgeType = arr[1];
                                //        newDt.Rows.Add(new object[] { knowledgeType });
                                //    }
                                //}
                                ////获取一级目录
                                //SPList list = web.Lists.TryGetList(config.knowledgeBase);
                                //var folders = list.RootFolder.SubFolders;
                                //string name = "";
                                //for (int i = 0; i < folders.Count; i++)
                                //{
                                //    if (folders[i].Name != "Forms" && folders[i].Name != "活动")
                                //    {
                                //        name += folders[i].Name + "|";
                                //    }
                                //}
                                //var arry = name.TrimEnd('|').Split('|');
                                //List<GuessYouLike> guessList = new List<GuessYouLike>();
                                //for (int j = 0; j < arry.Length; j++)
                                //{
                                //    GuessYouLike guessYouLike = new GuessYouLike();
                                //    int count = 0;
                                //    for (int i = 0; i < newDt.Rows.Count; i++)
                                //    {
                                //        if (newDt.Rows[i]["KnowledgeType"].ToString() == arry[j])
                                //        {
                                //            count = count + 1;
                                //        }
                                //    }
                                //    guessYouLike.Name = arry[j];
                                //    guessYouLike.number = count;
                                //    guessList.Add(guessYouLike);
                                //}
                                //var sorted = guessList.OrderByDescending(x => x.number);
                                //string folderName = sorted.ToList()[0].Name.ToString();
                                //SPQuery spQuery1 = new SPQuery();
                                //spQuery1.ViewAttributes = "Scope=\"Recursive\"";
                                //spQuery1.Query = "<Where><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq></Where>" +
                                //    "<OrderBy><FieldRef Name='Grade' Ascending='false' /></OrderBy>";
                                //for (int i = 0; i < folders.Count; i++)
                                //{
                                //    if (folders[i].Name == folderName)
                                //    {
                                //        spQuery1.Folder = list.RootFolder.SubFolders[folders[i].ServerRelativeUrl];
                                //    }
                                //}
                                //var guess = docList.GetItems(spQuery1);
                                //documentList = "[";
                                //for (int i = 0; i < guess.Count; i++)
                                //{
                                //    var arryNew = guess[i].Name.ToString().Split('.');
                                //    string extension = arryNew[arryNew.Length - 1];
                                //    string uploadTime = Convert.ToDateTime(guess[i]["Created"]).ToString("yyyy-MM-dd hh:MM:ss");
                                //    documentList += "{\"Id\":\"" + guess[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + guess[i].File.Title + "\",\"Author\":\"" + guess[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + guess[i]["KnowledgeType"] + "\",\"Clicks\":\"" + guess[i]["Clicks"] + "\",\"Downloads\":\"" + guess[i]["Downloads"] + "\",\"Comments\":\"" + guess[i]["Comments"] + "\",\"Grade\":\"" + guess[i]["Grade"] + "\",\"DownloadIntegral\":\"" + guess[i]["DownloadIntegral"] + "\"},";
                                //}
                                //if (documentList.Substring(documentList.Length - 1) == ",")
                                //{
                                //    documentList = documentList.TrimEnd(',');
                                //}
                                //documentList += "]";
                                break;
                            #endregion
                        }
                    }
                }
                else
                {
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {
                        SPWeb web = spSite.OpenWeb();
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        var folders = docList.RootFolder.SubFolders;
                        if (requestID == "0")
                        {
                            SPQuery spQuery1 = new SPQuery();
                            spQuery1.Folder = docList.RootFolder;
                            spQuery1.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Eq><FieldRef Name=\"FileDirRef\" /><Value Type=\"Text\">" + docList.RootFolder.ServerRelativeUrl + "</Value></Eq></And></Where>";
                            documentList = "[";
                            var folder1 = docList.GetItems(spQuery1);
                            if (folder1.Count > 0)
                            {
                                for (int i = 0; i < folder1.Count; i++)
                                {
                                    if (folder1[i].Name != "Forms")
                                        documentList += "{\"Id\":\"" + folder1[i].ID + "\",\"extension\":\"folder\",\"Title\":\"" + folder1[i].DisplayName + "\",\"Author\":\"\",\"UploadTime\":\"\",\"KnowledgeType\":\"\",\"Clicks\":\"\",\"Downloads\":\"\",\"Comments\":\"\",\"Grade\":\"\",\"DownloadIntegral\":\"\"},";
                                }
                            }
                            if (documentList.Substring(documentList.Length - 1) == ",")
                            {
                                documentList = documentList.TrimEnd(',');
                            }
                            documentList += "]";
                            listName = "大华知识库";
                        }
                        else
                        {
                            List<int> IdList = new List<int>();
                            foreach (SPFolder f in folders)
                            {
                                string fID = string.Empty;
                                if (f.Name == "Forms")
                                {
                                    fID = "-1";
                                }
                                else
                                {
                                    fID = f.Item.ID.ToString();
                                }

                                if (fID == requestID)
                                {
                                    documentList = "[";
                                    listName = f.Name;
                                    //查询传入下级是否有文档
                                    SPQuery spQuery = new SPQuery();
                                    spQuery.Folder = docList.RootFolder.SubFolders[f.ServerRelativeUrl];
                                    spQuery.Query = @"<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where><OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                                    SPListItemCollection docItem1 = docList.GetItems(spQuery);
                                    if (docItem1.Count > 0)
                                    {
                                        for (int i = 0; i < docItem1.Count; i++)
                                        {
                                            var arryNew = docItem1[i].Name.ToString().Split('.');
                                            string extension = arryNew[arryNew.Length - 1];
                                            string uploadTime = Convert.ToDateTime(docItem1[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                            string knowledgeType = "";
                                            if (docItem1[i]["KnowledgeType"].ToString().IndexOf('/') != -1)
                                            {
                                                knowledgeType = docItem1[i]["KnowledgeType"].ToString().Split('/')[1];
                                            }
                                            documentList += "{\"Id\":\"" + docItem1[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + docItem1[i].File.Title + "\",\"Author\":\"" + docItem1[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + docItem1[i]["Clicks"] + "\",\"Downloads\":\"" + docItem1[i]["Downloads"] + "\",\"Comments\":\"" + docItem1[i]["Comments"] + "\",\"Grade\":\"" + docItem1[i]["Grade"] + "\",\"DownloadIntegral\":\"" + docItem1[i]["DownloadIntegral"] + "\"},";
                                            IdList.Add(docItem1[i].ID);
                                        }

                                    }
                                    //else
                                    //{
                                    SPQuery spQuery1 = new SPQuery();
                                    spQuery1.Folder = docList.RootFolder.SubFolders[f.ServerRelativeUrl];
                                    spQuery1.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Eq><FieldRef Name=\"FileDirRef\" /><Value Type=\"Text\">" + f.ServerRelativeUrl + "</Value></Eq></And></Where>";
                                    var folder1 = docList.GetItems(spQuery1);
                                    if (folder1.Count > 0)
                                    {
                                        //documentList = "[";
                                        for (int i = 0; i < folder1.Count; i++)
                                        {
                                            if (!IdList.Contains(folder1[i].ID))
                                                documentList += "{\"Id\":\"" + folder1[i].ID + "\",\"extension\":\"folder\",\"Title\":\"" + folder1[i].DisplayName + "\",\"Author\":\"\",\"UploadTime\":\"\",\"KnowledgeType\":\"\",\"Clicks\":\"\",\"Downloads\":\"\",\"Comments\":\"\",\"Grade\":\"\",\"DownloadIntegral\":\"\"},";
                                        }
                                    }
                                    if (documentList.Substring(documentList.Length - 1) == ",")
                                    {
                                        documentList = documentList.TrimEnd(',');
                                    }
                                    documentList += "]";
                                }
                                else
                                {
                                    documentList = ClickTableRow(requestID);
                                    var item = web.Lists.TryGetList(config.knowledgeBase).GetItemById(Convert.ToInt32(requestID));
                                    parentName = item.Folder.ToString().Split('/')[1];
                                    if (parentName == f.Name)
                                    {
                                        parentId = f.Item.ID.ToString();
                                    }
                                    listName = item.Name;
                                }
                            }
                        }
                    }
                }
                documentList = documentList.Replace("\n", "<br>");
            });
        }
        public class GuessYouLike
        {
            public string Name { get; set; }
            public int number { get; set; }
        }

        [WebMethod]
        public static string ClickTableRow(string fileID)
        {
            string documentReturn = "";
            if (fileID != "index")
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {

                        List<int> IdList = new List<int>();
                        SPWeb web = spSite.OpenWeb();
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);

                        if (fileID == "0")
                        {

                            SPQuery spQuery1 = new SPQuery();
                            spQuery1.Folder = docList.RootFolder;
                            spQuery1.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Eq><FieldRef Name=\"FileDirRef\" /><Value Type=\"Text\">" + docList.RootFolder.ServerRelativeUrl + "</Value></Eq></And></Where>";
                            documentReturn = "[";
                            var folder1 = docList.GetItems(spQuery1);
                            if (folder1.Count > 0)
                            {
                                for (int i = 0; i < folder1.Count; i++)
                                {
                                    if (folder1[i].Name != "Forms")
                                        documentReturn += "{\"Id\":\"" + folder1[i].ID + "\",\"extension\":\"folder\",\"Title\":\"" + folder1[i].DisplayName + "\",\"Author\":\"\",\"UploadTime\":\"\",\"KnowledgeType\":\"\",\"Clicks\":\"\",\"Downloads\":\"\",\"Comments\":\"\",\"Grade\":\"\",\"DownloadIntegral\":\"\"},";
                                }
                            }
                            if (documentReturn.Substring(documentReturn.Length - 1) == ",")
                            {
                                documentReturn = documentReturn.TrimEnd(',');
                            }
                            documentReturn += "]";

                        }
                        else
                        {
                            var listById = docList.GetItemById(Convert.ToInt32(fileID));
                            //查询传入下级是否有文档
                            SPQuery spQuery = new SPQuery();
                            spQuery.Folder = listById.Folder;
                            spQuery.Query = @"<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Geq><FieldRef Name='Grade'/><Value Type='Double'>0</Value></Geq></And></Where>
                        <OrderBy><FieldRef Name='Created' Ascending='true' /></OrderBy>";
                            SPListItemCollection docItem1 = docList.GetItems(spQuery);
                            documentReturn = "[";
                            if (docItem1.Count > 0)
                            {
                                for (int i = 0; i < docItem1.Count; i++)
                                {
                                    var arryNew = docItem1[i].Name.ToString().Split('.');
                                    string extension = arryNew[arryNew.Length - 1];
                                    string uploadTime = Convert.ToDateTime(docItem1[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    string knowledgeType = "";
                                    if (docItem1[i]["KnowledgeType"].ToString().IndexOf('/') != -1)
                                    {
                                        knowledgeType = docItem1[i]["KnowledgeType"].ToString().Split('/')[1];
                                    }
                                    documentReturn += "{\"Id\":\"" + docItem1[i].ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + docItem1[i].File.Title + "\",\"Author\":\"" + docItem1[i]["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + docItem1[i]["Clicks"] + "\",\"Downloads\":\"" + docItem1[i]["Downloads"] + "\",\"Comments\":\"" + docItem1[i]["Comments"] + "\",\"Grade\":\"" + docItem1[i]["Grade"] + "\",\"DownloadIntegral\":\"" + docItem1[i]["DownloadIntegral"] + "\"},";
                                    IdList.Add(docItem1[i].ID);
                                }

                            }

                            SPQuery spQuery1 = new SPQuery();
                            spQuery1.Folder = listById.Folder;
                            spQuery1.Query = "<Where><And><Eq><FieldRef Name='_ModerationStatus' /><Value Type='ModStat'>0</Value></Eq><Eq><FieldRef Name=\"FileDirRef\" /><Value Type=\"Text\">" + listById.Folder.ServerRelativeUrl + "</Value></Eq></And></Where>";
                            var folder1 = docList.GetItems(spQuery1);
                            if (folder1.Count > 0)
                            {
                                for (int i = 0; i < folder1.Count; i++)
                                {
                                    if (!IdList.Contains(folder1[i].ID))
                                        documentReturn += "{\"Id\":\"" + folder1[i].ID + "\",\"extension\":\"folder\",\"Title\":\"" + folder1[i].DisplayName + "\",\"Author\":\"\",\"UploadTime\":\"\",\"KnowledgeType\":\"\",\"Clicks\":\"\",\"Downloads\":\"\",\"Comments\":\"\",\"Grade\":\"\",\"DownloadIntegral\":\"\"},";
                                }
                            }
                            if (documentReturn.Substring(documentReturn.Length - 1) == ",")
                            {
                                documentReturn = documentReturn.TrimEnd(',');
                            }
                            documentReturn += "]";
                        }
                    }
                });
            }
            return documentReturn;
        }
    }
}