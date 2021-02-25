using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Runtime.InteropServices;
using SPKnowledgeManagement.SitePages;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
//using Microsoft.SharePoint.Client;

namespace SPKnowledgeManagement.Layouts.SPKnowledgeManagement
{
    [Guid("cacb7e9a-2053-4ab6-a965-eebbe8994eff")]
    public class FileUpload : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }
        private string siteUrl = config.siteUrl;
        private string kbTitle = config.knowledgeBase;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";//这里很关键，虽然前台数据类型是json，但这里一定要写html  
            //获取前台传来的文件  
            HttpFileCollection files = HttpContext.Current.Request.Files;
            string kTitle = HttpContext.Current.Request["title"].ToString();//标题
            string kCategory = HttpContext.Current.Request["category"].ToString();//目标目录
            string kRemark = HttpContext.Current.Request["remark"].ToString();//备注
            bool isActivity = false;//是否是活动
            if (kCategory.Contains("活动"))
                isActivity = true;
            string kAuthor = HttpContext.Current.Request["author"].ToString();//作者
            Guid destiFolderId = Guid.Parse(HttpContext.Current.Request["destiFolderId"].ToString());
            string truePath = kCategory.Replace(config.knowledgeBase, config.adminManagement);
            int outerResult = 0;//0:完全失败,   1:完全上传成功,   2:上传成功，邮件发送失败，记录插入失败, ,3:上传成功，邮件发送成功，记录插入失败 4:上传成功，邮件发送失败，记录插入成功
            int UploadState = 0;
            string UploadRemark = "";
            int MailState = 0;
            string MailRemark = "";
            int RecordState = 0;
            string RecordRemark = "";
            int uploadItemId = 0;
            SPUser currentUser;
            int downLoadIntegral = config.defaultDownIntegral;
            int upLoadIntegral = Convert.ToInt32(config.addIntegral);

            if (string.IsNullOrEmpty(kCategory))
            {
                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "0";
                resResult.ResponseRemark = "所选目录异常，请重新选择";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'fail',remark:'所选目录异常，请重新选择'}");
                return;
            }
            using (SPSite docSite = new SPSite(siteUrl))
            {
                currentUser = docSite.OpenWeb().CurrentUser;
                System.IO.Stream stm = files[0].InputStream;
                string fileName = files[0].FileName;
                if (fileName.Contains("/") || fileName.Contains("//") || fileName.Contains("\\"))
                {
                    fileName = System.IO.Path.GetFileName(fileName);//文件名  “Default.aspx”
                }
                SPWeb web = docSite.OpenWeb();
                SPList myList = web.Lists.TryGetList(kbTitle);

                try
                {
                    web.AllowUnsafeUpdates = true;
                    int iLength = (int)stm.Length;
                    if (iLength > 0)
                    {

                        SPFolder destiFolder = myList.GetItemByUniqueId(destiFolderId).Folder;
                        Byte[] filecontent = new byte[iLength];
                        stm.Read(filecontent, 0, iLength);

                        SPQuery spIsExist = new SPQuery();
                        spIsExist.Folder = destiFolder;
                        SPListItemCollection items = myList.GetItems(spIsExist);
                        if (items.Count > 0)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                if (items[i].Name == fileName)
                                {
                                    ResponseResult resResult = new ResponseResult();
                                    resResult.ResponseState = "0";
                                    resResult.ResponseRemark = "文件已存在，上传失败！";
                                    string resStr = JsonHelper.ObjToJson(resResult);
                                    context.Response.Write(resStr);
                                    return;
                                }
                            }
                        }

                        SPFile f = destiFolder.Files.Add(fileName, filecontent);
                        uploadItemId = f.Item.ID;
                        UploadState = 2;
                        stm.Close();

                    }
                    else
                    {
                        UploadState = 0;
                        UploadRemark = "文件内容为空，请重新选择";
                        ResponseResult resResult = new ResponseResult();
                        resResult.ResponseState = "0";
                        resResult.ResponseRemark = UploadRemark;
                        string resStr = JsonHelper.ObjToJson(resResult);
                        context.Response.Write(resStr);
                        web.AllowUnsafeUpdates = false;
                        //context.Response.Write("{uploadStatus:'0',remark:'" + UploadRemark + "'}");
                        return;
                    }
                }
                catch (Exception exUpload)
                {
                    web.AllowUnsafeUpdates = false;
                    if (UploadState == 0)
                    {
                        UploadRemark = "上传文件时引发错误：" + exUpload.Message;

                        ResponseResult resResult = new ResponseResult();
                        resResult.ResponseState = "0";
                        resResult.ResponseRemark = UploadRemark;
                        string resStr = JsonHelper.ObjToJson(resResult);
                        context.Response.Write(resStr);

                        //context.Response.Write("{uploadStatus:'0',remark:'" + UploadRemark + "'}");
                        return;
                    }
                    if (UploadState == 2)
                    {
                        UploadRemark = "更新文件属性时引发错误：" + exUpload.Message;

                        ResponseResult resResult = new ResponseResult();
                        resResult.ResponseState = "2";
                        resResult.ResponseRemark = UploadRemark + "，请通知管理员";
                        string resStr = JsonHelper.ObjToJson(resResult);
                        context.Response.Write(resStr);

                        //context.Response.Write("{uploadStatus:'2',remark:'" + UploadRemark + "'}");
                        return;
                    }
                }
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite docSite = new SPSite(siteUrl))
                {
                    try
                    {
                        SPWeb web = docSite.OpenWeb();
                        SPList myList = web.Lists.TryGetList(kbTitle);
                        web.AllowUnsafeUpdates = true;
                        SPList mappingList = web.Lists.TryGetList(config.folderIntegralMapList);
                        foreach (SPListItem mapItem in mappingList.Items)
                        {
                            if (kCategory.Contains(mapItem["CatalogueName"].ToString()))
                            {
                                downLoadIntegral = Convert.ToInt32(mapItem["DownloadIntegral"]);
                                break;
                            }
                        }
                        SPListItem item = myList.GetItemById(uploadItemId);
                        item["Title"] = kTitle;
                        item["KnowledgeType"] = kCategory;
                        item["Remark"] = kRemark;
                        item["Activity"] = isActivity ? 1 : 0;
                        item["DownloadIntegral"] = downLoadIntegral;
                        item["Owner"] = currentUser.Name;
                        item["OwnerAccount"] = currentUser.LoginName;

                        item.SystemUpdate(false);


                        //uploadItemId = item.ID;
                        UploadState = 1;
                        web.AllowUnsafeUpdates = false;
                    }
                    catch (Exception exUpdate)
                    {
                        if (UploadState == 2)
                        {
                            UploadRemark = "更新文件属性时引发错误：" + exUpdate.Message;

                            ResponseResult resResult = new ResponseResult();
                            resResult.ResponseState = "2";
                            resResult.ResponseRemark = UploadRemark + "，请通知管理员";
                            string resStr = JsonHelper.ObjToJson(resResult);
                            context.Response.Write(resStr);

                            //context.Response.Write("{uploadStatus:'2',remark:'" + UploadRemark + "'}");
                            return;
                        }
                    }
                }
            });
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite docSite = new SPSite(siteUrl))
                {
                    SPWeb web = docSite.OpenWeb();
                    SPList myList = web.Lists.TryGetList(kbTitle);
                    web.AllowUnsafeUpdates = true;


                    try
                    {
                        //发送邮件
                        SPFolder destiFolder = myList.GetItemByUniqueId(destiFolderId).Folder;
                        SPList approverList = docSite.OpenWeb().Lists.TryGetList(config.approverList);
                        //SPFieldUserValueCollection userValues=item["作者"] as SPFieldUserValueCollection;

                        string destiUrl = destiFolder.ServerRelativeUrl;
                        string allMail = "";
                        string[] approverMails = new string[] { };//审批者邮件集合
                        foreach (SPListItem approverItem in approverList.Items)
                        {
                            if (destiUrl.Equals(approverItem["FolderPath"].ToString()) || destiUrl.Contains(approverItem["FolderPath"].ToString()))
                            {
                                SPFieldUserValueCollection userValues = approverItem["FolderAdmin"] as SPFieldUserValueCollection;
                                foreach (SPFieldUserValue userValue in userValues)
                                {
                                    if (null != userValue.User && userValue.User.Email != "")
                                    {
                                        allMail += userValue.User.Email + ";";
                                    }
                                }
                                break;
                            }
                            //if (destiUrl.Equals(approverItem["FolderPath"].ToString()) || destiUrl.Contains(approverItem["FolderPath"].ToString()))
                            //{
                            //    allMail = approverItem["ApproverEmail"].ToString();
                            //    break;
                            //}
                        }
                        if (allMail.Contains(";") && allMail != "")
                        {
                            approverMails = (allMail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        else
                        {
                            if (allMail != "")
                                approverMails = new string[] { allMail };
                        }

                        SPList smtpList = docSite.OpenWeb().Lists.TryGetList(config.mailConfigList);
                        MailHelper email = new MailHelper(smtpList.Items[0]["MailFrom"].ToString(), smtpList.Items[0]["MailPWD"].ToString(), smtpList.Items[0]["MailSMTP"].ToString());
                        email.mailSubject = "知识管理平台待审批知识通知";
                        string mailBody = "";
                        string mailContentHeader = MailHelper.GeneratorMailHeader("您好，知识管理平台中有您待审批的知识");
                        string mailContentBody = MailHelper.GeneratorApproveMailContent(kTitle, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), currentUser.Name,kCategory,kRemark);
                        string mailContentFooter = MailHelper.BasicFooter;
                        mailBody = mailContentHeader + mailContentBody + mailContentFooter;
                        email.mailBody = mailBody;
                        email.isbodyHtml = true;    //是否是HTML
                        if (approverMails.Length > 0)
                            email.mailToArray = approverMails;//接收者邮件集合      
                        //else
                        //    email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合      
                        if (email.Send("审批"))//发送提醒邮件
                        {
                            MailState = 1;
                        }
                        else
                        {
                            MailState = 0;
                            MailRemark = "审批提醒邮件传送失败";
                        }
                    }
                    catch (Exception exMail)
                    {
                        MailState = 0;
                        MailRemark = "传递审批提醒邮件时引发错误：" + exMail.Message;
                    }
                    try
                    {
                        SPList mappingList = web.Lists.TryGetList(config.folderIntegralMapList);
                        foreach (SPListItem mapItem in mappingList.Items)
                        {
                            if (kCategory.Contains(mapItem["CatalogueName"].ToString()))
                            {
                                upLoadIntegral = Convert.ToInt32(mapItem["UploadIntegral"]);
                                break;
                            }
                        }
                        SPList uploadList = web.Lists.TryGetList(config.uploadList);
                        SPListItem newItem = uploadList.Items.Add();
                        string loginName = currentUser.LoginName;
                        int start = loginName.IndexOf("\\");
                        string loginAccount = loginName.Substring(start, loginName.Length - start).TrimStart('\\');
                        newItem["Title"] = kTitle;
                        newItem["UserAccount"] = loginAccount;
                        newItem["UploadFileName"] = files[0].FileName;
                        newItem["DocumentId"] = uploadItemId;
                        newItem["UserName"] = currentUser.Name;
                        newItem["UploadIntegral"] = upLoadIntegral;
                        newItem["KnowledgeType"] = kCategory;
                        newItem["ApproveState"] = "待审批";
                        newItem.Update();//插入上传记录
                        RecordState = 1;
                    }
                    catch (Exception exRecord)
                    {
                        RecordState = 0;
                        RecordRemark = "写入上传记录时引发错误：" + exRecord.Message;
                    }
                    web.AllowUnsafeUpdates = false;
                }
            });
            //完全失败
            if (UploadState == 0)
            {
                outerResult = 0;

                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "0";
                resResult.ResponseRemark = UploadRemark + "，请通知管理员";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'0',remark:'" + UploadRemark + "'}");
                return;
            }
            //完全成功
            if (UploadState == 1 && MailState == 1 && RecordState == 1)
            {
                outerResult = 1;

                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "1";
                resResult.ResponseRemark = "文件上传成功，请等待管理员审批";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'1',remark:'文件上传成功'}");
                return;
            }
            //上传成功，邮件失败，记录插入失败
            if (UploadState == 1 && MailState == 0 && RecordState == 0)
            {
                outerResult = 2;

                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "2";
                resResult.ResponseRemark = MailRemark + " ; " + RecordRemark + "，请通知管理员";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'2',remark:'" + MailRemark + " ; " + RecordRemark + "'}");
                return;
            }
            //上传成功，邮件成功，记录插入失败
            if (UploadState == 1 && MailState == 1 && RecordState == 0)
            {
                outerResult = 3;

                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "2";
                resResult.ResponseRemark = RecordRemark + "，请通知管理员";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'2',remark:'" + RecordRemark + "'}");
                return;
            }

            //上传成功，邮件失败，记录插入成功
            if (UploadState == 1 && MailState == 0 && RecordState == 1)
            {
                outerResult = 4;

                ResponseResult resResult = new ResponseResult();
                resResult.ResponseState = "2";
                resResult.ResponseRemark = MailRemark + "，请通知管理员";
                string resStr = JsonHelper.ObjToJson(resResult);
                context.Response.Write(resStr);

                //context.Response.Write("{uploadStatus:'2',remark:'" + MailRemark + "'}");
                return;
            }

        }
    }
}
