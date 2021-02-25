using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
//using System.Web.Services;
using System.Web.UI;
//using System.Web.UI.WebControls;

namespace SPKnowledgeManagement.SitePages
{
    public partial class UserCenter : System.Web.UI.Page
    {
        public string userName = string.Empty;
        public string integral = string.Empty;
        public string downNumber = string.Empty;
        public string downList = string.Empty;
        public string uploadList = string.Empty;
        public string listName = string.Empty;
        public string unReadList = string.Empty;
        public string readList = string.Empty;
        public string examineList = string.Empty;
        public string param = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            //listName = Request.QueryString["ListName"];
            string param = Request.QueryString["ListName"];
            if (param == null) param = "";
            //if (!IsPostBack)
            if (param != "upload" && param != "download" && param != "sharedoc" && param != null && param != "")
            {
                Response.Write("<script language=javascript>if(confirm('参数异常，将返回前一页面')){history.go(-1);}</script>");
                return;
            }
            switch (param)
            {
                case "upload":
                    listName = "上传列表"; break;
                case "download":
                    listName = "下载列表"; break;
                case "sharedoc":
                    listName = "未读分享"; break;
                case "":
                    listName = "上传列表"; break;
            }

            GetUserInfo();
        }

        protected void GetUserInfo()
        {

            string loginAccount = "";
            SPUser currentUser;
            using (SPSite spSite = new SPSite(config.siteUrl))
            {
                SPWeb web = spSite.OpenWeb();
                loginAccount = SPTools.GetLoginUserAccount(web);
                currentUser = web.CurrentUser;
                userName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList userList = web.Lists.TryGetList(config.userList);
                    SPQuery spQueryUser = new SPQuery();
                    //指定过滤条件
                    spQueryUser.Query = "<Where><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                    SPListItemCollection userCenterInfo = userList.GetItems(spQueryUser);
                    if (userCenterInfo.Count > 0)
                    {
                        integral = userCenterInfo[0]["Integral"].ToString();
                    }
                    SPList documentList = web.Lists.TryGetList(config.knowledgeBase);
                    //下载列表
                    #region
                    SPList listDown = web.Lists.TryGetList(config.downList);
                    SPListItemCollection userDownload = listDown.GetItems(spQueryUser);
                    if (userDownload.Count > 0)
                    {
                        downNumber = userDownload.Count.ToString();
                        downList = "[";
                        for (int i = 0; i < userDownload.Count; i++)
                        {
                            var arryNew = userDownload[i]["DownFileName"].ToString().Split('.');
                            string extension = arryNew[arryNew.Length - 1];
                            int Id = Convert.ToInt32(userDownload[i]["DocumentId"]);
                            //判断大华知识库中是否还有这个文档
                            var isExisted = SPTools.IsItemExisted(Id, documentList);
                            if (isExisted)
                            {
                                string downloadTime = Convert.ToDateTime(userDownload[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                SPListItem documentListCol = documentList.GetItemById(Id);
                                string knowledgeType = "";
                                if (documentListCol["KnowledgeType"].ToString().IndexOf('/') != -1)
                                {
                                    knowledgeType = documentListCol["KnowledgeType"].ToString().Split('/')[1];
                                }
                                downList += "{\"Id\":\"" + documentListCol.ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + documentListCol.File.Title + "\",\"Author\":\"" + documentListCol["Owner"] + "\",\"DownloadTime\":\"" + downloadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + documentListCol["Clicks"] + "\",\"Downloads\":\"" + documentListCol["Downloads"] + "\",\"Comments\":\"" + documentListCol["Comments"] + "\",\"Grade\":\"" + documentListCol["Grade"] + "\",\"DownloadIntegral\":\"" + documentListCol["DownloadIntegral"] + "\"},";
                            }
                        }
                        if (downList.Substring(downList.Length - 1) == ",")
                        {
                            downList = downList.TrimEnd(',');
                        }
                        downList += "]";
                    }
                    else
                    {
                        downNumber = "0";
                        downList = "[]";
                    }
                    #endregion
                    //上传列表
                    #region
                    SPList listUpload = web.Lists.TryGetList(config.uploadList);
                    SPListItemCollection userUpload = listUpload.GetItems(spQueryUser);
                    if (userUpload.Count > 0)
                    {
                        uploadList = "[";
                        for (int i = 0; i < userUpload.Count; i++)
                        {
                            var arryNew = userUpload[i]["UploadFileName"].ToString().Split('.');
                            string extension = arryNew[arryNew.Length - 1];
                            int Id = Convert.ToInt32(userUpload[i]["DocumentId"]);
                            //判断知识库中是否还有这个文档
                            var isExisted = SPTools.IsItemExisted(Id, documentList);
                            if (isExisted)
                            {
                                SPListItem documentUploadList = documentList.GetItemById(Id);
                                if (documentUploadList["_ModerationStatus"].ToString() == "0")
                                {
                                    string uploadTime = Convert.ToDateTime(userUpload[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    string knowledgeType = "";
                                    if (documentUploadList["KnowledgeType"].ToString().IndexOf('/') != -1)
                                    {
                                        knowledgeType = documentUploadList["KnowledgeType"].ToString().Split('/')[1];
                                    }
                                    uploadList += "{\"Id\":\"" + documentUploadList.ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + documentUploadList.File.Title + "\",\"Author\":\"" + documentUploadList["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"Clicks\":\"" + documentUploadList["Clicks"] + "\",\"Downloads\":\"" + documentUploadList["Downloads"] + "\",\"Comments\":\"" + documentUploadList["Comments"] + "\",\"Grade\":\"" + documentUploadList["Grade"] + "\",\"DownloadIntegral\":\"" + documentUploadList["DownloadIntegral"] + "\"},";
                                }
                            }
                        }
                        if (uploadList.Substring(uploadList.Length - 1) == ",")
                        {
                            uploadList = uploadList.TrimEnd(',');
                        }
                        uploadList += "]";
                    }
                    else
                    {
                        uploadList = "[]";
                    }
                    #endregion
                    //分享列表
                    SPList listShare = web.Lists.TryGetList(config.docShareList);
                    #region
                    SPQuery spUnRead = new SPQuery();
                    spUnRead.Query = "<Where>" +
                                                       "<And>" +
                                                            "<Eq>" +
                                                                "<FieldRef Name='ShareAccount' /><Value Type='Text'>" + currentUser.LoginName + "</Value>" +
                                                            "</Eq>" +
                                                            "<Eq>" +
                                                                "<FieldRef Name='IsRead' /><Value Type='Boolean'>0</Value>" +
                                                            "</Eq>" +
                                                        "</And>" +
                                                 "</Where>";

                    SPListItemCollection unRead = listShare.GetItems(spUnRead);
                    if (unRead.Count > 0)
                    {
                        unReadList = "[";
                        for (int i = 0; i < unRead.Count; i++)
                        {
                            var arryNew = unRead[i]["ShareFileName"].ToString().Split('.');
                            string extension = arryNew[arryNew.Length - 1];
                            int Id = Convert.ToInt32(unRead[i]["ShareDocumentId"]);
                            //判断大华知识库中是否还有这个文档
                            var isExisted = SPTools.IsItemExisted(Id, documentList);
                            if (isExisted)
                            {
                                string shareTime = Convert.ToDateTime(unRead[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");

                                unReadList += "{\"ShareDocumentId\":\"" + unRead[i]["ShareDocumentId"] + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + unRead[i].Title + "\",\"ShareTime\":\"" + shareTime + "\",\"OwnerName\":\"" + unRead[i]["OwnerName"] + "\",\"IsRead\":\"" + unRead[i]["IsRead"] + "\"},";
                            }
                        }
                        if (unReadList.Substring(unReadList.Length - 1) == ",")
                        {
                            unReadList = unReadList.TrimEnd(',');
                        }
                        unReadList += "]";
                    }
                    else
                    {
                        unReadList = "[]";
                    }
                    #endregion
                    //所有分享
                    #region
                    SPQuery spAllRead = new SPQuery();
                    spAllRead.Query = "<Where>" +
                                                            "<Eq>" +
                                                                "<FieldRef Name='ShareAccount' /><Value Type='Text'>" + currentUser.LoginName + "</Value>" +
                                                            "</Eq>" +
                                                 "</Where>";
                    SPListItemCollection allRead = listShare.GetItems(spAllRead);
                    if (allRead.Count > 0)
                    {
                        readList = "[";
                        for (int i = 0; i < allRead.Count; i++)
                        {
                            var arryNew = allRead[i]["ShareFileName"].ToString().Split('.');
                            string extension = arryNew[arryNew.Length - 1];
                            int Id = Convert.ToInt32(allRead[i]["ShareDocumentId"]);
                            //判断大华知识库中是否还有这个文档
                            var isExisted = SPTools.IsItemExisted(Id, documentList);
                            if (isExisted)
                            {
                                string shareTime = Convert.ToDateTime(allRead[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                readList += "{\"ShareDocumentId\":\"" + allRead[i]["ShareDocumentId"] + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + allRead[i].Title + "\",\"ShareTime\":\"" + shareTime + "\",\"OwnerName\":\"" + allRead[i]["OwnerName"] + "\",\"IsRead\":\"" + allRead[i]["IsRead"] + "\"},";
                            }
                        }
                        if (readList.Substring(readList.Length - 1) == ",")
                        {
                            readList = readList.TrimEnd(',');
                        }
                        readList += "]";
                    }
                    else
                    {
                        readList = "[]";
                    }
                    #endregion
                    //未审批通过文档
                    #region
                    SPListItemCollection userUploadNoEx = listUpload.GetItems(spQueryUser);
                    if (userUploadNoEx.Count > 0)
                    {
                        examineList = "[";
                        for (int i = 0; i < userUploadNoEx.Count; i++)
                        {
                            var arryNew = userUploadNoEx[i]["UploadFileName"].ToString().Split('.');
                            string extension = arryNew[arryNew.Length - 1];
                            int Id = Convert.ToInt32(userUploadNoEx[i]["DocumentId"]);
                            //判断大华知识库中是否还有这个文档
                            var isExisted = SPTools.IsItemExisted(Id, documentList);
                            if (isExisted)
                            {
                                SPListItem documentUploadList = documentList.GetItemById(Id);
                                if (documentUploadList["_ModerationStatus"].ToString() != "0")
                                {
                                    string examineState = "";
                                    switch (documentUploadList["_ModerationStatus"].ToString())
                                    {
                                        case "1":
                                            examineState = "拒绝";
                                            break;
                                        case "2":
                                            examineState = "待定";
                                            break;
                                        case "3":
                                            examineState = "草稿";
                                            break;
                                    }
                                    string uploadTime = Convert.ToDateTime(userUploadNoEx[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                                    string knowledgeType = "";
                                    if (documentUploadList["KnowledgeType"].ToString().IndexOf('/') != -1)
                                    {
                                        knowledgeType = documentUploadList["KnowledgeType"].ToString().Split('/')[1];
                                    }
                                    examineList += "{\"Id\":\"" + documentUploadList.ID + "\",\"extension\":\"" + extension + "\",\"Title\":\"" + documentUploadList.File.Title + "\",\"Author\":\"" + documentUploadList["Owner"] + "\",\"UploadTime\":\"" + uploadTime + "\",\"KnowledgeType\":\"" + knowledgeType + "\",\"ExamineState\":\"" + examineState + "\"},";
                                }
                            }
                        }
                        if (examineList.Substring(examineList.Length - 1) == ",")
                        {
                            examineList = examineList.TrimEnd(',');
                        }
                        examineList += "]";
                    }
                    else
                    {
                        examineList = "[]";
                    }
                    #endregion
                }
            });
        }

        //[WebMethod]
        //public static void GetShareGroup(string peopleEnt, string ids)
        //{
        //    peopleEnt = peopleEnt.TrimEnd(';');
        //    var arry = peopleEnt.Split(';');
        //    ids = ids.TrimEnd(';');
        //    var arry1 = ids.Split(';');
        //    using (SPSite spSite = new SPSite(config.siteUrl))
        //    {
        //        SPWeb web = spSite.OpenWeb();
        //        SPList userList = web.Lists.TryGetList(config.userList);
        //        var user = web.Users.GetCollection(arry);
        //        var user1 = web.Users.GetByID(Convert.ToInt32(arry1[0]));
        //        SPGroupCollection collGroups = SPContext.Current.Web.Groups;
        //    }
        //}

        protected void GetShareUser(object sender, EventArgs e)
        {
            try
            {
                this.btnConfirm1.Disabled = true;
                ShowLoadingDiv.Visible = true;
                //HttpContext.Current.Response.Write("<script language=JavaScript type=text/javascript>function StartLoading(){$('#shareContent').showLoading();}</script>");
                //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>ShowShareLoading();</script>",true);
                //Response.Write("<script>function StartLoading(){ $('.modal-content').showLoading();}</script>");
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
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {

                        SPWeb web = spSite.OpenWeb();
                        web.AllowUnsafeUpdates = true;
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        //获取分享用户
                        var pickEntities = peoplePicker.ResolvedEntities;
                        this.Title = pickEntities.Count.ToString();
                        if (pickEntities.Count == 0)
                        {
                            Response.Write("<script>alert('选择用户错误，请正确选择用户');window.location.href='" + Request.Url.AbsolutePath + "?ListName=upload';</script>");
                            return;

                        }
                        var docId = docID.Value;
                        var spDoc = docList.GetItemById(Convert.ToInt32(docId));
                        string docName = "";
                        docName = spDoc.File.Name;
                        string author = "";
                        author = spDoc["Owner"].ToString();
                        string authorAccount = "";
                        authorAccount = spDoc["OwnerAccount"].ToString();
                        string[] emailReceive = new string[pickEntities.Count];
                        List<string> emailRecv = new List<string>();
                        string emailUrl = config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + docId + "";
                        for (int i = 0; i < pickEntities.Count; i++)
                        {
                            PickerEntity pickItem = pickEntities[i] as PickerEntity;
                            SPUser user = web.EnsureUser(pickItem.Key);
                            string userEmail = "";
                            if (user != null)
                                userEmail = web.EnsureUser(pickItem.Key).Email;
                            if (!string.IsNullOrEmpty(userEmail))
                            {
                                emailRecv.Add(userEmail);
                                emailReceive[i] = userEmail;
                            }
                            else
                            {
                                emailRecv.Add(config.tmpEmailReciever);
                                emailReceive[i] = config.tmpEmailReciever;
                            }
                        }
                        if (emailRecv.Count == 0)
                        {
                            Response.Write("<script>alert('知识分享邮件提醒失败，未找到被分享者的邮件，请主动通知分享人员');window.location.href='" + Request.Url.AbsolutePath + "?ListName=upload';</script></script>");
                            return;
                        }
                        SPList list = web.Lists.TryGetList(config.mailConfigList);

                        MailHelper email = new MailHelper(list.Items[0]["MailFrom"].ToString(), list.Items[0]["MailPWD"].ToString(), list.Items[0]["MailSMTP"].ToString());
                        email.mailSubject = "知识管理平台新分享知识文档";
                        string mailBody = "";
                        //                            "<p>以下为平台最新文档</p></br>";
                        string mailContentHeader = MailHelper.GeneratorMailHeader("您好，知识管理平台有新分享给您的知识文档");
                        string mailContentBody = MailHelper.GeneratorShareMailContent(docId,spDoc["Title"].ToString(),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),author,loginName,spDoc["Remark"].ToString());
                        string mailContentFooter = MailHelper.BasicFooter;
                        mailBody = mailContentHeader + mailContentBody + mailContentFooter;
                        email.mailBody = mailBody;
                        email.isbodyHtml = true;    //是否是HTML
                        //email.mailToArray = emailReceive;//接收者邮件集合
                        //email.mailToArray = emailReceive;//接收者邮件集合

                        if (emailReceive.Length > 0)
                            email.mailToArray = emailReceive;//接收者邮件集合
                        //email.mailToArray = new string[] { "" };//接收者邮件集合
                        else
                            email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合
                        //email.mailCcArray = new string[] { "xyzi@mail.yst.com.cn" };//抄送者邮件集合
                        if (email.Send("分享"))
                        {
                            Response.Write("<script>alert('邮件分享成功')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('知识分享邮件提醒失败，请主动通知分享人员')</script>");
                        }
                        SPList shareList = web.Lists.TryGetList(config.docShareList);

                        for (int i = 0; i < pickEntities.Count; i++)
                        {
                            PickerEntity pickItem = pickEntities[i] as PickerEntity;
                            string shareAccount = pickItem.Key;
                            string shareName = pickItem.DisplayText;
                            SPListItem shareItem = shareList.Items.Add();
                            shareItem["Title"] = spDoc.Title;
                            shareItem["OwnerAccount"] = loginAccount;
                            shareItem["OwnerName"] = loginName;
                            shareItem["ShareAccount"] = shareAccount;
                            shareItem["ShareName"] = shareName;
                            shareItem["ShareDocumentId"] = docId;
                            shareItem["ShareFileName"] = docName;
                            shareItem["IsRead"] = 0;
                            //web.AllowUnsafeUpdates = true;
                            shareItem.Update();
                            //web.AllowUnsafeUpdates = false;
                        }
                        Response.Write("<script>alert('站内分享成功');window.location.href='" + Request.Url.AbsolutePath + "?ListName=upload';</script>");
                        //Response.Redirect(Request.Url.AbsolutePath + "?ListName=上传列表");
                        web.AllowUnsafeUpdates = false;
                    }
                });
                //HttpContext.Current.Response.Write("<script language=JavaScript type=text/javascript>function CloseLoading(){$('#shareContent').hideLoading();}</script>");
                ShowLoadingDiv.Visible = false;
                this.btnConfirm1.Disabled = false;
                //ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript2", "<script>HideShareLoading();</script>", true);
            }
            catch (Exception ex)
            {
                ShowLoadingDiv.Visible = false;
                this.btnConfirm1.Disabled = false;

                //HttpContext.Current.Response.Write("<script language=JavaScript type=text/javascript>function CloseLoading2$('#shareContent').hideLoading();}</script>");
                //Response.Write("<script>function CloseLoading(){ $('.modal-content').hideLoading();}</script>");
                Response.Write("<script>alert('站内知识分享异常：" + ex.Message + "');window.location.href='" + Request.Url.AbsolutePath + "?ListName=upload';</script>");

            }
        }
    }
}