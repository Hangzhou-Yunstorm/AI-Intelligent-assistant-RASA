using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.IO;
using Microsoft.Web.Hosting.Administration;


//using System.Web.UI.WebControls;

namespace SPKnowledgeManagement.SitePages
{
    public partial class DocumentDown : System.Web.UI.Page
    {
        public string requestID = string.Empty;
        public string docInfoJson = string.Empty;
        public string integral = string.Empty;
        public string docDowned = "no";
        public string isComment = "no";
        public string ownGrade = "0";
        public string previewUrl = "";
        public string fileName = "";
        public int SharedState = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            requestID = Request.QueryString["FileID"];
            if (!SPTools.IsNumeric(requestID))
            {
                Response.Write("<script language=javascript>if(confirm('参数异常，将返回前一页面')){history.go(-1);}</script>");
                return;
            }
            if (Request.QueryString["Shared"] != null && Request.QueryString["Shared"] != "")
                SharedState = Convert.ToInt32(Request.QueryString["Shared"]);
            if (!IsPostBack)
            {
                bool isExsited = false;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {
                        SPWeb web = spSite.OpenWeb();
                        SPList docInfoList = web.Lists.TryGetList(config.knowledgeBase);
                        isExsited = SPTools.IsItemExisted(Convert.ToInt32(requestID), docInfoList);
                    }
                });
                if (isExsited)
                    GetDocumentInfo();
                else
                {
                    Response.Write("<script language=javascript>if(confirm('文档不存在，将返回前一页面')){history.go(-1);}</script>");
                    return;
                }
            }
        }




        /// <summary>
        /// 页面加载
        /// </summary>
        protected void GetDocumentInfo()
        {
            string loginAccount = "";
            string loginName = "";
            SPUser currentUser;
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                currentUser = web.CurrentUser;
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginName = web.CurrentUser.Name;
            }
            //登录则点击数+1  临时提升权限
            #region
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    SPWeb web = site.OpenWeb();
                    SPList docInfoList1 = web.Lists.TryGetList(config.knowledgeBase);
                    web.AllowUnsafeUpdates = true;
                    var addClick = docInfoList1.GetItemById(Convert.ToInt32(requestID));
                    addClick["Clicks"] = Convert.ToInt32(addClick["Clicks"]) + 1;
                    addClick.SystemUpdate(false);



                    web.AllowUnsafeUpdates = false;
                }
            });
            #endregion
            //若是分享文档，则标记为已读  临时提升权限
            #region
            if (SharedState != 0)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                  {
                      using (SPSite site = new SPSite(config.siteUrl))
                      {
                          SPWeb web = site.OpenWeb();
                          SPList shareDocList = web.Lists.TryGetList(config.docShareList);
                          web.AllowUnsafeUpdates = true;
                          SPQuery spQuery = new SPQuery();
                          spQuery.Query = "<Where>" +
                                                             "<And>" +
                                                                  "<Eq>" +
                                                                      "<FieldRef Name='ShareAccount' /><Value Type='Text'>" + currentUser.LoginName + "</Value>" +
                                                                  "</Eq>" +
                                                                  "<Eq>" +
                                                                      "<FieldRef Name='ShareDocumentId' /><Value Type='Text'>" + requestID + "</Value>" +
                                                                  "</Eq>" +
                                                              "</And>" +
                                                       "</Where>";
                          var shareItems = shareDocList.GetItems(spQuery);
                          foreach (SPListItem share in shareItems)
                          {
                              share["IsRead"] = 1;
                              share.SystemUpdate(false);
                          }

                          web.AllowUnsafeUpdates = false;
                      }
                  });
            }
            #endregion
            #region
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList docInfoList = web.Lists.TryGetList(config.knowledgeBase);
                    try
                    {
                        var isExsited = SPTools.IsItemExisted(Convert.ToInt32(requestID), docInfoList);
                        if (isExsited)
                        {
                            var docInfo = docInfoList.GetItemById(Convert.ToInt32(requestID));
                            if (docInfo.DoesUserHavePermissions(currentUser, SPBasePermissions.ViewListItems))
                            {
                                docInfoJson = "[{\"Title\":\"" + docInfo.File.Title + "\",\"Author\":\"" + docInfo["Owner"] + "\",\"KnowledgeType\":\"" + docInfo["KnowledgeType"] + "\",\"Clicks\":\"" + docInfo["Clicks"] + "\",\"Downloads\":\"" + docInfo["Downloads"] + "\",\"Grade\":\"" + docInfo["Grade"] + "\",\"Remark\":\"" + docInfo["Remark"].ToString().Replace("\n", "<br>") + "\",\"DownloadIntegral\":\"" + docInfo["DownloadIntegral"] + "\",\"Id\":\"" + docInfo.ID + "\",\"UploadTime\":\"" + Convert.ToDateTime(docInfo["Created"]).ToString() + "\"}]";
                                string docID = docInfo.ID.ToString();
                                SPList list1 = web.Lists.TryGetList(config.userList);
                                SPQuery spQuery = new SPQuery();
                                //指定过滤条件
                                spQuery.Query = "<Where><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                                var userIntegral = list1.GetItems(spQuery);
                                if (userIntegral.Count > 0)
                                {
                                    integral = userIntegral[0]["Integral"].ToString();
                                }
                                else
                                {
                                    integral = "0";
                                }
                                //是否下载
                                SPList listDown = web.Lists.TryGetList(config.downList);
                                SPListItemCollection userDownload = listDown.GetItems(spQuery);
                                for (int i = 0; i < userDownload.Count; i++)
                                {
                                    if (userDownload[i]["DocumentId"].ToString() == requestID)
                                    {
                                        docDowned = "yes";
                                    }
                                }
                                //是否评论
                                SPList gradeList = web.Lists.TryGetList(config.gradeList);
                                SPQuery spQuery1 = new SPQuery();
                                spQuery1.Query = "<Where><And><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq>" +
                                                                                    "<Eq><FieldRef Name='DocumentId'/><Value Type='Number'>" + requestID + "</Value></Eq></And></Where>";
                                var isCommentList = gradeList.GetItems(spQuery1);
                                if (isCommentList.Count > 0)
                                {
                                    isComment = "yes";
                                    ownGrade = isCommentList[0]["Grade"].ToString();
                                }
                                else
                                {
                                    isComment = "no";
                                    ownGrade = "0";
                                }

                                try
                                {
                                    SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                                    var docItem = docList.GetItemById(Convert.ToInt32(requestID));
                                    fileName = docItem.Name;
                                    if (docItem.File.Name.Split(new char[] { '.' })[1].ToLower() == "png" || docItem.File.Name.Split(new char[] { '.' })[1].ToLower() == "jpg" || docItem.File.Name.Split(new char[] { '.' })[1].ToLower() == "gif" || docItem.File.Name.Split(new char[] { '.' })[1].ToLower() == "bmp")
                                    {
                                        previewUrl = config.siteUrl + docItem.File;
                                    }
                                    else
                                    {
                                        previewUrl = config.siteUrl + "_layouts/15/WopiFrame2.aspx?sourcedoc=" + Server.UrlEncode(config.previewSiteName) + Server.UrlEncode(docItem.File.Url) + "&action=interactivepreview&wdSmallView=1";
                                    }
                                }
                                catch (Exception e)
                                {
                                    string mess = e.Message;
                                }
                            }
                            else
                            {
                                Response.Redirect("NoPermission.html");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                    }
                }
            });
            #endregion
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="fileID">文章ID</param>
        /// <param name="comment">评论内容</param>
        /// <param name="grade">评分</param>
        /// <returns></returns>
        [WebMethod]
        public static string SendComment(string fileID, string comment, string grade)
        {
            //获取登录用户名
            string result = "";
            string loginAccount = "";
            string loginName = "";
            string loginTotalAccount = "";
            if (comment == "")
                comment = "暂无评论";
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginTotalAccount = web.CurrentUser.LoginName;
                loginName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    web.AllowUnsafeUpdates = true;
                    SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                    try
                    {
                        //获取文章名
                        var listById = docList.GetItemById(Convert.ToInt32(fileID));
                        string docTitle = listById.File.Title.ToString();
                        string authorTotalAccount = listById["OwnerAccount"].ToString();
                        string authorName = listById["Owner"].ToString();
                        #region
                        //添加评论到评论表
                        SPList userList = web.Lists.TryGetList(config.userComment);
                        SPListItem listItem = userList.Items.Add();
                        listItem["Title"] = docTitle;
                        listItem["UserAccount"] = loginAccount;
                        listItem["UserName"] = loginName;
                        listItem["Comment"] = comment;
                        listItem["DocumentId"] = fileID;
                        listItem["UserTotalAccount"] = loginTotalAccount;
                        listItem.Update();
                        #endregion
                        #region
                        //添加评分到评分表
                        SPList gradeList = web.Lists.TryGetList(config.gradeList);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq>" +
                                                                            "<Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></And></Where>";
                        SPListItemCollection docGrade = gradeList.GetItems(spQuery);
                        if (docGrade.Count == 0)
                        {
                            SPListItem listItem1 = gradeList.Items.Add();
                            listItem1["Title"] = docTitle;
                            listItem1["UserAccount"] = loginAccount;
                            listItem1["UserName"] = loginName;
                            listItem1["Grade"] = grade;
                            listItem1["DocumentId"] = fileID;
                            listItem1.Update();
                            //计算评价评分
                            Decimal grade1 = 0;
                            SPQuery spQuery1 = new SPQuery();
                            spQuery1.Query = "<Where><Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></Where>";
                            var gradeList1 = gradeList.GetItems(spQuery1);
                            if (gradeList1.Count > 0)
                            {
                                for (int i = 0; i < gradeList1.Count; i++)
                                {
                                    grade1 += Convert.ToDecimal(gradeList1[i]["Grade"]);
                                }
                                grade1 = grade1 / gradeList1.Count;
                            }
                            SPListItem listItem2 = docList.GetItemById(Convert.ToInt32(fileID));
                            listItem2["Grade"] = grade1;
                            listItem2.SystemUpdate(false);
                        }
                        web.AllowUnsafeUpdates = false;
                        result = "success";
                        #endregion
                        //#region 发送邮件评论通知至作者
                        //string authorEmail = web.EnsureUser(authorTotalAccount).Email;
                        //SPList mailConfigList = web.Lists.TryGetList(config.mailConfigList);

                        //MailHelper email = new MailHelper(SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailFrom"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailPWD"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailSMTP"]));
                        //email.mailSubject = "知识管理平台文章评论通知";
                        //string mailContent = MailHelper.GeneratorEvaluateNotification(authorName, loginName, docTitle, config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + fileID);
                        //email.mailBody = mailContent;
                        //email.isbodyHtml = true;    //是否是HTML
                        //email.mailToArray = new string[] { authorEmail };//接收者邮件集合
                        //email.Send();
                        //#endregion
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                        web.AllowUnsafeUpdates = false;
                        result = "error";
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 加载评论
        /// </summary>
        /// <param name="fileID">文章ID</param>
        /// <returns></returns>
        [WebMethod]
        public static string LoadComment(string fileID)
        {
            string resComment = "";
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    try
                    {
                        SPList userList = web.Lists.TryGetList(config.userComment);
                        SPList gradeList = web.Lists.TryGetList(config.gradeList);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></Where>" +
                                " <OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                        var commentItem = userList.GetItems(spQuery);

                        resComment = "[";
                        for (int i = 0; i < commentItem.Count; i++)
                        {
                            SPQuery spQueryGrade = new SPQuery();
                            spQueryGrade.Query = "<Where><And><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + commentItem[i]["UserAccount"] + "</Value></Eq>" +
                                                                               "<Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></And></Where>";
                            var gradeItem = gradeList.GetItems(spQueryGrade);
                            string grade = "0";
                            if (gradeItem.Count > 0)
                            {
                                grade = gradeItem[0]["Grade"].ToString();
                            }
                            resComment += "{\"Id\":\"" + commentItem[i].ID + "\",\"UserName\":\"" + commentItem[i]["UserName"] + "\",\"Grade\":\"" + grade + "\",\"Floor\":\"" + (Convert.ToInt32(commentItem.Count) - i).ToString() + "\",\"Comment\":\"" + commentItem[i]["Comment"].ToString().Replace("\n", "<br>") + "\",\"Created\":\"" + Convert.ToDateTime(commentItem[i]["Created"]).ToString("yyyy-MM-dd") + "\",\"ReplyId\":\"" + commentItem[i]["ReplyId"] + "\",\"ReplyFloor\":\"" + commentItem[i]["ReplyFloor"] + "\"},";
                        }
                        if (resComment.Substring(resComment.Length - 1) == ",")
                        {
                            resComment = resComment.TrimEnd(',');
                        }
                        resComment += "]";
                    }
                    catch (Exception e)
                    {
                        resComment = "";
                        string message = e.Message;
                    }
                }
            });
            return resComment;
        }

        /// <summary>
        /// 评论回复
        /// </summary>
        /// <param name="replyId">回复ID</param>
        /// <param name="replyContent">回复内容</param>
        /// <param name="fileID">文章ID</param>
        /// <returns></returns>
        [WebMethod]
        public static string ReplyComment(string replyId, string replyContent, string fileID)
        {
            string replyFloor = "";
            string result = "";
            string loginAccount = "";
            string loginName = "";
            string loginTotalAccount = "";
            if (replyContent == "")
                replyContent = "暂无评论";
            SPUser currentUser;
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                currentUser = web.CurrentUser;
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginTotalAccount = web.CurrentUser.LoginName;
                loginName = web.CurrentUser.Name;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite spSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = spSite.OpenWeb();
                    SPList commentsList = web.Lists.TryGetList(config.userComment);
                    web.AllowUnsafeUpdates = true;
                    var user = currentUser;
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></Where>" +
                        " <OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                    var commentItem = commentsList.GetItems(spQuery);
                    //判断回复第几楼
                    for (int i = 0; i < commentItem.Count; i++)
                    {
                        if (commentItem[i].ID.ToString() == replyId)
                        {
                            replyFloor = (Convert.ToInt32(commentItem.Count) - i).ToString();
                        }
                    }
                    try
                    {
                        //获取文章名
                        #region
                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        var listById = docList.GetItemById(Convert.ToInt32(fileID));
                        string authorTotalAccount = listById["OwnerAccount"].ToString();
                        string authorName = listById["Owner"].ToString();
                        string docTitle = listById.File.Title.ToString();
                        #endregion
                        int start = user.LoginName.IndexOf("\\");
                        SPList userList = web.Lists.TryGetList(config.userComment);
                        SPListItem listItem = userList.Items.Add();
                        listItem["Title"] = docTitle;
                        listItem["UserAccount"] = loginAccount;
                        listItem["UserTotalAccount"] = loginTotalAccount;
                        listItem["UserName"] = loginName;
                        listItem["Comment"] = replyContent;
                        listItem["DocumentId"] = fileID;
                        listItem["ReplyId"] = replyId;
                        listItem["ReplyFloor"] = replyFloor;
                        listItem.Update();
                        result = "success";
                        //#region 发送邮件评论回复通知至评论作者
                        //string docAuthorEmail = web.EnsureUser(authorTotalAccount).Email;
                        //SPListItem originComment = userList.GetItemById(Convert.ToInt32(replyId));
                        //string commentAuthorTotalAccount = originComment["UserTotalAccount"].ToString();
                        //string commentAuthorName = originComment["UserName"].ToString();
                        //string commentAuthorEmail = web.EnsureUser(commentAuthorTotalAccount).Email;
                        //SPList mailConfigList = web.Lists.TryGetList(config.mailConfigList);

                        //#region 发送至评论作者
                        //MailHelper email = new MailHelper(SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailFrom"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailPWD"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailSMTP"]));
                        //email.mailSubject = "知识管理平台文章评论回复通知";
                        //string mailContent = MailHelper.GeneratorReplyEvaluateNotification(commentAuthorName, loginName, (commentItem.Count + 1).ToString(), docTitle, config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + fileID);
                        //email.mailBody = mailContent;
                        //email.isbodyHtml = true;    //是否是HTML
                        //email.mailToArray = new string[] { commentAuthorEmail };//接收者邮件集合
                        //email.Send();
                        //#endregion
                        //#region 发送至文章作者
                        //MailHelper emailAuthor = new MailHelper(SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailFrom"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailPWD"]), SPTools.CheckStringIsNull(mailConfigList.Items[0]["MailSMTP"]));
                        //email.mailSubject = "知识管理平台文章评论回复通知";
                        //string authorMailContent = MailHelper.GeneratorReplyEvaluateNotification(authorName, loginName, (commentItem.Count + 1).ToString(), docTitle, config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + fileID);
                        //emailAuthor.mailBody = authorMailContent;
                        //emailAuthor.isbodyHtml = true;    //是否是HTML
                        //emailAuthor.mailToArray = new string[] { docAuthorEmail };//接收者邮件集合
                        //emailAuthor.Send();
                        //#endregion

                        //#endregion
                        web.AllowUnsafeUpdates = false;
                    }
                    catch (Exception e)
                    {
                        string mess = e.Message;
                        web.AllowUnsafeUpdates = false;
                        result = "error";
                    }
                }
            });
            return result;
        }



        public void AnalyseUrl()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite downSite = new SPSite(config.siteUrl))
                {
                    SPWeb web = downSite.OpenWeb();
                    SPList docInfoList = web.Lists.TryGetList(config.knowledgeBase);
                    try
                    {
                        var isExsited = SPTools.IsItemExisted(Convert.ToInt32(requestID), docInfoList);
                        if (isExsited)
                        {
                            var docInfo = docInfoList.GetItemById(Convert.ToInt32(requestID));
                            string fileName = docInfo.Name;
                            var doc = docInfo.File.OpenBinary(SPOpenBinaryOptions.Unprotected);

                            Response.ClearHeaders();
                            Response.Clear();
                            Response.Expires = 0;
                            Response.Buffer = true;
                            if (fileName.EndsWith(".docx"))
                                Response.ContentType = "Application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            else
                                Response.ContentType = "application/octet-stream";

                            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            Response.BinaryWrite(doc);
                            Response.Flush();
                            Response.Close();
                            Response.End();
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('下载出错：" + ex.Message + "')</script>");
                    }
                }
            });
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>返回文件在sharepoint上路径</returns>
        [WebMethod]
        public static string SubmitDown(string fileId)
        {

            string downUrl = "";
            var oldIntegral = 0;
            string loginAccount = "";
            string loginName = "";
            string authorAccount = "";
            SPUser currentUser;
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                currentUser = web.CurrentUser;
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginName = web.CurrentUser.Name;
            }
            //下载记录加一
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite siteIntegral = new SPSite(config.siteUrl))
                {
                    SPWeb web1 = siteIntegral.OpenWeb();
                    SPList docInfoList1 = web1.Lists.TryGetList(config.knowledgeBase);
                }
            });
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    SPWeb web1 = site.OpenWeb();
                    SPList docInfoList1 = web1.Lists.TryGetList(config.knowledgeBase);
                    web1.AllowUnsafeUpdates = true;
                    //获取用户现有积分
                    SPList userList = web1.Lists.TryGetList(config.userList);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                    var userItem = userList.GetItems(spQuery);
                    int nowIntegral = Convert.ToInt32(userItem[0]["Integral"]);
                    //记录下载前用户积分
                    oldIntegral = Convert.ToInt32(userItem[0]["Integral"]);
                    var addDown = docInfoList1.GetItemById(Convert.ToInt32(fileId));
                    authorAccount = SPTools.GetLoginUserAccount(addDown["OwnerAccount"].ToString());
                    //下载所需积分
                    int downIntegral = Convert.ToInt32(addDown["DownloadIntegral"]);
                    if (downIntegral <= nowIntegral)
                    {
                        //下载次数加一
                        addDown["Downloads"] = Convert.ToInt32(addDown["Downloads"]) + 1;
                        addDown.SystemUpdate(false);
                        //下载扣除积分
                        SPListItem listItem = userList.GetItems(spQuery)[0];
                        listItem["Integral"] = nowIntegral - downIntegral;
                        listItem.SystemUpdate(false);
                        web1.AllowUnsafeUpdates = false;
                    }
                    else
                    {
                        web1.AllowUnsafeUpdates = false;
                        downUrl = "notEnoughIntegral";
                    }
                }
            });
            if (downUrl == "notEnoughIntegral")
            {
                return downUrl;
            }
            else
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    #region
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {
                        try
                        {
                            SPWeb web = spSite.OpenWeb();
                            SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                            var docItem = docList.GetItemById(Convert.ToInt32(fileId));
                            int downIntegral = Convert.ToInt32(docItem["DownloadIntegral"]);
                            web.AllowUnsafeUpdates = true;
                            SPList userList = web.Lists.TryGetList(config.userList);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name='UserAccount'/><Value Type='Text'>" + loginAccount + "</Value></Eq></Where>";
                            var userItem = userList.GetItems(spQuery);
                            int nowIntegral = Convert.ToInt32(userItem[0]["Integral"]);
                            //判断积分是否扣除
                            var userItem1 = userList.GetItems(spQuery);
                            if (downIntegral != 0)
                            {
                                if (nowIntegral < oldIntegral)
                                {

                                    downUrl = config.siteUrl + docItem.File.Url;//路径 
                                }
                            }
                            else
                            {
                                downUrl = config.siteUrl + docItem.File.Url;//路径 
                            }
                            //向下载表添加
                            SPList downList = web.Lists.TryGetList(config.downList);
                            SPListItem downItem = downList.Items.Add();
                            downItem["Title"] = docItem.Title;
                            downItem["UserAccount"] = loginAccount;
                            downItem["AuthorAccount"] = authorAccount;
                            downItem["DownFileName"] = docItem.Name;
                            downItem["DocumentId"] = fileId;
                            downItem["UserName"] = currentUser.Name;
                            downItem["KnowledgeType"] = docItem["KnowledgeType"];
                            downItem["TotalAccount"] = currentUser.LoginName;
                            downItem["DownloadIntegral"] = downIntegral;
                            downItem.Update();
                            web.AllowUnsafeUpdates = false;


                        }
                        catch (Exception e)
                        {
                            string message = e.Message;
                            downUrl = "";
                        }
                    }
                    #endregion
                });

                return downUrl;
            }
        }
        protected void hiddenbtn_ServerClick(object sender, EventArgs e)
        {
            //string downUrl = hiddenfieldpath.Value;
            //if (!string.IsNullOrEmpty(downUrl))
            AnalyseUrl();

        }

        [WebMethod]
        public static string GetCommentNum(string fileID)
        {
            string totalNum = "0";
            string loginAccount = "";
            string loginName = "";
            using (SPSite site = new SPSite(config.siteUrl))
            {
                SPWeb web = site.OpenWeb();
                loginAccount = SPTools.GetLoginUserAccount(web);
                loginName = web.CurrentUser.Name;
            }
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(config.siteUrl))
                    {
                        SPWeb web = site.OpenWeb();
                        SPList spComment = web.Lists.TryGetList(config.userComment);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name='DocumentId'/><Value Type='Text'>" + fileID + "</Value></Eq></Where>" +
                                " <OrderBy><FieldRef Name='Created' Ascending='false' /></OrderBy>";
                        SPListItemCollection commentItems = spComment.GetItems(spQuery);
                        if (commentItems.Count > 0)
                        {
                            totalNum = commentItems.Count.ToString();
                        }
                    }
                });
                return totalNum;
            }
            catch (Exception ex)
            {
                return totalNum;
            }
        }

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
                        //this.Title = pickEntities.Count.ToString();
                        if (pickEntities.Count == 0)
                        {
                            Response.Write("<script>alert('选择用户错误，请正确选择用户');window.location.href='DocumentDown.aspx?FileID=" + requestID + "';</script>");
                            return;

                        }
                        var docId = docID.Value;
                        var spDoc = docList.GetItemById(Convert.ToInt32(docId));
                        string docName = "";
                        docName = spDoc.File.Name;
                        string author = "";
                        author = spDoc["Owner"].ToString();
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
                            Response.Write("<script>alert('知识分享邮件提醒失败，未找到被分享者的邮件，请主动通知分享人员');window.location.href='DocumentDown.aspx?FileID=" + requestID + "';</script>");
                            return;
                        }
                        SPList list = web.Lists.TryGetList(config.mailConfigList);
                        MailHelper email = new MailHelper(list.Items[0]["MailFrom"].ToString(), list.Items[0]["MailPWD"].ToString(), list.Items[0]["MailSMTP"].ToString());
                        email.mailSubject = "知识管理平台新分享知识文档";
                        string mailBody = "";
                        //                            "<p>以下为平台最新文档</p></br>";
                        string mailContentHeader = MailHelper.GeneratorMailHeader("您好，知识管理平台有新分享给您的知识文档");
                        string mailContentBody = MailHelper.GeneratorShareMailContent(docId, spDoc["Title"].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), author, loginName, spDoc["Remark"].ToString());
                        string mailContentFooter = MailHelper.BasicFooter;
                        mailBody = mailContentHeader + mailContentBody + mailContentFooter;
                        email.mailBody = mailBody;
                        email.isbodyHtml = true;    //是否是HTML

                        if (emailReceive.Length > 0)
                            email.mailToArray = emailReceive;//接收者邮件集合
                        else
                            email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合
                        if (email.Send("分享"))
                        {
                            Response.Write("<script>alert('邮件分享成功');</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('知识分享邮件提醒失败，请主动通知分享人员');</script>");
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
                        Response.Write("<script>alert('站内分享成功');window.location.href='DocumentDown.aspx?FileID=" + requestID + "'</script>");
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
                Response.Write("<script>alert('站内知识分享异常：" + ex.Message + "');window.location.href='DocumentDown.aspx?FileID=" + requestID + "'</script>");

            }
        }

    }
}