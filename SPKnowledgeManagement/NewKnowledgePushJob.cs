using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Threading.Tasks;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using SPKnowledgeManagement.SitePages;

namespace SPKnowledgeManagement
{
    class NewKnowledgePushJob : SPJobDefinition
    {
        private string[] memberMails;
        public NewKnowledgePushJob()
            : base()
        { }


        public NewKnowledgePushJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "KMPUSHJOB";
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }
        public override void Execute(Guid targetInstanceId)
        {
            System.Diagnostics.Trace.Assert(false);

            try
            {
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                SPContentDatabase contentDb = webApplication.ContentDatabases[targetInstanceId];

                if (config.siteName == "")
                {
                    SPList list = contentDb.Sites[0].OpenWeb().Lists.TryGetList(config.mailConfigList);

                    SPWeb web = contentDb.Sites[0].OpenWeb();
                    SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                    //获取系统时间
                    string today = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    //获取最新审批通过文档
                    SPQuery spQuery = new SPQuery();
                    spQuery.ViewAttributes = "Scope=\"Recursive\"";
                    spQuery.Query = "<Where>" +
                                                    "<Eq>" +
                                                        "<FieldRef Name='ApprovedDate' />" +
                                                        "<Value Type='Text'>" + today + "</Value>" +
                                                    "</Eq>" +
                                                "</Where>";
                    SPListItemCollection passItem = docList.GetItems(spQuery);

                    if (passItem.Count > 0)
                    {
                        string mailBody = "";
                        //                            "<p>以下为平台最新文档</p></br>";
                        string mailContentHeader = MailHelper.GeneratorMailHeader("您好，以下内容为知识管理平台最新文档推送");
                        string mailContentBody = MailHelper.GeneratorMailContent(passItem, "push");
                        string mailContentFooter = MailHelper.BasicFooter;
                        mailBody = mailContentHeader + mailContentBody + mailContentFooter;
                        SPList mailGroupList = web.Lists.TryGetList(config.mailGroup);
                        SPListItem mailItem = mailGroupList.GetItemById(1);
                        string receiver = mailItem["EmailAdd"].ToString();
                        if (receiver.Contains(";"))
                        {
                            memberMails = (receiver.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        else
                        {
                            memberMails = new string[] { receiver };
                        }

                        MailHelper email = new MailHelper(list.Items[0]["MailFrom"].ToString(), list.Items[0]["MailPWD"].ToString(), list.Items[0]["MailSMTP"].ToString());
                        email.mailSubject = "知识管理平台新文档";
                        email.mailBody = mailBody;
                        email.isbodyHtml = true;    //是否是HTML
                        if (receiver != "")
                            email.mailToArray = memberMails;//接收者邮件集合
                        else
                            email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合
                        bool result = email.Send("推送");

                    }
                }
                else
                {
                    SPList list = contentDb.Sites[config.siteName].OpenWeb().Lists.TryGetList(config.mailConfigList);
                    SPWeb web = contentDb.Sites[config.siteName].OpenWeb();
                    SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                    //获取系统时间
                    string today = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    //获取最新审批通过文档
                    SPQuery spQuery = new SPQuery();
                    spQuery.ViewAttributes = "Scope=\"Recursive\"";
                    spQuery.Query = "<Where>" +
                                                    "<Eq>" +
                                                        "<FieldRef Name='ApprovedDate' />" +
                                                        "<Value Type='Text'>" + today + "</Value>" +
                                                    "</Eq>" +
                                                "</Where>";
                    SPListItemCollection passItem = docList.GetItems(spQuery);

                    if (passItem.Count > 0)
                    {
                        string mailBody = "";
                        //                            "<p>以下为平台最新文档</p></br>";
                        string mailContentHeader = MailHelper.GeneratorMailHeader("您好，以下内容为知识管理平台最新文档推送");
                        string mailContentBody = MailHelper.GeneratorMailContent(passItem, "push");
                        string mailContentFooter = MailHelper.BasicFooter;
                        mailBody = mailContentHeader + mailContentBody + mailContentFooter;
                        SPList mailGroupList = web.Lists.TryGetList(config.mailGroup);
                        SPListItem mailItem = mailGroupList.GetItemById(1);
                        string receiver = mailItem["EmailAdd"].ToString();
                        if (receiver.Contains(";"))
                        {
                            memberMails = (receiver.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        else
                        {
                            memberMails = new string[] { receiver };
                        }

                        MailHelper email = new MailHelper(list.Items[0]["MailFrom"].ToString(), list.Items[0]["MailPWD"].ToString(), list.Items[0]["MailSMTP"].ToString());
                        email.mailSubject = "知识管理平台新文档";
                        email.mailBody = mailBody;
                        email.isbodyHtml = true;    //是否是HTML
                        if (receiver != "")
                            email.mailToArray = memberMails;//接收者邮件集合
                        else
                            email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合
                        bool result = email.Send("推送");

                    }
                }

            }
            catch (Exception ex)
            {

            }

        }
    }
}